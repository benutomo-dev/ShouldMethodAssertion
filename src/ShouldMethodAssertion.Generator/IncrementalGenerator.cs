using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShouldMethodAssertion.Generator.Emitters;
using SourceGeneratorCommons;
using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ShouldMethodAssertion.Generator;

[Generator(LanguageNames.CSharp)]
internal sealed class IncrementalGenerator : IIncrementalGenerator
{
    record struct RedirectMarkerTypes(
        CsTypeRef ActualValueType,
        EquatableArray<CsTypeRef> TypeArgs
        );

    record struct ShouldExtensionWithProvider(
        GeneratorAttributeSyntaxContext Context,
        CsDeclarationProvider DeclarationProvider,
        CsTypeRef ShouldObjectType,
        RedirectMarkerTypes RedirectMarkerTypes,
        CsTypeRefWithAnnotation ActualValueType
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
        var csDeclarationProvider = context.CreateCsDeclarationProvider();

        var csDeclarationProviderWithExtraInfo = context.CreateCsDeclarationProvider()
            .Select(ToCsDeclarationProviderWithExtraInfo);

        var shouldObjectWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataNames.ShouldExtensionAttribute, IsTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProviderWithExtraInfo)
            .Select(ToShouldObjectWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var shouldMethodDefinitionWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataNames.ShouldMethodDefinitionAttribute, IsTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProviderWithExtraInfo)
            .Select(ToShouldMethodDefinitionWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var shouldExtensionSource = shouldObjectWithProvider
            .Select(ToShouldExtensionInput);

        var shouldObjectSource = shouldObjectWithProvider
            .Select(ToShouldObjectInput);

        var shouldObjectAssertionMethodSource = shouldObjectWithProvider
            .SelectMany(EnumerateShouldObjectAssertionMethodsInput)
            .Where(v => v.PartialDefinitionType is not null);

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

    private static ShouldExtensionWithProvider? ToShouldObjectWithProvider((GeneratorAttributeSyntaxContext, CsDeclarationProviderWithExtraInfo) args, CancellationToken cancellationToken)
    {
        var (context, (declarationProvider, redirectMakerTypes)) = args;

        var shouldObjectTypeSymbol = context.TargetSymbol as INamedTypeSymbol;

        if (shouldObjectTypeSymbol is null)
            return default;

        var actualValueTypeSymbol = context.Attributes[0].ConstructorArguments[0].Value as INamedTypeSymbol;

        DebugSGen.AssertIsNotNull(actualValueTypeSymbol);

        var rawShouldObjectType = declarationProvider.GetTypeReference(shouldObjectTypeSymbol).Type;

        // TypeArg1,.. をShouldExtension型の型パラメータに置き換えるための辞書を作成
        Dictionary<CsTypeRef, CsTypeRef>? actualValueTypeArgsRedirectDictionary = null;
        AddTypeArgsRedirect(ref actualValueTypeArgsRedirectDictionary, rawShouldObjectType, redirectMakerTypes.TypeArgs);

        // ShouldExtension属性のTypeArgsからActualValue用の型引数を作成
        var typeArgsTypedConstant = context.Attributes[0].NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.TypeArgs).Value;
        var explicitTypeArgs = BuildExlicitTypeArgs(declarationProvider, typeArgsTypedConstant, actualValueTypeArgsRedirectDictionary);

        // ShouldExtension属性で指定されたママのActualValueの型を作成
        // ※ この時点のActualValueの型はまだtype(bool)などの他に、type(IComparable<>)やtype(TypeArg1)など型パラメータが仮の状態
        var actualValueType = declarationProvider.GetTypeReference(actualValueTypeSymbol);

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

        // ActualValueの型がref structの場合は、ShouldExtensionの型もref structにすることを強制する
        if (actualValueType is { Type.TypeDefinition: CsStruct { IsRef: true } })
        {
            var refStructConvertedExtensionType = new CsStruct(
                rawShouldObjectType.TypeDefinition.Container,
                rawShouldObjectType.TypeDefinition.Name,
                (rawShouldObjectType.TypeDefinition as CsGenericDefinableTypeDeclaration)?.GenericTypeParams ?? EquatableArray<CsTypeParameterDeclaration>.Empty,
                isRef: true
                );

            shouldObjectType = rawShouldObjectType.WithTypeDefinition(refStructConvertedExtensionType);
        }

        return new(context, declarationProvider, shouldObjectType, redirectMakerTypes, actualValueType);
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

    private static ShouldExtensionInput ToShouldExtensionInput(ShouldExtensionWithProvider args, CancellationToken cancellationToken)
    {
        var stringType = args.DeclarationProvider.SpecialType.String;
        var notNullAttributeType = args.DeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.NotNullAttribute);
        var callerArgumentExpressionAttributeType = args.DeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.CallerArgumentExpressionAttribute);

        DebugSGen.AssertIsNotNull(callerArgumentExpressionAttributeType);

        return new(args.ShouldObjectType, args.ActualValueType, stringType, notNullAttributeType, callerArgumentExpressionAttributeType);
    }

    private static ShouldObjectInput ToShouldObjectInput(ShouldExtensionWithProvider args, CancellationToken _)
    {
        return new(args.ShouldObjectType, args.ActualValueType);
    }

    private static ShouldMethodDefinitionInput ToShouldMethodDefinitionInput(ShouldMethodDefinitionWithProvider args, CancellationToken cancellationToken)
    {
        return new(args.PartialDefinitionType, args.MethodParameters, args.ActualValueType);
    }

    private static IEnumerable<ShouldObjectAssertionMethodsInput> EnumerateShouldObjectAssertionMethodsInput(ShouldExtensionWithProvider args, CancellationToken cancellationToken)
    {
        var (context, declarationProvider, shouldObjectType, redirectMakerTypes, actualValueType) = args;

        var typeSymbol = context.TargetSymbol as ITypeSymbol;

        if (typeSymbol is null)
            yield break;

        var shouldMethodAttributeSymbol = declarationProvider.Compilation.GetTypeByMetadataName(MetadataNames.ShouldMethodAttribute);

        var shouldMethodDefinitionAttributeSymbol = declarationProvider.Compilation.GetTypeByMetadataName(MetadataNames.ShouldMethodDefinitionAttribute);

        var shouldMethodAttributes = typeSymbol.GetAttributes().Where(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodAttributeSymbol));

        var stringType = declarationProvider.SpecialType.String;
        var callerArgumentExpressionAttributeType = declarationProvider.GetTypeReferenceByMetadataName(MetadataNames.CallerArgumentExpressionAttribute);

        var shouldMethodDefinitionTypeTypeArgsRedirectDictionary = new Dictionary<CsTypeRef, CsTypeRef>
        {
            {redirectMakerTypes.ActualValueType, actualValueType.Type}
        };
        AddTypeArgsRedirect(ref shouldMethodDefinitionTypeTypeArgsRedirectDictionary, shouldObjectType, redirectMakerTypes.TypeArgs);

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

            var rawShouldMethodDefinitionType = declarationProvider.GetTypeReference(shouldMethodDefinitionTypeSymbol);

            var shouldMethodDefinitionAttribute = shouldMethodDefinitionTypeSymbol.GetAttributes().FirstOrDefault(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodDefinitionAttributeSymbol));

            var failedDefaultValue = new ShouldObjectAssertionMethodsInput(
                    shouldObjectType,
                    actualValueType,
                    stringType,
                    callerArgumentExpressionAttributeType,
                    rawShouldMethodDefinitionType.Type,
                    convertMethodName,
                    null,
                    EquatableArray<CsMethod>.Empty,
                    $"ソース生成に失敗しました。");

            if (shouldMethodDefinitionAttribute is null)
            {
                yield return failedDefaultValue with
                {
                    WarningMessage = $"{shouldMethodDefinitionTypeSymbol.Name}に{HintingAttributeSymbolNames.ShouldMethodDefinitionAttribute}が付与されていません。",
                };
                continue;
            }



            Dictionary<CsTypeRef, CsTypeRef>? shouldMethodDefinitionIntenalTypeRedirectDictionary = null;

            CsTypeRefWithAnnotation shouldMethodDefinitionType;

            if (explicitTypeArgs.IsDefaultOrEmpty)
            {
                if (!rawShouldMethodDefinitionType.Type.TypeArgs[0].IsDefaultOrEmpty)
                {
                    yield return failedDefaultValue with
                    {
                        WarningMessage = $"型パラメータ付きの定義に対しては{HintingAttributeSymbolNames.TypeArgs}の指定が必須です。",
                    };
                    continue;
                }

                shouldMethodDefinitionType = rawShouldMethodDefinitionType;
            }
            else
            {
                if (rawShouldMethodDefinitionType.Type.TypeArgs[0].IsDefaultOrEmpty || explicitTypeArgs.Length != rawShouldMethodDefinitionType.Type.TypeArgs[0].Length)
                {
                    yield return failedDefaultValue with
                    {
                        WarningMessage = $"{HintingAttributeSymbolNames.TypeArgs}の数が適用対象の型の型パラメータの数と一致しません。",
                    };
                    continue;
                }

                shouldMethodDefinitionType = rawShouldMethodDefinitionType.WithTypeArgs(EquatableArray.Create(explicitTypeArgs));

                shouldMethodDefinitionIntenalTypeRedirectDictionary = new Dictionary<CsTypeRef, CsTypeRef>(explicitTypeArgs.Length);

                for (int i = 0; i < shouldMethodDefinitionType.Type.TypeArgs[0].Length; i++)
                {
                    shouldMethodDefinitionIntenalTypeRedirectDictionary.Add(
                        rawShouldMethodDefinitionType.Type.TypeArgs.Values[0][i].Type,
                        shouldMethodDefinitionType.Type.TypeArgs.Values[0][i].Type
                        );
                }
            }

            var shouldMethodDefinitionActualValueTypeArgsRedirectDictionary = new Dictionary<CsTypeRef, CsTypeRef>
            {
                {redirectMakerTypes.ActualValueType, actualValueType.Type}
            };
            AddTypeArgsRedirect(ref shouldMethodDefinitionActualValueTypeArgsRedirectDictionary, shouldMethodDefinitionType.Type, redirectMakerTypes.TypeArgs);

            var shouldMethodDefinitionActualValueType = GetActualValueTypeFromShouldMethodDefinitionAttribute(args.DeclarationProvider, shouldMethodDefinitionAttribute, shouldMethodDefinitionActualValueTypeArgsRedirectDictionary);

            var shouldMethods = shouldMethodDefinitionTypeSymbol.GetMembers()
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

            yield return new(
                shouldObjectType,
                actualValueType,
                stringType,
                callerArgumentExpressionAttributeType,
                shouldMethodDefinitionType.Type,
                convertMethodName,
                shouldMethodDefinitionActualValueType,
                shouldMethods,
                null);
        }
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
    public static IEnumerable<IMethodSymbol> WhereShouldMethod(this IEnumerable<ISymbol> symbols)
    {
        return symbols
            .OfType<IMethodSymbol>()
            .Where(v => v is { IsStatic: false, MethodKind: MethodKind.Ordinary, DeclaredAccessibility: Accessibility.Public })
            .Where(v => v.Name.StartsWith(AssertMethodNames.Should, StringComparison.Ordinal));
    }
}