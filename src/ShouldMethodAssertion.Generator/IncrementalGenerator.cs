using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ShouldMethodAssertion.Generator.Emitters;
using SourceGeneratorCommons;
using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ShouldMethodAssertion.Generator;

public record struct GeneratorAttributeSymbolContext(
    INamedTypeSymbol TargetSymbol,
    Compilation Compilation,
    ImmutableArray<AttributeData> Attributes
    );

record struct BuildProperties(
    string? ShouldMethodDefinitionClass
    );

[Generator(LanguageNames.CSharp)]
internal sealed class IncrementalGenerator : IIncrementalGenerator
{

    record struct RedirectMarkerTypes(
        CsTypeRef ActualValueType,
        EquatableArray<CsTypeRef> TypeArgs
        );

    record struct ShouldObjectInfoWithProvider(
        GeneratorAttributeSyntaxContext Context,
        CsDeclarationProviderWithExtraInfo CsDeclarationProviderWithExtraInfo,
        PartialDefinitionTypeWithActualValueType ShouldObjectType,
        PartialDefinitionTypeWithActualValueType? NullableTShouldObjectType
        );

    record struct ShouldMethodDefinitionWithProvider(
        GeneratorAttributeSyntaxContext Context,
        CsDeclarationProvider DeclarationProvider,
        CsTypeRef PartialDefinitionType,
        EquatableArray<(string Name, bool MayBeNull)> MethodParameters,
        CsTypeRefWithAnnotation ActualValueType
        );

    record struct CsDeclarationProviderWithExtraInfo(
        CsDeclarationProvider DeclarationProvider,
        RedirectMarkerTypes RedirectMarkerTypes
        );

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        context.RegisterSourceOutput(context.CompilationProvider, (context, compilation) =>
        {
            var sb = new SourceBuilder(context, "_debug_info_.cs");

            sb.AppendLine($"#if false");

            sb.AppendLine($"#endif");

            sb.Commit();
        });
#endif

        var buildProperties = context.AnalyzerConfigOptionsProvider
            .SelectBuildProperties();

        var polyfillSource = context.CompilationProvider
            .Combine(buildProperties)
            .SelectMany(EnumerablePolyfills);

        context.RegisterSourceOutput(polyfillSource, PolyfillsEmitter.Emit);


        var csDeclarationProvider = context.CreateCsDeclarationProvider();

        var csDeclarationProviderWithExtraInfo = context.CreateCsDeclarationProvider()
            .Select(ToCsDeclarationProviderWithExtraInfo);

        var selfDefinedShouldObjectsWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataNames.ShouldExtensionAttribute, IsTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProviderWithExtraInfo)
            .Select(ToShouldObjectWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var refAssemblyDefinedShouldObjectsExtensionMethodInfos = context.CompilationProvider.ForAttributeWithMetadataNameInReferenceAssemblies(MetadataNames.ShouldExtensionAttribute, (v, _) => v)
            .Combine(csDeclarationProviderWithExtraInfo)
            .SelectMany(ToShouldExtensionMethodInfos)
            .Collect();

        var selfDefindedShouldShouldObjectsExtensionMethodInfos = selfDefinedShouldObjectsWithProvider
            .SelectMany(ToShouldExtensionMethodInfos)
            .Collect();

        var shouldExtensionSource = refAssemblyDefinedShouldObjectsExtensionMethodInfos
            .Combine(selfDefindedShouldShouldObjectsExtensionMethodInfos)
            .Combine(csDeclarationProvider)
            .Combine(buildProperties)
            .SelectMany(ToShouldExtensionInput);

        var shouldObjectSource = selfDefinedShouldObjectsWithProvider
            .Select(ToShouldObjectInput);

        var shouldObjectAssertionMethodSource = selfDefinedShouldObjectsWithProvider
            .SelectMany(EnumerateShouldObjectAssertionMethodsInput);


        var shouldMethodDefinitionWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataNames.ShouldMethodDefinitionAttribute, IsTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProviderWithExtraInfo)
            .Select(ToShouldMethodDefinitionWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var shouldMethodDefinitionSource = shouldMethodDefinitionWithProvider
            .Select(ToShouldMethodDefinitionInput);

        // ShouldExtension属性を付与した型に対する実装補完partial定義の出力
        context.RegisterSourceOutput(shouldObjectSource, ShouldObjectEmitter.Emit);

        // ShouldExtension属性を付与した型を返すxxx.Should()拡張メソッドの実装
        context.RegisterSourceOutput(shouldExtensionSource, ShouldExtensionEmitter.Emit);

        // ShouldExtension属性を付与した型に対するxxx.Should().BeXxxx()メソッドの実装
        context.RegisterSourceOutput(shouldObjectAssertionMethodSource, ShouldObjectAssertionMethodsEmitter.Emit);

        // ShouldMethodDefinition属性を付与した型に対する実装補完partial定義の出力
        context.RegisterSourceOutput(shouldMethodDefinitionSource, ShouldMethodDefinitionEmitter.Emit);

#if DEBUG
        context.RegisterSourceOutput(context.CompilationProvider.ForAttributeWithMetadataNameInReferenceAssemblies(MetadataNames.ShouldExtensionAttribute, (v, _) => v), (context, arg) =>
        {
            var sb = new SourceBuilder(context, $"_debug_info_{arg.TargetSymbol.MetadataName}_.cs");

            sb.AppendLine($"#if false");

            sb.AppendLine($"TargetSymbol: {arg.TargetSymbol}");
            sb.AppendLine($"Attributes: {string.Join(",", arg.Attributes.Select(v => v.AttributeClass))}");

            sb.AppendLine($"#endif");

            sb.Commit();
        });
#endif

    }

    private static IEnumerable<string> EnumerablePolyfills((Compilation, BuildProperties) arg, CancellationToken cancellationToken)
    {
        var (compilation, buildProperties) = arg;

        if (string.IsNullOrWhiteSpace(buildProperties.ShouldMethodDefinitionClass))
            yield break;

        var overloadResolutionPriorityAttributeTypes = compilation.GetTypesByMetadataName(MetadataNames.OverloadResolutionPriorityAttribute);

        if (!overloadResolutionPriorityAttributeTypes.Any(v => compilation.IsSymbolAccessibleWithin(v, compilation.Assembly)))
            yield return MetadataNames.OverloadResolutionPriorityAttribute;
    }

    private static bool IsTypeDeclarationSyntax(SyntaxNode syntaxNode, CancellationToken cancellationToken)
    {
        if (syntaxNode is not TypeDeclarationSyntax typeDeclarationSyntax)
            return false;

        return true;
    }

    private static CsDeclarationProviderWithExtraInfo ToCsDeclarationProviderWithExtraInfo(CsDeclarationProvider csDeclarationProvider, CancellationToken _)
    {
        var actualValueTypeRedirectType = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.ActualValueType);

        DebugSGen.AssertIsNotNull(actualValueTypeRedirectType);

        var typeArg1RedirectType = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.TypeArg1);
        var typeArg2RedirectType = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.TypeArg2);
        var typeArg3RedirectType = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.TypeArg3);
        var typeArg4RedirectType = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.TypeArg4);

        DebugSGen.AssertIsNotNull(typeArg1RedirectType);
        DebugSGen.AssertIsNotNull(typeArg2RedirectType);
        DebugSGen.AssertIsNotNull(typeArg3RedirectType);
        DebugSGen.AssertIsNotNull(typeArg4RedirectType);

        return new CsDeclarationProviderWithExtraInfo(csDeclarationProvider, new(actualValueTypeRedirectType, EquatableArray.Create(typeArg1RedirectType, typeArg2RedirectType, typeArg3RedirectType, typeArg4RedirectType)));
    }

    private static ShouldObjectInfoWithProvider? ToShouldObjectWithProvider((GeneratorAttributeSyntaxContext, CsDeclarationProviderWithExtraInfo) args, CancellationToken cancellationToken)
    {
        var (context, csDeclarationProviderWithExtra) = args;

        var shouldObjectTypeSymbol = context.TargetSymbol as INamedTypeSymbol;

        if (shouldObjectTypeSymbol is null)
            return default;

        var shouldObjectType = BuildShouldObjectType(csDeclarationProviderWithExtra, shouldObjectTypeSymbol, context.Attributes);

        if (TryBuildNullableTShouldObjectType(csDeclarationProviderWithExtra.DeclarationProvider, shouldObjectType, out var nullableTShouldObjectType))
            return new(context, csDeclarationProviderWithExtra, shouldObjectType, nullableTShouldObjectType);
        else
            return new(context, csDeclarationProviderWithExtra, shouldObjectType, default);
    }

    private static IEnumerable<ShouldExtensionMethodInfo> ToShouldExtensionMethodInfos(ShouldObjectInfoWithProvider args, CancellationToken cancellationToken)
    {
        var (context, csDeclarationProviderWithExtra, shouldObjectType, nullableTShouldObjectType) = args;

        var overloadResolutionPriorityTypedConstant = context.Attributes[0].NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.OverloadResolutionPriority).Value;
        int overloadResolutionPriority = (overloadResolutionPriorityTypedConstant.Value as int?) ?? 0;

        yield return new(shouldObjectType, overloadResolutionPriority);

        if (nullableTShouldObjectType.HasValue)
            yield return new(nullableTShouldObjectType.Value, overloadResolutionPriority);
    }

    private static IEnumerable<ShouldExtensionMethodInfo> ToShouldExtensionMethodInfos((GeneratorAttributeSymbolContext, CsDeclarationProviderWithExtraInfo) args, CancellationToken cancellationToken)
    {
        var (context, csDeclarationProviderWithExtra) = args;

        var shouldObjectType = BuildShouldObjectType(csDeclarationProviderWithExtra, context.TargetSymbol, context.Attributes);

        var overloadResolutionPriorityTypedConstant = context.Attributes[0].NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.OverloadResolutionPriority).Value;
        int overloadResolutionPriority = (overloadResolutionPriorityTypedConstant.Value as int?) ?? 0;

        yield return new(shouldObjectType, overloadResolutionPriority);

        if (TryBuildNullableTShouldObjectType(csDeclarationProviderWithExtra.DeclarationProvider, shouldObjectType, out var nullableTShouldObjectType))
            yield return new(nullableTShouldObjectType, overloadResolutionPriority);
    }

    private static PartialDefinitionTypeWithActualValueType BuildShouldObjectType(CsDeclarationProviderWithExtraInfo csDeclarationProviderWithExtra, INamedTypeSymbol shouldObjectTypeSymbol, ImmutableArray<AttributeData> attributes)
    {
        var actualValueTypeSymbol = attributes[0].ConstructorArguments[0].Value as INamedTypeSymbol;

        DebugSGen.AssertIsNotNull(actualValueTypeSymbol);

        var rawShouldObjectType = csDeclarationProviderWithExtra.DeclarationProvider.GetTypeReference(shouldObjectTypeSymbol).Type;

        // TypeArg1,.. をShouldExtension型の型パラメータに置き換えるための辞書を作成
        Dictionary<CsTypeRef, CsTypeRef>? actualValueTypeArgsRedirectDictionary = null;
        AddTypeArgsRedirect(ref actualValueTypeArgsRedirectDictionary, rawShouldObjectType, csDeclarationProviderWithExtra.RedirectMarkerTypes.TypeArgs);

        // ShouldExtension属性のTypeArgsからActualValue用の型引数を作成
        var typeArgsTypedConstant = attributes[0].NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.TypeArgs).Value;
        var explicitTypeArgs = BuildExlicitTypeArgs(csDeclarationProviderWithExtra.DeclarationProvider, typeArgsTypedConstant, actualValueTypeArgsRedirectDictionary);

        // ShouldExtension属性で指定されたママのActualValueの型を作成
        // ※ この時点のActualValueの型はまだtype(bool)などの他に、type(IComparable<>)やtype(TypeArg1)など型パラメータが仮の状態
        var actualValueType = csDeclarationProviderWithExtra.DeclarationProvider.GetTypeReference(actualValueTypeSymbol);

        // ActualValueの型の型引数をShouldExtension属性で指定されていた型に置換
        // ・型引数なしの場合(typeof(TypeArg1)などを含む)
        // 　　⇒ 影響なし
        // ・属性の指定が"typeof(IComparable<>), TypeArgs=[typeof(TypaArgs1)]"でShouldExtensionの型がXxx<TSomeType>の場合
        // 　　⇒ ActualValueの型はIComparable<TSomeType>
        if (!explicitTypeArgs.IsDefaultOrEmpty)
            actualValueType = actualValueType.WithTypeArgs(EquatableArray.Create(explicitTypeArgs));

        // ActualValueの型に残る型引数置換用の型(TypeArg1など)を実際の型パラメータに置換
        // ・ActualValueの型自体や型パラメータにTypeArg1などが既に含まれていない
        // 　　⇒ 影響なし
        // ・属性の指定が"typeof(TypeArg1)でShouldExtensionの型がXxx<TSomeType>の場合
        // 　　⇒ ActualValueの型はTSomeType
        if (actualValueTypeArgsRedirectDictionary is not null)
            actualValueType = actualValueType.WithTypeRedirection(actualValueTypeArgsRedirectDictionary);

        // ActualValueの型が参照型とみなされる場合、null許容の参照型とする
        // ・ActualValueの型がclass
        // 　　⇒ null許容にする
        // ・ActualValueの型がstruct
        // 　　⇒ 影響なし
        // ・ActualValueの型がstruct制約付きの型パラメータ
        // 　　⇒ 影響なし
        // ・ActualValueの型がそのほかの型パラメータ
        // 　　⇒ null許容にする
        if (actualValueType.Type.TypeDefinition is CsTypeParameterDeclaration)
        {
            var genericTypeParam = rawShouldObjectType.TypeDefinition.GenericTypeParams.Values.FirstOrDefault(v => v.Name == actualValueType.Type.TypeDefinition.Name);

            DebugSGen.AssertIsNotNull(genericTypeParam);

            if (genericTypeParam.Where?.TypeCategory != CsGenericConstraintTypeCategory.Struct)
                actualValueType = actualValueType.ToNullableIfReferenceType();
        }
        else
        {
            actualValueType = actualValueType.ToNullableIfReferenceType();
        }

        var shouldObjectType = rawShouldObjectType;

        var actualValueTypeIsRefStruct = actualValueType is { Type.TypeDefinition: CsStruct { IsRef: true } };

        // ActualValueの型がref structの場合は、ShouldExtensionの型もref structにすることを強制する
        if (actualValueTypeIsRefStruct)
        {
            var refStructConvertedExtensionType = new CsStruct(
                rawShouldObjectType.TypeDefinition.Container,
                rawShouldObjectType.TypeDefinition.Name,
                (rawShouldObjectType.TypeDefinition as CsGenericDefinableTypeDeclaration)?.GenericTypeParams ?? EquatableArray<CsTypeParameterDeclaration>.Empty,
                isRef: true
                );

            shouldObjectType = rawShouldObjectType.WithTypeDefinition(refStructConvertedExtensionType);
        }

        return new(shouldObjectType, actualValueType);
    }

    private static bool TryBuildNullableTShouldObjectType(CsDeclarationProvider csDeclarationProvider, PartialDefinitionTypeWithActualValueType shouldObjectType, out PartialDefinitionTypeWithActualValueType nullableTShouldObjectType)
    {
        // ActualValueの型がNullable<T>を除く否ref structの値型の場合はNullable<T>版のShouldObjectも作成
        if (shouldObjectType.ActualValueType.Type.TypeDefinition.IsValueType && !shouldObjectType.ActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
        {
            var nullableTIsAllowedRefStruct = csDeclarationProvider.SpecialType.NullableT.TypeDefinition.GenericTypeParams[0].Where.AllowRefStruct;

            var actualValueTypeIsRefStruct = shouldObjectType.ActualValueType is { Type.TypeDefinition: CsStruct { IsRef: true } };

            if (!actualValueTypeIsRefStruct || nullableTIsAllowedRefStruct)
            {
                var nullableTConvertedExtensionType = new CsStruct(
                    shouldObjectType.PartialDefinitionType.TypeDefinition.Container,
                    $"Nullable{shouldObjectType.PartialDefinitionType.TypeDefinition.Name}",
                    (shouldObjectType.PartialDefinitionType.TypeDefinition as CsGenericDefinableTypeDeclaration)?.GenericTypeParams ?? EquatableArray<CsTypeParameterDeclaration>.Empty,
                    accessibility: CsAccessibility.Public
                    );

                nullableTShouldObjectType = new(
                    shouldObjectType.PartialDefinitionType.WithTypeDefinition(nullableTConvertedExtensionType),
                    new(csDeclarationProvider.SpecialType.NullableT.WithTypeArgs(EquatableArray.Create(EquatableArray.Create(shouldObjectType.ActualValueType))), isNullableIfRefereceType: false)
                    );

                return true;
            }
        }

        nullableTShouldObjectType = default;
        return false;
    }

    private static ShouldMethodDefinitionWithProvider? ToShouldMethodDefinitionWithProvider((GeneratorAttributeSyntaxContext, CsDeclarationProviderWithExtraInfo) args, CancellationToken cancellationToken)
    {
        var (context, (declarationProvider, redirectMakerTypes)) = args;

        var shouldMethodDefinitionTypeSymbol = context.TargetSymbol as ITypeSymbol;

        if (shouldMethodDefinitionTypeSymbol is null)
            return default;

        var rawPartialDefinitionType = declarationProvider.GetTypeReference(shouldMethodDefinitionTypeSymbol).Type;

        Dictionary<CsTypeRef, CsTypeRef>? actualValueTypeArgsRedirectDictionary = null;
        AddTypeArgsRedirect(ref actualValueTypeArgsRedirectDictionary, rawPartialDefinitionType, redirectMakerTypes.TypeArgs);

        var actualValueType = GetActualValueTypeFromShouldMethodDefinitionAttribute(declarationProvider, context.Attributes[0], actualValueTypeArgsRedirectDictionary);

        var partialDefinitionType = rawPartialDefinitionType;

        if (actualValueType is { Type.TypeDefinition: CsStruct { IsRef: true } })
        {
            var extensionType = new CsStruct(
                rawPartialDefinitionType.TypeDefinition.Container,
                rawPartialDefinitionType.TypeDefinition.Name,
                (rawPartialDefinitionType.TypeDefinition as CsGenericDefinableTypeDeclaration)?.GenericTypeParams ?? EquatableArray<CsTypeParameterDeclaration>.Empty,
                isRef: true
                );

            partialDefinitionType = rawPartialDefinitionType.WithTypeDefinition(extensionType);
        }

        var parametersSource = shouldMethodDefinitionTypeSymbol.GetMembers()
            .WhereShouldMethod()
            .Select(v => v.Parameters.Select(param => (param.Name, param.HasExplicitDefaultValue)).ToArray())
            .ToArray();

        var parameterNameSet = parametersSource
            .SelectMany(v => v)
            .Select(v => v.Name)
            .ToHashSet();

        var parameters = ImmutableArray.CreateBuilder<(string Name, bool MayBeNull)>(parameterNameSet.Count);

        foreach (var parameterName in parameterNameSet.OrderBy(v => v))
        {
            bool mayBeNull = false;
            foreach (var parametersSourceItem in parametersSource)
            {
                var index = Array.FindIndex(parametersSourceItem, v => v.Name == parameterName);

                if (index < 0 || parametersSourceItem[index].HasExplicitDefaultValue)
                {
                    mayBeNull = true;
                    break;
                }
            }

            parameters.Add((parameterName, mayBeNull));
        }

        return new(context, declarationProvider, partialDefinitionType, parameters.MoveToImmutable(), actualValueType);
    }

    private static IEnumerable<ShouldExtensionInput> ToShouldExtensionInput((((ImmutableArray<ShouldExtensionMethodInfo>, ImmutableArray<ShouldExtensionMethodInfo>), CsDeclarationProvider), BuildProperties) args, CancellationToken cancellationToken)
    {
        var (((extensionMethodInfos1, extensionMethodInfos2), csDeclarationProvider), buildProperties) = args;

        var stringType = csDeclarationProvider.SpecialType.String;
        var notNullAttributeType = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.NotNullAttribute);
        var callerArgumentExpressionAttributeType = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.CallerArgumentExpressionAttribute);

        DebugSGen.AssertIsNotNull(callerArgumentExpressionAttributeType);

        if (string.IsNullOrWhiteSpace(buildProperties.ShouldMethodDefinitionClass))
            yield break;

        var classNameSplitterIndex = buildProperties.ShouldMethodDefinitionClass!.LastIndexOf('.');

        string? shouldMethodDefinitionNameSpace;
        string shouldMethodDefinitionClassName;
        if (classNameSplitterIndex < 0 || buildProperties.ShouldMethodDefinitionClass.AsSpan(9, classNameSplitterIndex).IsWhiteSpace())
        {
            shouldMethodDefinitionNameSpace = null;
            shouldMethodDefinitionClassName = buildProperties.ShouldMethodDefinitionClass;
        }
        else
        {
            shouldMethodDefinitionNameSpace = buildProperties.ShouldMethodDefinitionClass.Substring(0, classNameSplitterIndex);
            shouldMethodDefinitionClassName = buildProperties.ShouldMethodDefinitionClass.Substring(classNameSplitterIndex + 1);
        }

        var shouldMethodAssertionAssermbly = csDeclarationProvider.Compilation.GetFirstTypeByMetadataName(MetadataNames.ShouldExtensionAttribute)?.ContainingAssembly;

        if (SymbolEqualityComparer.Default.Equals(csDeclarationProvider.Compilation.Assembly, shouldMethodAssertionAssermbly))
        {
            // ShouldMethodAssertionに対しては拡張メソッドを生成しない
            yield return new(
                EquatableArray<ShouldExtensionMethodInfo>.Empty,
                shouldMethodDefinitionNameSpace,
                shouldMethodDefinitionClassName,
                stringType,
                notNullAttributeType,
                callerArgumentExpressionAttributeType
                );
        }
        else
        {
            var shouldExtensionMethodInfos = extensionMethodInfos1.Concat(extensionMethodInfos2).ToImmutableArray().ToEquatableArray();

            yield return new(
                shouldExtensionMethodInfos,
                shouldMethodDefinitionNameSpace,
                shouldMethodDefinitionClassName,
                stringType,
                notNullAttributeType,
                callerArgumentExpressionAttributeType
                );
        }
    }

    private static ShouldObjectInput ToShouldObjectInput(ShouldObjectInfoWithProvider args, CancellationToken _)
    {
        return new(args.ShouldObjectType, args.NullableTShouldObjectType);
    }

    private static ShouldMethodDefinitionInput ToShouldMethodDefinitionInput(ShouldMethodDefinitionWithProvider args, CancellationToken cancellationToken)
    {
        return new(args.PartialDefinitionType, args.MethodParameters, args.ActualValueType);
    }

    private static IEnumerable<ShouldObjectAssertionMethodsInput> EnumerateShouldObjectAssertionMethodsInput(ShouldObjectInfoWithProvider args, CancellationToken cancellationToken)
    {
        var (context, (declarationProvider, redirectMakerTypes), shouldObjectType, nullableTShouldObjectType) = args;

        var typeSymbol = context.TargetSymbol as ITypeSymbol;

        if (typeSymbol is null)
            yield break;

        var shouldMethodAttributeSymbol = declarationProvider.Compilation.GetFirstTypeByMetadataName(MetadataNames.ShouldMethodAttribute);

        var shouldMethodDefinitionAttributeSymbol = declarationProvider.Compilation.GetFirstTypeByMetadataName(MetadataNames.ShouldMethodDefinitionAttribute);

        if (shouldMethodDefinitionAttributeSymbol is null)
            yield break;

        var shouldMethodAttributes = typeSymbol.GetAttributes().Where(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodAttributeSymbol));

        var stringType = declarationProvider.SpecialType.String;
        var callerArgumentExpressionAttributeType = declarationProvider.GetTypeReferenceByMetadataName(MetadataNames.CallerArgumentExpressionAttribute);

        var shouldMethodDefinitionTypeTypeArgsRedirectDictionary = new Dictionary<CsTypeRef, CsTypeRef>
        {
            {redirectMakerTypes.ActualValueType, shouldObjectType.ActualValueType.Type}
        };
        AddTypeArgsRedirect(ref shouldMethodDefinitionTypeTypeArgsRedirectDictionary, shouldObjectType.PartialDefinitionType, redirectMakerTypes.TypeArgs);

        DebugSGen.AssertIsNotNull(callerArgumentExpressionAttributeType);

        foreach (var shouldMethodAttributeData in shouldMethodAttributes)
        {
            var shouldMethodDefinitionTypeSymbol = shouldMethodAttributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
            DebugSGen.AssertIsNotNull(shouldMethodDefinitionTypeSymbol);

            var convertMethodName = shouldMethodAttributeData.NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.ConvertBy).Value.Value as string;

            var typeArgsTypedConstant = shouldMethodAttributeData.NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.TypeArgs).Value;

            var explicitTypeArgs = BuildExlicitTypeArgs(declarationProvider, typeArgsTypedConstant, shouldMethodDefinitionTypeTypeArgsRedirectDictionary);

            if (shouldMethodDefinitionTypeSymbol.IsUnboundGenericType)
                shouldMethodDefinitionTypeSymbol = shouldMethodDefinitionTypeSymbol.OriginalDefinition;

            if (!TryBuildShouldObjectAssertionMethodsInfo(
                args.CsDeclarationProviderWithExtraInfo,
                shouldObjectType,
                shouldMethodDefinitionTypeSymbol,
                shouldMethodDefinitionAttributeSymbol,
                explicitTypeArgs,
                out var shouldMethodDefinitionType,
                out var shouldMethods
                ))
            {
                continue;
            }

            yield return new(
                shouldObjectType,
                shouldMethodDefinitionType,
                shouldMethods,
                stringType,
                callerArgumentExpressionAttributeType,
                convertMethodName,
                null);

            if (nullableTShouldObjectType.HasValue)
            {
                yield return new(
                    nullableTShouldObjectType.Value,
                    shouldMethodDefinitionType,
                    shouldMethods,
                    stringType,
                    callerArgumentExpressionAttributeType,
                    convertMethodName,
                    null);
            }
        }

        if (nullableTShouldObjectType.HasValue)
        {
            var nullableStructShouldHaveValueTypeSymbol = declarationProvider.Compilation.GetFirstTypeByMetadataName(MetadataNames.NullableStructShouldHaveValue);

            DebugSGen.AssertIsNotNull(nullableStructShouldHaveValueTypeSymbol);

            if (nullableStructShouldHaveValueTypeSymbol is not null)
            {
                var explicitTypeArgs = EquatableArray.Create(shouldObjectType.ActualValueType);

                if (TryBuildShouldObjectAssertionMethodsInfo(
                    args.CsDeclarationProviderWithExtraInfo,
                    shouldObjectType,
                    nullableStructShouldHaveValueTypeSymbol,
                    shouldMethodDefinitionAttributeSymbol,
                    explicitTypeArgs,
                    out var shouldMethodDefinitionType,
                    out var shouldMethods
                    ))
                {
                    yield return new(
                        nullableTShouldObjectType.Value,
                        shouldMethodDefinitionType,
                        shouldMethods,
                        stringType,
                        callerArgumentExpressionAttributeType,
                        ActualValueConvertMethodName: null,
                        null);
                }
            }
        }
    }


    private static bool TryBuildShouldObjectAssertionMethodsInfo(CsDeclarationProviderWithExtraInfo csDeclarationProviderWithExtraInfo, PartialDefinitionTypeWithActualValueType shouldObjectType, INamedTypeSymbol shouldMethodDefinitionTypeSymbol, INamedTypeSymbol shouldMethodDefinitionAttributeSymbol, EquatableArray<CsTypeRefWithAnnotation> explicitTypeArgs, out PartialDefinitionTypeWithActualValueType shouldMethodDefinitionType, out EquatableArray<CsMethod> shouldMethods)
    {
        var (declarationProvider, redirectMakerTypes) = csDeclarationProviderWithExtraInfo;

        Dictionary<CsTypeRef, CsTypeRef>? shouldMethodDefinitionIntenalTypeRedirectDictionary = null;

        shouldMethodDefinitionType = default;

        var rawShouldMethodDefinitionType = declarationProvider.GetTypeReference(shouldMethodDefinitionTypeSymbol);

        var shouldMethodDefinitionAttribute = shouldMethodDefinitionTypeSymbol.GetAttributes().FirstOrDefault(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodDefinitionAttributeSymbol));
        if (shouldMethodDefinitionAttribute is null)
        {
            DebugSGen.Fail($"{shouldMethodDefinitionTypeSymbol.Name}に{HintingAttributeSymbolNames.ShouldMethodDefinitionAttribute}が付与されていません。");
            shouldMethods = default;
            return false;
        }

        if (explicitTypeArgs.IsDefaultOrEmpty)
        {
            if (!rawShouldMethodDefinitionType.Type.TypeArgs[0].IsDefaultOrEmpty)
            {
                DebugSGen.Fail($"型パラメータ付きの定義に対しては{HintingAttributeSymbolNames.TypeArgs}の指定が必須です。");
                shouldMethods = default;
                return false;
            }

            shouldMethodDefinitionType = shouldMethodDefinitionType with { PartialDefinitionType = rawShouldMethodDefinitionType.Type };
        }
        else
        {
            if (rawShouldMethodDefinitionType.Type.TypeArgs[0].IsDefaultOrEmpty || explicitTypeArgs.Length != rawShouldMethodDefinitionType.Type.TypeArgs[0].Length)
            {
                DebugSGen.Fail($"{HintingAttributeSymbolNames.TypeArgs}の数が適用対象の型の型パラメータの数と一致しません。");
                shouldMethodDefinitionType = default;
                shouldMethods = default;
                return false;
            }

            shouldMethodDefinitionType = shouldMethodDefinitionType with { PartialDefinitionType = rawShouldMethodDefinitionType.WithTypeArgs(EquatableArray.Create(explicitTypeArgs)).Type };

            shouldMethodDefinitionIntenalTypeRedirectDictionary = new Dictionary<CsTypeRef, CsTypeRef>(explicitTypeArgs.Length);

            for (int i = 0; i < shouldMethodDefinitionType.PartialDefinitionType.TypeArgs[0].Length; i++)
            {
                shouldMethodDefinitionIntenalTypeRedirectDictionary.Add(
                    rawShouldMethodDefinitionType.Type.TypeArgs.Values[0][i].Type,
                    shouldMethodDefinitionType.PartialDefinitionType.TypeArgs.Values[0][i].Type
                    );
            }
        }

        var shouldMethodDefinitionActualValueTypeArgsRedirectDictionary = new Dictionary<CsTypeRef, CsTypeRef>
        {
            {redirectMakerTypes.ActualValueType, shouldObjectType.ActualValueType.Type}
        };
        AddTypeArgsRedirect(ref shouldMethodDefinitionActualValueTypeArgsRedirectDictionary, shouldMethodDefinitionType.PartialDefinitionType, redirectMakerTypes.TypeArgs);

        shouldMethodDefinitionType = shouldMethodDefinitionType with
        {
            ActualValueType = GetActualValueTypeFromShouldMethodDefinitionAttribute(declarationProvider, shouldMethodDefinitionAttribute, shouldMethodDefinitionActualValueTypeArgsRedirectDictionary)
        };

        shouldMethods = shouldMethodDefinitionTypeSymbol.GetMembers()
            .WhereShouldMethod()
            .Select(v =>
            {
                var method = declarationProvider.GetMethodDeclaration(v);

                if (shouldMethodDefinitionIntenalTypeRedirectDictionary is null)
                    return method;

                var remapReturnType = method.ReturnType.WithTypeRedirection(shouldMethodDefinitionIntenalTypeRedirectDictionary);

                if (!method.ReturnType.Equals(remapReturnType))
                {
                    method = method with { ReturnType = remapReturnType };
                }

                var remapedParamsBuilder = ImmutableArray.CreateBuilder<CsMethodParam>(method.Params.Length);
                foreach (var param in method.Params.Values)
                {
                    var remapedPramType = param.Type.WithTypeRedirection(shouldMethodDefinitionIntenalTypeRedirectDictionary);

                    if (param.Type.Equals(remapReturnType))
                        remapedParamsBuilder.Add(param);
                    else
                        remapedParamsBuilder.Add(param with { Type = remapedPramType });
                }

                method = method with { Params = remapedParamsBuilder.MoveToImmutable() };

                return method;
            })
            .ToImmutableArray();

        return true;
    }

    private static void AddTypeArgsRedirect([NotNullIfNotNull(nameof(typeRedirectDictionary))] ref Dictionary<CsTypeRef, CsTypeRef>? typeRedirectDictionary, CsTypeRef typeArgsRefereceType, EquatableArray<CsTypeRef> typeArgs)
    {
        if (!typeArgsRefereceType.TypeArgs[0].IsDefaultOrEmpty && !typeArgsRefereceType.TypeArgs[0].IsDefaultOrEmpty)
        {
            typeRedirectDictionary ??= new Dictionary<CsTypeRef, CsTypeRef>(typeArgsRefereceType.TypeArgs[0].Length);

            DebugSGen.Assert(typeArgsRefereceType.TypeArgs[0].Length <= typeArgs.Length);
            for (int i = 0; i < typeArgsRefereceType.TypeArgs[0].Length && i < typeArgs.Length; i++)
            {
                typeRedirectDictionary[typeArgs[i]] = typeArgsRefereceType.TypeArgs[0][i].Type;
            }
        }
    }

    private static CsTypeRefWithAnnotation GetActualValueTypeFromShouldMethodDefinitionAttribute(CsDeclarationProvider declarationProvider, AttributeData attributeData, IReadOnlyDictionary<CsTypeRef, CsTypeRef>? typeRedirectDictionary)
    {
        DebugSGen.AssertIsNotNull(attributeData.AttributeClass);
        DebugSGen.Assert(attributeData.AttributeClass.Name == HintingAttributeSymbolNames.ShouldMethodDefinitionAttribute);

        var actualValueTypeSymbol = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;

        DebugSGen.AssertIsNotNull(actualValueTypeSymbol);

        var actualValueType = declarationProvider.GetTypeReference(actualValueTypeSymbol);

        if (typeRedirectDictionary is not null)
        {
            // ShouldMethodDefinition属性で指定されたActualValueの型のTypeArg1,...をShouldMethodDefinition型自体の型引数で置き換え
            actualValueType = actualValueType.WithTypeRedirection(typeRedirectDictionary);
        }

        var acceptNullReference = (bool)(attributeData.NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.AcceptNullReference).Value.Value ?? false);

        return acceptNullReference ? actualValueType.ToNullableIfReferenceType() : actualValueType;
    }

    private static EquatableArray<CsTypeRefWithAnnotation> BuildExlicitTypeArgs(CsDeclarationProvider declarationProvider, TypedConstant typedConstant, IReadOnlyDictionary<CsTypeRef, CsTypeRef>? typeRedirectDictionary)
    {
        if (typedConstant.IsNull)
            return default;

        DebugSGen.Assert(typedConstant.Kind == TypedConstantKind.Array);

        var explicitTypeArgs = typedConstant.Values
                .Select(v => declarationProvider.GetTypeReference((ITypeSymbol)v.Value!))
                .Select(v => typeRedirectDictionary is null ? v : v.WithTypeRedirection(typeRedirectDictionary))
                .ToImmutableArray()
                .ToEquatableArray();

        return explicitTypeArgs;
    }
}

file static class FileLocalExtensions
{
    public static IncrementalValueProvider<BuildProperties> SelectBuildProperties(this IncrementalValueProvider<AnalyzerConfigOptionsProvider> analyzerConfigOptions)
    {
        return analyzerConfigOptions.Select((analyzerConfigOptionsProvider, _) =>
        {
            analyzerConfigOptionsProvider.GlobalOptions.TryGetValue(BuildPropertyNames.ShouldMethodDefinitionClass, out var shouldMethodDefinitionClassValue);

            return new BuildProperties(shouldMethodDefinitionClassValue);
        });
    }

    public static IEnumerable<IMethodSymbol> WhereShouldMethod(this IEnumerable<ISymbol> symbols)
    {
        return symbols
            .OfType<IMethodSymbol>()
            .Where(v => v is { IsStatic: false, MethodKind: MethodKind.Ordinary, DeclaredAccessibility: Accessibility.Public })
            .Where(v => v.Name.StartsWith(AssertMethodNames.Should, StringComparison.Ordinal));
    }

    public static IncrementalValuesProvider<T> ForAttributeWithMetadataNameInReferenceAssemblies<T>(this IncrementalValueProvider<Compilation> compilationProvider, string fullyQualifiedMetadataName, Func<GeneratorAttributeSymbolContext, CancellationToken, T> transform)
    {
        return compilationProvider.SelectMany((compilation, cancellationToken) =>
        {
            return QueryAttributeTypes(compilation, fullyQualifiedMetadataName)
                .Select(v => transform(v, cancellationToken));
        });
    }

    private static IEnumerable<GeneratorAttributeSymbolContext> QueryAttributeTypes(Compilation compilation, string fullyQualifiedMetadataName)
    {
        var attributeSymbol = compilation.GetFirstTypeByMetadataName(fullyQualifiedMetadataName);

        var shouldMethodAssertionAssemblySymbol = attributeSymbol?.ContainingAssembly;

        foreach (var asmRef in compilation.References)
        {
            var asmSymbol = compilation.GetAssemblyOrModuleSymbol(asmRef) as IAssemblySymbol;

            if (asmSymbol is null)
                continue;

            if (!SymbolEqualityComparer.Default.Equals(asmSymbol, shouldMethodAssertionAssemblySymbol) && !asmSymbol.Modules.Any(v => v.ReferencedAssemblySymbols.Any(v => SymbolEqualityComparer.Default.Equals(v, shouldMethodAssertionAssemblySymbol))))
                continue;

            foreach (var typeSymbol in GetAllTypeSymbols(asmSymbol.GlobalNamespace))
            {
                if (!compilation.IsSymbolAccessibleWithin(typeSymbol, compilation.Assembly))
                    continue;

                if (!typeSymbol.GetAttributes().Any(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, attributeSymbol)))
                    continue;

                yield return new(typeSymbol, compilation, typeSymbol.GetAttributes().Where(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, attributeSymbol)).ToImmutableArray());
            }
        }
    }

    private static IEnumerable<INamedTypeSymbol> GetAllTypeSymbols(INamespaceSymbol namespaceSymbol)
    {
        return namespaceSymbol.GetTypeMembers()
            .Concat(namespaceSymbol.GetNamespaceMembers().SelectMany(GetAllTypeSymbols));
    }
}