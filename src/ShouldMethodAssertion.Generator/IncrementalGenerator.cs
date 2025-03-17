using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ShouldMethodAssertion.Generator.Emitters;
using SourceGeneratorCommons;
using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;
using System.Collections.Immutable;

namespace ShouldMethodAssertion.Generator;

[Generator(LanguageNames.CSharp)]
public class IncrementalGenerator : IIncrementalGenerator
{
    record struct ShouldExtensionWithProvider(
        GeneratorAttributeSyntaxContext Context,
        CsDeclarationProvider DeclarationProvider,
        CsTypeReference PartialDefinitionType,
        CsTypeRefWithNullability ActualValueType,
        CsTypeRefWithNullability? RawActualValueType,
        EquatableArray<CsGenericTypeParam> ActualValueTypeGenericTypeParams
        );

    record struct ShouldMethodDefinitionWithProvider(
        GeneratorAttributeSyntaxContext Context,
        CsDeclarationProvider DeclarationProvider,
        CsTypeReference PartialDefinitionType,
        EquatableArray<(string Name, bool MayBeNull)> MethodParameters,
        CsTypeRefWithNullability ActualValueType
        );

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var csDeclarationProvider = context.CreateCsDeclarationProvider();

        var shouldExtensionWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataNames.ShouldExtensionAttribute, IsTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProvider)
            .Select(ToShouldExtensionWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var shouldMethodDefinitionWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataNames.ShouldMethodDefinitionAttribute, IsTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProvider)
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

    private static ShouldExtensionWithProvider? ToShouldExtensionWithProvider((GeneratorAttributeSyntaxContext context, CsDeclarationProvider declarationProvider) args, CancellationToken cancellationToken)
    {
        var (context, declarationProvider) = args;

        var shouldExtentionObjectTypeSymbol = context.TargetSymbol as ITypeSymbol;

        if (shouldExtentionObjectTypeSymbol is null)
            return default;

        var actualValueTypeSymbol = context.Attributes[0].ConstructorArguments[0].Value as INamedTypeSymbol;

        DebugSGen.AssertIsNotNull(actualValueTypeSymbol);

        var (actualValueType, rawActualValueType, actualValueTypeGenericTypeParams) = GetActualValueTypeAsNullable(args.declarationProvider, actualValueTypeSymbol);

        var rawPartialDefinitionType = declarationProvider.GetTypeReference(shouldExtentionObjectTypeSymbol).Type;

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

        return new(context, declarationProvider, partialDefinitionType, actualValueType, rawActualValueType, actualValueTypeGenericTypeParams);
    }

    private static ShouldMethodDefinitionWithProvider? ToShouldMethodDefinitionWithProvider((GeneratorAttributeSyntaxContext context, CsDeclarationProvider declarationProvider) args, CancellationToken cancellationToken)
    {
        var (context, declarationProvider) = args;

        var shouldMethodDefinitionTypeSymbol = context.TargetSymbol as ITypeSymbol;

        if (shouldMethodDefinitionTypeSymbol is null)
            return default;

        var actualValueType = GetActualValueTypeFromShouldMethodDefinitionAttribute(declarationProvider, context.Attributes[0]);

        var rawPartialDefinitionType = declarationProvider.GetTypeReference(shouldMethodDefinitionTypeSymbol).Type;

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
        var callerArgumentExpressionAttributeType = args.DeclarationProvider.GetTypeReferenceByMetadataName(MetadataNames.CallerArgumentExpressionAttribute);

        DebugSGen.AssertIsNotNull(callerArgumentExpressionAttributeType);

        return new(args.PartialDefinitionType, args.ActualValueType, args.RawActualValueType, args.ActualValueTypeGenericTypeParams, stringType, callerArgumentExpressionAttributeType);
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
        var (context, declarationProvider, extensionType, actualValueType, rawActualValueType, actualValueTypeGenericTypeParams) = args;

        var typeSymbol = context.TargetSymbol as ITypeSymbol;

        if (typeSymbol is null)
            yield break;

        var shouldMethodAttributeSymbol = declarationProvider.Compilation.GetTypeByMetadataName(MetadataNames.ShouldMethodAttribute);

        var shouldMethodDefinitionAttributeSymbol = declarationProvider.Compilation.GetTypeByMetadataName(MetadataNames.ShouldMethodDefinitionAttribute);

        var shouldMethodAttributes = typeSymbol.GetAttributes().Where(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodAttributeSymbol));

        var stringType = declarationProvider.SpecialType.String;
        var callerArgumentExpressionAttribute = declarationProvider.GetTypeReferenceByMetadataName(MetadataNames.CallerArgumentExpressionAttribute);

        DebugSGen.AssertIsNotNull(callerArgumentExpressionAttribute);

        foreach (var shouldMethodAttributeData in shouldMethodAttributes)
        {
            var shouldMethodDefinitionTypeSymbol = shouldMethodAttributeData.ConstructorArguments[0].Value as INamedTypeSymbol;

            var convertMethodName = shouldMethodAttributeData.NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.ConvertBy).Value.Value as string;

            var typeArgsTypedConstant = shouldMethodAttributeData.NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.TypeArgs).Value;

            var explicitTypeArgs = typeArgsTypedConstant.Kind == TypedConstantKind.Array
                ? typeArgsTypedConstant.Values.Select(v => args.DeclarationProvider.GetTypeReference((ITypeSymbol)v.Value!)).ToImmutableArray().ToEquatableArray()
                : default;

            DebugSGen.AssertIsNotNull(shouldMethodDefinitionTypeSymbol);

            if (shouldMethodDefinitionTypeSymbol.IsUnboundGenericType)
                shouldMethodDefinitionTypeSymbol = shouldMethodDefinitionTypeSymbol.OriginalDefinition;

            var shouldMethodDefinitionType = declarationProvider.GetTypeReference(shouldMethodDefinitionTypeSymbol);

            var shouldMethodDefinitionAttribute = shouldMethodDefinitionTypeSymbol.GetAttributes().FirstOrDefault(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodDefinitionAttributeSymbol));

            var failedDefaultValue = new ShouldObjectAssertionMethodsInput(
                    extensionType,
                    actualValueType,
                    stringType,
                    callerArgumentExpressionAttribute,
                    shouldMethodDefinitionType.Type,
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

            var shouldMethodDefinitionActualValueType = GetActualValueTypeFromShouldMethodDefinitionAttribute(args.DeclarationProvider, shouldMethodDefinitionAttribute);

            Dictionary<CsTypeReference, CsTypeReference>? typeRedirectDictionary = null;

            if (!explicitTypeArgs.IsDefaultOrEmpty)
            {
                if (shouldMethodDefinitionActualValueType.Type.TypeArgs.IsDefaultOrEmpty || explicitTypeArgs.Length != shouldMethodDefinitionActualValueType.Type.TypeArgs[0].Length)
                {
                    yield return failedDefaultValue with
                    {
                        WarningMessage = $"{HintingAttributeSymbolNames.TypeArgs}の数が適用対象の型の型パラメータの数と一致しません。",
                    };
                    continue;
                }

                typeRedirectDictionary = new(explicitTypeArgs.Length);

                for (int i = 0; i < explicitTypeArgs.Length; i++)
                {
                    typeRedirectDictionary.Add(
                        shouldMethodDefinitionActualValueType.Type.TypeArgs.Values[0][i].Type,
                        explicitTypeArgs[i].Type
                        );
                }

                shouldMethodDefinitionActualValueType = shouldMethodDefinitionActualValueType.WithTypeArgs(EquatableArray.Create(explicitTypeArgs));

                shouldMethodDefinitionType = shouldMethodDefinitionType.WithTypeRedirection(typeRedirectDictionary);
            }
            else if (!shouldMethodDefinitionActualValueType.Type.TypeArgs.IsDefaultOrEmpty && shouldMethodDefinitionActualValueType.Type.TypeArgs[0].Values.Any(v => v.Type.TypeDefinition is CsErrorType))
            {
                if (shouldMethodDefinitionType.Type.TypeArgs[0].Length != shouldMethodDefinitionActualValueType.Type.TypeArgs[0].Length)
                {
                    yield return failedDefaultValue with
                    {
                        WarningMessage = $"型パラメータの数が一致しないため、型パラメータを継承出来ません。{HintingAttributeSymbolNames.ShouldMethodAttribute}にTypeArgsの指定が必要です。",
                    };
                    continue;
                }

                typeRedirectDictionary = new(shouldMethodDefinitionType.Type.TypeArgs[0].Length);

                for (int i = 0; i < shouldMethodDefinitionType.Type.TypeArgs[0].Length; i++)
                {
                    typeRedirectDictionary.Add(
                        shouldMethodDefinitionActualValueType.Type.TypeArgs.Values[0][i].Type,
                        shouldMethodDefinitionType.Type.TypeArgs[0][i].Type
                        );
                }

                shouldMethodDefinitionActualValueType = shouldMethodDefinitionActualValueType.WithTypeArgs(shouldMethodDefinitionType.Type.TypeArgs);
            }

            var shouldMethods = shouldMethodDefinitionTypeSymbol.GetMembers()
                .WhereShouldMethod()
                .Select(v =>
                {
                    var method = declarationProvider.GetMethodDeclaration(v);

                    if (typeRedirectDictionary is null)
                        return method;

                    var remapReturnType = method.ReturnType.WithTypeRedirection(typeRedirectDictionary);

                    if (!method.ReturnType.Equals(remapReturnType))
                    {
                        method = method with { ReturnType = remapReturnType };
                    }

                    var remapedParamsBuilder = ImmutableArray.CreateBuilder<CsMethodParam>(method.Params.Length);
                    foreach (var param in method.Params.Values)
                    {
                        var remapedPramType = param.Type.WithTypeRedirection(typeRedirectDictionary);

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
                callerArgumentExpressionAttribute,
                shouldMethodDefinitionType.Type,
                convertMethodName,
                shouldMethodDefinitionActualValueType,
                shouldMethods,
                null);
        }
    }

    private static CsTypeRefWithNullability GetActualValueTypeFromShouldMethodDefinitionAttribute(CsDeclarationProvider declarationProvider, AttributeData attributeData)
    {
        DebugSGen.AssertIsNotNull(attributeData.AttributeClass);
        DebugSGen.Assert(attributeData.AttributeClass.Name == HintingAttributeSymbolNames.ShouldMethodDefinitionAttribute);

        var actualValueTypeSymbol = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;

        DebugSGen.AssertIsNotNull(actualValueTypeSymbol);

        var actualValueType = declarationProvider.GetTypeReference(actualValueTypeSymbol);

        var acceptNullReference = (bool)(attributeData.NamedArguments.FirstOrDefault(v => v.Key == HintingAttributeSymbolNames.AcceptNullReference).Value.Value ?? false);

        return acceptNullReference ? actualValueType.ToNullableIfReferenceType() : actualValueType;
    }

    private static (CsTypeRefWithNullability Type, CsTypeRefWithNullability? RawType, EquatableArray<CsGenericTypeParam> GenericTypeParams) GetActualValueTypeAsNullable(CsDeclarationProvider declarationProvider, INamedTypeSymbol actualValueTypeSymbol)
    {
        var rawActualValueType = declarationProvider.GetTypeReference(actualValueTypeSymbol);

        if (rawActualValueType.Type.TypeDefinition is CsStruct { IsRef: true })
        {
            // ref structはNullable<T>にできない
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
            .Where(v => v.Name.StartsWith("Should", StringComparison.Ordinal));
    }
}