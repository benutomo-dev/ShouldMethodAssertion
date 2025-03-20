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
    record struct ShouldExtensionWithProvider(
        GeneratorAttributeSyntaxContext Context,
        CsDeclarationProvider DeclarationProvider,
        CsTypeReference ActualValueTypeRedicertType,
        EquatableArray<CsTypeReference> TypeArgRedirectTypes,
        CsTypeReference PartialDefinitionType,
        CsTypeRefWithNullability ActualValueType
        );

    record struct ShouldMethodDefinitionWithProvider(
        GeneratorAttributeSyntaxContext Context,
        CsDeclarationProvider DeclarationProvider,
        CsTypeReference PartialDefinitionType,
        EquatableArray<(string Name, bool MayBeNull)> MethodParameters,
        CsTypeRefWithNullability ActualValueType
        );

    record struct CsDeclarationProviderWithExtraInfo(
        CsDeclarationProvider DeclarationProvider,
        EquatableArray<CsTypeReference> TypeArgs
        );

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var csDeclarationProvider = context.CreateCsDeclarationProvider();

        var csDeclarationProviderWithExtraInfo = context.CreateCsDeclarationProvider()
            .Select(ToCsDeclarationProviderWithExtraInfo);

        var shouldExtensionWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataNames.ShouldExtensionAttribute, IsTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProviderWithExtraInfo)
            .Select(ToShouldExtensionWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var shouldMethodDefinitionWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataNames.ShouldMethodDefinitionAttribute, IsTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProviderWithExtraInfo)
            .Select(ToShouldMethodDefinitionWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var shouldExtensionSource = shouldExtensionWithProvider
            .Select(ToShouldExtensionInput);

        var shouldObjectSource = shouldExtensionWithProvider
            .Select(ToShouldObjectInput);

        var shouldMethodDefinitionSource = shouldMethodDefinitionWithProvider
            .Select(ToShouldMethodDefinitionInput);

        var shouldObjectAssertionMethodSource = shouldExtensionWithProvider
            .SelectMany(EnumerateShouldObjectAssertionMethodsInput)
            .Where(v => v.PartialDefinitionType is not null);

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
        var typeArg1 = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.TypeArg1);
        var typeArg2 = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.TypeArg2);
        var typeArg3 = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.TypeArg3);
        var typeArg4 = csDeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.TypeArg4);

        DebugSGen.AssertIsNotNull(typeArg1);
        DebugSGen.AssertIsNotNull(typeArg2);
        DebugSGen.AssertIsNotNull(typeArg3);
        DebugSGen.AssertIsNotNull(typeArg4);

        return new CsDeclarationProviderWithExtraInfo(csDeclarationProvider, EquatableArray.Create(typeArg1, typeArg2, typeArg3, typeArg4));
    }

    private static ShouldExtensionWithProvider? ToShouldExtensionWithProvider((GeneratorAttributeSyntaxContext, CsDeclarationProviderWithExtraInfo) args, CancellationToken cancellationToken)
    {
        var (context, (declarationProvider, typeArgRedirectTypes)) = args;

        var shouldExtentionObjectTypeSymbol = context.TargetSymbol as INamedTypeSymbol;

        if (shouldExtentionObjectTypeSymbol is null)
            return default;

        var actualValueTypeRedirectType = declarationProvider.GetTypeReferenceByMetadataName(MetadataNames.ActualValueType);

        DebugSGen.AssertIsNotNull(actualValueTypeRedirectType);

        var actualValueTypeSymbol = context.Attributes[0].ConstructorArguments[0].Value as INamedTypeSymbol;

        DebugSGen.AssertIsNotNull(actualValueTypeSymbol);

        var rawExtensionType = declarationProvider.GetTypeReference(shouldExtentionObjectTypeSymbol).Type;

        // TypeArg1,.. をShouldExtension型の型パラメータに置き換えるための辞書を作成
        Dictionary<CsTypeReference, CsTypeReference>? actualValueTypeArgsRedirectDictionary = null;
        AddTypeArgsRedirect(ref actualValueTypeArgsRedirectDictionary, rawExtensionType, typeArgRedirectTypes);

        // ShouldExtension属性のTypeArgsからActualValue用の型引数を作成
        var typeArgsTypedConstant = context.Attributes[0].NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.TypeArgs).Value;
        var explicitTypeArgs = typeArgsTypedConstant.Kind == TypedConstantKind.Array
            ? typeArgsTypedConstant.Values
                .Select(v => declarationProvider.GetTypeReference((ITypeSymbol)v.Value!))
                .Select(v => actualValueTypeArgsRedirectDictionary is null ? v : v.WithTypeRedirection(actualValueTypeArgsRedirectDictionary))
                .ToImmutableArray()
                .ToEquatableArray()
            : default;

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
            var genericTypeParam = rawExtensionType.TypeDefinition.GenericTypeParams.Values.FirstOrDefault(v => v.Name == actualValueType.Type.TypeDefinition.Name);

            DebugSGen.AssertIsNotNull(genericTypeParam.Name);

            if (genericTypeParam.Where?.TypeCategory != CsGenericConstraintTypeCategory.Struct)
                actualValueType = actualValueType.ToNullableIfReferenceType();
        }
        else
        {
            actualValueType = actualValueType.ToNullableIfReferenceType();
        }

        var extensionType = rawExtensionType;

        // ActualValueの型がref structの場合は、ShouldExtensionの型もref structにすることを強制する
        if (actualValueType is { Type.TypeDefinition: CsStruct { IsRef: true } })
        {
            var refStructConvertedExtensionType = new CsStruct(
                rawExtensionType.TypeDefinition.Container,
                rawExtensionType.TypeDefinition.Name,
                (rawExtensionType.TypeDefinition as CsGenericDefinableTypeDeclaration)?.GenericTypeParams ?? EquatableArray<CsGenericTypeParam>.Empty,
                isRef: true
                );

            extensionType = rawExtensionType.WithTypeDefinition(refStructConvertedExtensionType);
        }

        return new(context, declarationProvider, actualValueTypeRedirectType, typeArgRedirectTypes, extensionType, actualValueType);
    }

    private static ShouldMethodDefinitionWithProvider? ToShouldMethodDefinitionWithProvider((GeneratorAttributeSyntaxContext, CsDeclarationProviderWithExtraInfo) args, CancellationToken cancellationToken)
    {
        var (context, (declarationProvider, typeArgRedirectTypes)) = args;

        var shouldMethodDefinitionTypeSymbol = context.TargetSymbol as ITypeSymbol;

        if (shouldMethodDefinitionTypeSymbol is null)
            return default;

        var rawPartialDefinitionType = declarationProvider.GetTypeReference(shouldMethodDefinitionTypeSymbol).Type;

        Dictionary<CsTypeReference, CsTypeReference>? actualValueTypeArgsRedirectDictionary = null;
        AddTypeArgsRedirect(ref actualValueTypeArgsRedirectDictionary, rawPartialDefinitionType, typeArgRedirectTypes);

        var actualValueType = GetActualValueTypeFromShouldMethodDefinitionAttribute(declarationProvider, context.Attributes[0], actualValueTypeArgsRedirectDictionary);

        var partialDefinitionType = rawPartialDefinitionType;

        if (actualValueType is { Type.TypeDefinition: CsStruct { IsRef: true } })
        {
            var extensionType = new CsStruct(
                rawPartialDefinitionType.TypeDefinition.Container,
                rawPartialDefinitionType.TypeDefinition.Name,
                (rawPartialDefinitionType.TypeDefinition as CsGenericDefinableTypeDeclaration)?.GenericTypeParams ?? EquatableArray<CsGenericTypeParam>.Empty,
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

        return new(args.PartialDefinitionType, args.ActualValueType, stringType, notNullAttributeType, callerArgumentExpressionAttributeType);
    }

    static ShouldObjectInput ToShouldObjectInput(ShouldExtensionWithProvider args, CancellationToken _)
    {
        return new(args.PartialDefinitionType, args.ActualValueType);
    }

    private static ShouldMethodDefinitionInput ToShouldMethodDefinitionInput(ShouldMethodDefinitionWithProvider args, CancellationToken cancellationToken)
    {
        return new(args.PartialDefinitionType, args.MethodParameters, args.ActualValueType);
    }

    private static IEnumerable<ShouldObjectAssertionMethodsInput> EnumerateShouldObjectAssertionMethodsInput(ShouldExtensionWithProvider args, CancellationToken cancellationToken)
    {
        var (context, declarationProvider, actualValueTypeRedirectType, typeArgRedirectTypes, extensionType, actualValueType) = args;

        var typeSymbol = context.TargetSymbol as ITypeSymbol;

        if (typeSymbol is null)
            yield break;

        var shouldMethodAttributeSymbol = declarationProvider.Compilation.GetTypeByMetadataName(MetadataNames.ShouldMethodAttribute);

        var shouldMethodDefinitionAttributeSymbol = declarationProvider.Compilation.GetTypeByMetadataName(MetadataNames.ShouldMethodDefinitionAttribute);

        var shouldMethodAttributes = typeSymbol.GetAttributes().Where(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodAttributeSymbol));

        var stringType = declarationProvider.SpecialType.String;
        var callerArgumentExpressionAttributeType = declarationProvider.GetTypeReferenceByMetadataName(MetadataNames.CallerArgumentExpressionAttribute);

        var shouldMethodDefinitionTypeTypeArgsRedirectDictionary = new Dictionary<CsTypeReference, CsTypeReference>
            {
                {actualValueTypeRedirectType, actualValueType.Type}
            };
        AddTypeArgsRedirect(ref shouldMethodDefinitionTypeTypeArgsRedirectDictionary, extensionType, typeArgRedirectTypes);

        DebugSGen.AssertIsNotNull(callerArgumentExpressionAttributeType);

        foreach (var shouldMethodAttributeData in shouldMethodAttributes)
        {
            var shouldMethodDefinitionTypeSymbol = shouldMethodAttributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
            DebugSGen.AssertIsNotNull(shouldMethodDefinitionTypeSymbol);

            var convertMethodName = shouldMethodAttributeData.NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.ConvertBy).Value.Value as string;

            var typeArgsTypedConstant = shouldMethodAttributeData.NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.TypeArgs).Value;

            var explicitTypeArgs = typeArgsTypedConstant.Kind == TypedConstantKind.Array
                ? typeArgsTypedConstant.Values.Select(v => args.DeclarationProvider.GetTypeReference((ITypeSymbol)v.Value!).WithTypeRedirection(shouldMethodDefinitionTypeTypeArgsRedirectDictionary)).ToImmutableArray().ToEquatableArray()
                : default;

            if (shouldMethodDefinitionTypeSymbol.IsUnboundGenericType)
                shouldMethodDefinitionTypeSymbol = shouldMethodDefinitionTypeSymbol.OriginalDefinition;

            var rawShouldMethodDefinitionType = declarationProvider.GetTypeReference(shouldMethodDefinitionTypeSymbol);

            var shouldMethodDefinitionAttribute = shouldMethodDefinitionTypeSymbol.GetAttributes().FirstOrDefault(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodDefinitionAttributeSymbol));

            var failedDefaultValue = new ShouldObjectAssertionMethodsInput(
                    extensionType,
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



            Dictionary<CsTypeReference, CsTypeReference>? shouldMethodDefinitionIntenalTypeRedirectDictionary = null;

            CsTypeRefWithNullability shouldMethodDefinitionType;

            if (explicitTypeArgs.IsDefaultOrEmpty)
            {
                if (!rawShouldMethodDefinitionType.Type.TypeArgs.IsDefaultOrEmpty)
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
                if (rawShouldMethodDefinitionType.Type.TypeArgs.IsDefaultOrEmpty || explicitTypeArgs.Length != rawShouldMethodDefinitionType.Type.TypeArgs[0].Length)
                {
                    yield return failedDefaultValue with
                    {
                        WarningMessage = $"{HintingAttributeSymbolNames.TypeArgs}の数が適用対象の型の型パラメータの数と一致しません。",
                    };
                    continue;
                }

                shouldMethodDefinitionType = rawShouldMethodDefinitionType.WithTypeArgs(EquatableArray.Create(explicitTypeArgs));

                shouldMethodDefinitionIntenalTypeRedirectDictionary = new Dictionary<CsTypeReference, CsTypeReference>(explicitTypeArgs.Length);

                for (int i = 0; i < shouldMethodDefinitionType.Type.TypeArgs[0].Length; i++)
                {
                    shouldMethodDefinitionIntenalTypeRedirectDictionary.Add(
                        rawShouldMethodDefinitionType.Type.TypeArgs.Values[0][i].Type,
                        shouldMethodDefinitionType.Type.TypeArgs.Values[0][i].Type
                        );
                }
            }

            var shouldMethodDefinitionActualValueTypeArgsRedirectDictionary = new Dictionary<CsTypeReference, CsTypeReference>
            {
                {actualValueTypeRedirectType, actualValueType.Type}
            };
            AddTypeArgsRedirect(ref shouldMethodDefinitionActualValueTypeArgsRedirectDictionary, shouldMethodDefinitionType.Type, typeArgRedirectTypes);

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
                extensionType,
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

    private static void AddTypeArgsRedirect([NotNullIfNotNull(nameof(typeRedirectDictionary))] ref Dictionary<CsTypeReference, CsTypeReference>? typeRedirectDictionary, CsTypeReference typeArgsRefereceType, EquatableArray<CsTypeReference> typeArgs)
    {
        if (!typeArgsRefereceType.TypeArgs.IsDefaultOrEmpty && !typeArgsRefereceType.TypeArgs[0].IsDefaultOrEmpty)
        {
            typeRedirectDictionary ??= new Dictionary<CsTypeReference, CsTypeReference>(typeArgsRefereceType.TypeArgs[0].Length);

            DebugSGen.Assert(typeArgsRefereceType.TypeArgs[0].Length <= typeArgs.Length);
            for (int i = 0; i < typeArgsRefereceType.TypeArgs[0].Length && i < typeArgs.Length; i++)
            {
                typeRedirectDictionary[typeArgs[i]] = typeArgsRefereceType.TypeArgs[0][i].Type;
            }
        }
    }

    private static CsTypeRefWithNullability GetActualValueTypeFromShouldMethodDefinitionAttribute(CsDeclarationProvider declarationProvider, AttributeData attributeData, IReadOnlyDictionary<CsTypeReference, CsTypeReference>? typeRedirectDictionary)
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

    [Obsolete("今はNullable<T>は専用のShouldのみを用意し、値型に対するNullable<T>版の拡張メソッドは生やさないことにしたので一旦非推奨")]
    private static (CsTypeRefWithNullability Type, CsTypeRefWithNullability? RawType, EquatableArray<CsGenericTypeParam> GenericTypeParams) GetActualValueTypeAsNullable(CsDeclarationProvider declarationProvider, INamedTypeSymbol actualValueTypeSymbol)
    {
        var rawActualValueType = declarationProvider.GetTypeReference(actualValueTypeSymbol);

        if (rawActualValueType.Type.TypeDefinition is CsStruct { IsRef: true })
        {
            // ref structはNullable<T>にできない
            return (rawActualValueType, null, rawActualValueType.Type.TypeDefinition.GenericTypeParams);
        }
        else if (rawActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
        {
            // 元の型自体がNullable<T>
            return (rawActualValueType, null, rawActualValueType.Type.TypeDefinition.GenericTypeParams);
        }
        else
        {
            var nullableType = declarationProvider.MakeNullableTypeReference(rawActualValueType);

            if (nullableType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
                return (nullableType, rawActualValueType, rawActualValueType.Type.TypeDefinition.GenericTypeParams);
            else if (actualValueTypeSymbol.IsUnboundGenericType)
                return (nullableType, null, rawActualValueType.Type.TypeDefinition.GenericTypeParams);
            else
                return (nullableType, null, EquatableArray<CsGenericTypeParam>.Empty);
        }
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