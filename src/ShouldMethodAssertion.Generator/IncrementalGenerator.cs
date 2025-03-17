using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGeneratorCommons;
using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;
using System.Collections.Immutable;

namespace ShouldMethodAssertion.Generator;

[Generator(LanguageNames.CSharp)]
public class IncrementalGenerator : IIncrementalGenerator
{
    private const string ShouldAssertionContextMetadataName = "ShouldMethodAssertion.ShouldAssertionContexts.ShouldAssertionContext`1";

    private const string ShouldExtensionAttributeMetadataName = "ShouldMethodAssertion.DataAnnotations.ShouldExtensionAttribute";

    private const string ShouldMethodAttributeMetadataName = "ShouldMethodAssertion.DataAnnotations.ShouldMethodAttribute";

    private const string ShouldMethodDefinitionAttributeMetadataName = "ShouldMethodAssertion.DataAnnotations.ShouldMethodDefinitionAttribute";

    private const string CallerArgumentExpressionAttributeMetadataName = "System.Runtime.CompilerServices.CallerArgumentExpressionAttribute";

    private const string ExceptionCreateCall = "global::ShouldMethodAssertion.ShouldMethodDefinitions.AssertExceptionUtil.Create";

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
        CsTypeRefWithNullability ActualValueType
        );


    record struct ShouldObjectAndExtensionInput(
        CsTypeReference PartialDefinitionType,
        CsTypeRefWithNullability ActualValueType,
        CsTypeRefWithNullability? RawActualValueType,
        EquatableArray<CsGenericTypeParam> ActualValueTypeGenericTypeParams,
        CsTypeReference StringType,
        CsTypeReference CallerArgumentExpressionAttributeType
        );

    record struct ShouldMethodDefinitionInput(
        CsTypeReference PartialDefinitionType,
        CsTypeRefWithNullability ActualValueType,
        CsTypeReference ShouldAssertionContextType,
        bool IsGeneratedShouldAssertionContextType
        );

    record struct ShouldRefStructAssertionContextTypeInput(CsTypeReference ShouldAssertionContextType, CsTypeRefWithNullability ActualValueType);

    record struct ShouldObjectAssertionMethodsInput(
        CsTypeReference PartialDefinitionType,
        CsTypeRefWithNullability ShouldObjectActualValueType,
        CsTypeReference StringType,
        CsTypeReference CallerArgumentExpressionAttributeType,
        CsTypeReference ShouldMethodDefinitionType,
        string? ActualValueConvertMethodName,

        CsTypeRefWithNullability? ShouldMethodDefinitionActualValueType,
        CsTypeReference? ShouldAssertionContextType,
        EquatableArray<CsMethod> ShouldMethods,
        string? WarningMessage);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var csDeclarationProvider = context.CreateCsDeclarationProvider();

        var shouldExtensionWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(ShouldExtensionAttributeMetadataName, isTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProvider)
            .Select(toShouldExtensionWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var shouldMethodDefinitionWithProvider = context.SyntaxProvider.ForAttributeWithMetadataName(ShouldMethodDefinitionAttributeMetadataName, isTypeDeclarationSyntax, (v, _) => v)
            .Combine(csDeclarationProvider)
            .Select(toShouldMethodDefinitionWithProvider)
            .Where(v => v.HasValue)
            .Select((v, _) => v!.Value);

        var shouldObjectAndExtensionSource = shouldExtensionWithProvider
            .Select(toShouldObjectAndExtensionInput);

        var shouldMethodDefinitionSource = shouldMethodDefinitionWithProvider
            .Select(toShouldMethodDefinitionInput);

        var shouldRefStructAssertionContextTypeSource = shouldMethodDefinitionSource
            .Collect()
            .SelectMany(enumerateShouldRefStructAssertionContextTypeInput);

        var shouldObjectAssertionMethodSource = shouldExtensionWithProvider
            .SelectMany(enumerateShouldObjectAssertionMethodsInput)
            .Where(v => v.PartialDefinitionType is not null);

        // ShouldExtension属性を付与した型に対する以下の実装補完
        // 
        // readonly ref partial struct ShoudXxx
        // {
        //    private Xxx     Actual           { get; }
        //    private string? ActualExpression { get; }
        //    public ShoudXxx(Xxx actual, string? actualExpression) { ... }
        // }
        context.RegisterSourceOutput(shouldObjectAndExtensionSource, EmitShouldObject);

        // ShouldExtension属性を付与した型を返すxxx.Should()拡張メソッドの実装
        context.RegisterSourceOutput(shouldObjectAndExtensionSource, EmitShouldExtension);

        // ShouldExtension属性を付与した型に対するxxx.Should().BeXxxx()メソッドの実装
        context.RegisterSourceOutput(shouldObjectAssertionMethodSource, EmitShouldObjectAssertionMethods);

        // ShouldMethodDefinition属性を付与した型に対する以下の実装補完
        // 
        // ref partial struct XxxShouldBeYyy
        // {
        //   private global::ShouldMethodAssertion.ShouldAssertionContexts.ShouldAssertionContext<Xxx> Context { get; init; }
        // 
        //   public XxxShouldBeYyy(global::ShouldMethodAssertion.ShouldAssertionContexts.ShouldAssertionContext<Xxx> context)
        //   {
        //     Context = context;
        //   }
        // }
        context.RegisterSourceOutput(shouldMethodDefinitionSource, EmitShouldMethodDefinition);

        // ref struct用ShouldAssertionContextの出力
        context.RegisterSourceOutput(shouldRefStructAssertionContextTypeSource, EmitShouldRefStructAssertionContextType);



        static bool isTypeDeclarationSyntax(SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            if (syntaxNode is not TypeDeclarationSyntax typeDeclarationSyntax)
                return false;

            return true;
        }

        static ShouldExtensionWithProvider? toShouldExtensionWithProvider((GeneratorAttributeSyntaxContext context, CsDeclarationProvider declarationProvider) args, CancellationToken cancellationToken)
        {
            var (context, declarationProvider) = args;

            var typeSymbol = context.TargetSymbol as ITypeSymbol;

            if (typeSymbol is null)
                return default;

            var actualValueTypeSymbol = context.Attributes[0].ConstructorArguments[0].Value as INamedTypeSymbol;

            DebugSGen.AssertIsNotNull(actualValueTypeSymbol);

            var (actualValueType, rawActualValueType, actualValueTypeGenericTypeParams) = GetActualValueTypeAsNullable(args.declarationProvider, actualValueTypeSymbol);

            var rawPartialDefinitionType = declarationProvider.GetTypeReference(typeSymbol).Type;

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

        static ShouldMethodDefinitionWithProvider? toShouldMethodDefinitionWithProvider((GeneratorAttributeSyntaxContext context, CsDeclarationProvider declarationProvider) args, CancellationToken cancellationToken)
        {
            var (context, declarationProvider) = args;

            var typeSymbol = context.TargetSymbol as ITypeSymbol;

            if (typeSymbol is null)
                return default;

            var actualValueType = GetActualValueTypeFromShouldMethodDefinitionAttribute(declarationProvider, context.Attributes[0]);

            var rawPartialDefinitionType = declarationProvider.GetTypeReference(typeSymbol).Type;

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

            return new(context, declarationProvider, partialDefinitionType, actualValueType);
        }

        static ShouldObjectAndExtensionInput toShouldObjectAndExtensionInput(ShouldExtensionWithProvider args, CancellationToken cancellationToken)
        {
            var stringType = args.DeclarationProvider.SpecialType.String;
            var callerArgumentExpressionAttributeType = args.DeclarationProvider.GetTypeReferenceByMetadataName(CallerArgumentExpressionAttributeMetadataName);

            DebugSGen.AssertIsNotNull(callerArgumentExpressionAttributeType);

            return new (args.PartialDefinitionType, args.ActualValueType, args.RawActualValueType, args.ActualValueTypeGenericTypeParams, stringType, callerArgumentExpressionAttributeType);
        }

        static ShouldMethodDefinitionInput toShouldMethodDefinitionInput(ShouldMethodDefinitionWithProvider args, CancellationToken cancellationToken)
        {
            var (shouldAssertionContextType, isGenerated) = GetShouldAssertionContextType(args.DeclarationProvider, args.ActualValueType);

            return new(args.PartialDefinitionType, args.ActualValueType, shouldAssertionContextType, isGenerated);
        }

        static IEnumerable<ShouldObjectAssertionMethodsInput> enumerateShouldObjectAssertionMethodsInput(ShouldExtensionWithProvider args, CancellationToken cancellationToken)
        {
            var (context, declarationProvider, extensionType, actualValueType, rawActualValueType, actualValueTypeGenericTypeParams) = args;

            var typeSymbol = context.TargetSymbol as ITypeSymbol;

            if (typeSymbol is null)
                yield break;

            var shouldMethodAttributeSymbol = declarationProvider.Compilation.GetTypeByMetadataName(ShouldMethodAttributeMetadataName);

            var shouldMethodDefinitionAttributeSymbol = declarationProvider.Compilation.GetTypeByMetadataName(ShouldMethodDefinitionAttributeMetadataName);

            var shouldMethodAttributes = typeSymbol.GetAttributes().Where(v => SymbolEqualityComparer.Default.Equals(v.AttributeClass, shouldMethodAttributeSymbol));

            var stringType = declarationProvider.SpecialType.String;
            var callerArgumentExpressionAttribute = declarationProvider.GetTypeReferenceByMetadataName(CallerArgumentExpressionAttributeMetadataName);

            DebugSGen.AssertIsNotNull(callerArgumentExpressionAttribute);

            foreach (var shouldMethodAttributeData in shouldMethodAttributes)
            {
                var shouldMethodDefinitionTypeSymbol = shouldMethodAttributeData.ConstructorArguments[0].Value as INamedTypeSymbol;

                var convertMethodName = shouldMethodAttributeData.NamedArguments.FirstOrDefault(v => v.Key == "ConvertBy").Value.Value as string;

                var typeArgsTypedConstant = shouldMethodAttributeData.NamedArguments.FirstOrDefault(v => v.Key == "TypeArgs").Value;

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
                        null,
                        EquatableArray<CsMethod>.Empty,
                        $"ソース生成に失敗しました。");

                if (shouldMethodDefinitionAttribute is null)
                {
                    yield return failedDefaultValue with
                    {
                        WarningMessage = $"{shouldMethodDefinitionTypeSymbol.Name}に{ShouldMethodDefinitionAttributeMetadataName}属性が付与されていません。",
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
                            WarningMessage = $"TypeArgsの数が適用対象の型の型パラメータの数と一致しません。",
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
                            WarningMessage = $"型パラメータの数が一致しないため、型パラメータを継承出来ません。ShouldMethod属性にTypeArgsの指定が必要です。",
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

                var (shouldAssertionContextType, isGenerated) = GetShouldAssertionContextType(args.DeclarationProvider, shouldMethodDefinitionActualValueType);

                var shouldMethods = shouldMethodDefinitionTypeSymbol.GetMembers()
                    .OfType<IMethodSymbol>()
                    .Where(v => v is { IsStatic: false, MethodKind: MethodKind.Ordinary, DeclaredAccessibility: Accessibility.Public })
                    .Where(v => v.Name.StartsWith("Should", StringComparison.Ordinal))
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
                    shouldAssertionContextType,
                    shouldMethods,
                    null);
            }
        }

        static IEnumerable<ShouldRefStructAssertionContextTypeInput> enumerateShouldRefStructAssertionContextTypeInput(ImmutableArray<ShouldMethodDefinitionInput> v, CancellationToken _)
        {
            foreach (var item in v.Where(v => v.IsGeneratedShouldAssertionContextType).DistinctBy(v => v.ShouldAssertionContextType))
            {
                yield return new(item.ShouldAssertionContextType, item.ActualValueType);
            }
        }
    }

    private static CsTypeRefWithNullability GetActualValueTypeFromShouldMethodDefinitionAttribute(CsDeclarationProvider declarationProvider, AttributeData attributeData)
    {
        DebugSGen.AssertIsNotNull(attributeData.AttributeClass);
        DebugSGen.Assert(attributeData.AttributeClass.Name == "ShouldMethodDefinitionAttribute");

        var actualValueTypeSymbol = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;

        DebugSGen.AssertIsNotNull(actualValueTypeSymbol);

        var actualValueType = declarationProvider.GetTypeReference(actualValueTypeSymbol);

        var acceptNullReference = (bool)(attributeData.NamedArguments.FirstOrDefault(v => v.Key == "AcceptNullReference").Value.Value ?? false);

        return acceptNullReference ? actualValueType.ToNullableIfReferenceType() : actualValueType;
    }

    private static (CsTypeReference Type, bool IsGenerated) GetShouldAssertionContextType(CsDeclarationProvider declarationProvider, CsTypeRefWithNullability actualValueType)
    {
        if (actualValueType is { Type: { TypeDefinition: CsStruct { IsRef: true } } })
        {
            var shouldAssertionContextType = new CsTypeReference(
                new CsStruct(
                    new CsNameSpace("ShouldMethodAssertion.ShouldAssertionContexts"),
                    $"ShouldAssertionContext{actualValueType.Type.TypeDefinition.Name}",
                    actualValueType.Type.TypeDefinition.GenericTypeParams,
                    accessibility: CsAccessibility.Public,
                    isReadOnly: true,
                    isRef: true
                ),
                actualValueType.Type.TypeArgs
                );

            return (shouldAssertionContextType, true);
        }
        else
        {
            var shouldAssertionContextType = declarationProvider.GetTypeReferenceByMetadataName(ShouldAssertionContextMetadataName);

            DebugSGen.AssertIsNotNull(shouldAssertionContextType);

            shouldAssertionContextType = shouldAssertionContextType
                .WithTypeArgs(EquatableArray.Create(
                    EquatableArray.Create(actualValueType)
                ));

            return (shouldAssertionContextType, false);
        }
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

    private static void EmitShouldObject(SourceProductionContext context, ShouldObjectAndExtensionInput args)
    {
        var hintName = $"ShouldObjects/{args.PartialDefinitionType.TypeDefinition.MakeStandardHintName()}.cs";

        using var sb = new SourceBuilder(context, $"{hintName}.cs");

        using (sb.BeginTypeDefinitionBlock(args.PartialDefinitionType.TypeDefinition))
        {
            sb.AppendLineWithFirstIndent($"private {args.ActualValueType.GlobalReference} Actual {{ get; }}");
            sb.AppendLineWithFirstIndent($"private string? ActualExpression {{ get; }}");
            sb.AppendLine();

            using (sb.BeginBlock($"public {args.PartialDefinitionType.TypeDefinition.Name}({args.ActualValueType.GlobalReference} actual, string? actualExpression)"))
            {
                sb.AppendLineWithFirstIndent($"Actual = actual;");
                sb.AppendLineWithFirstIndent($"ActualExpression = actualExpression;");
            }
        }

        sb.Commit();
    }

    private static void EmitShouldExtension(SourceProductionContext context, ShouldObjectAndExtensionInput args)
    {
        const string actualParamName = "actual";
        const string actualExpressionParamName = "actualExpression";

        string hintName;

        if (args.ActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
            hintName = $"ShouldExtensions/{args.ActualValueType.Type.TypeArgs[0][0].Type.Cref}.cs";
        else
            hintName = $"ShouldExtensions/{args.ActualValueType.Type.Cref}.cs";

        using var sb = new SourceBuilder(context, $"{hintName}.cs");

        using (sb.BeginBlock($"namespace {nameof(ShouldMethodAssertion)}.ShouldExtensions"))
        {
            using (sb.BeginBlock($"public static partial class ShouldExtension"))
            {
                var callerArgumentExpressionAttribute = new CsAttribute(args.CallerArgumentExpressionAttributeType, [actualParamName]);

                writeMethod(sb, args.PartialDefinitionType, args.ActualValueType, args.ActualValueTypeGenericTypeParams, args.StringType, callerArgumentExpressionAttribute);

                if (args.RawActualValueType is not null)
                    writeMethod(sb, args.PartialDefinitionType, args.RawActualValueType.Value, args.ActualValueTypeGenericTypeParams, args.StringType, callerArgumentExpressionAttribute);
            }
        }

        sb.Commit();


        static void writeMethod(SourceBuilder sb, CsTypeReference partialDefinitionType, CsTypeRefWithNullability actualValueType, EquatableArray<CsGenericTypeParam> actualValueTypeGenericTypeParams, CsTypeReference stringType, CsAttribute callerArgumentExpressionAttribute)
        {
            var method = new CsExtensionMethod(
                "Should",
                partialDefinitionType.WithNullability(false),
                Params: EquatableArray.Create(
                    new CsMethodParam(actualValueType, actualParamName),
                    new CsMethodParamWithDefaultValue(stringType.WithNullability(true), actualExpressionParamName, DefaultValue: null, Attributes: EquatableArray.Create(callerArgumentExpressionAttribute))
                ),
                GenericTypeParams: actualValueTypeGenericTypeParams,
                Accessibility: CsAccessibility.Public
                );

            using (sb.BeginMethodDefinitionBlock(method, isPartial: false))
            {
                sb.AppendLineWithFirstIndent($"return new {partialDefinitionType.GlobalReference}({actualParamName}, {actualExpressionParamName});");
            }
        }
    }

    private static void EmitShouldRefStructAssertionContextType(SourceProductionContext context, ShouldRefStructAssertionContextTypeInput args)
    {
        using var sb = new SourceBuilder(context, $"AssertionContextTypes/{args.ActualValueType.Type.TypeDefinition.MakeStandardHintName()}");

        using (sb.BeginTypeDefinitionBlock(args.ShouldAssertionContextType.TypeDefinition))
        {
            sb.AppendLineWithFirstIndent($"public {args.ActualValueType.GlobalReference} Actual {{ get; }}");
            sb.AppendLineWithFirstIndent($"public string ActualExpression {{ get; }}");
            sb.AppendLineWithFirstIndent($"");
            sb.AppendLineWithFirstIndent($"private readonly (string name, string? expression) _param1;");
            sb.AppendLineWithFirstIndent($"private readonly (string name, string? expression) _param2;");
            sb.AppendLineWithFirstIndent($"private readonly (string name, string? expression) _param3;");
            sb.AppendLineWithFirstIndent($"");
            sb.AppendLineWithFirstIndent($"private readonly Dictionary<string, string?>? _extraParamsExpressions;");
            sb.AppendLineWithFirstIndent($"");
            using (sb.BeginBlock($"public {args.ShouldAssertionContextType.TypeDefinition.Name}({args.ActualValueType.GlobalReference} actual, string actualExpression, (string name, string? expression) param1, (string name, string? expression) param2, (string name, string? expression) param3, Dictionary<string, string?>? extraParamsExpressions)"))
            {
                sb.AppendLineWithFirstIndent($"Actual = actual;");
                sb.AppendLineWithFirstIndent($"ActualExpression = actualExpression;");
                sb.AppendLineWithFirstIndent($"_param1 = param1;");
                sb.AppendLineWithFirstIndent($"_param2 = param2;");
                sb.AppendLineWithFirstIndent($"_param3 = param3;");
                sb.AppendLineWithFirstIndent($"_extraParamsExpressions = extraParamsExpressions;");
            }
            sb.AppendLineWithFirstIndent($"");
            using (sb.BeginBlock($"public string? GetExpressionOf(string paramName)"))
            {
                sb.AppendLineWithFirstIndent($"if (_param1.name == paramName)");
                sb.AppendLineWithFirstIndent($"    return _param1.expression;");
                sb.AppendLineWithFirstIndent($"");
                sb.AppendLineWithFirstIndent($"if (_param2.name == paramName)");
                sb.AppendLineWithFirstIndent($"    return _param2.expression;");
                sb.AppendLineWithFirstIndent($"");
                sb.AppendLineWithFirstIndent($"if (_param3.name == paramName)");
                sb.AppendLineWithFirstIndent($"    return _param3.expression;");
                sb.AppendLineWithFirstIndent($"");
                sb.AppendLineWithFirstIndent($"if (_extraParamsExpressions is not null && _extraParamsExpressions.TryGetValue(paramName, out var expression))");
                sb.AppendLineWithFirstIndent($"    return expression;");
                sb.AppendLineWithFirstIndent($"");
                sb.AppendLineWithFirstIndent($"throw new ArgumentException(null, nameof(paramName));");
            }
        }

        sb.Commit();
    }

    private static void EmitShouldMethodDefinition(SourceProductionContext context, ShouldMethodDefinitionInput args)
    {
        using var sb = new SourceBuilder(context, $"ShouldMethodDefinitions/{args.PartialDefinitionType.TypeDefinition.MakeStandardHintName()}");

        using (sb.BeginTypeDefinitionBlock(args.PartialDefinitionType.TypeDefinition))
        {
            sb.AppendLineWithFirstIndent($"private {args.ShouldAssertionContextType.GlobalReference} Context {{ get; init; }}");
            sb.AppendLine();
            sb.AppendLineWithFirstIndent($"public {args.PartialDefinitionType.TypeDefinition.Name}({args.ShouldAssertionContextType.GlobalReference} context)");
            using (sb.BeginBlock())
            {
                sb.AppendLineWithFirstIndent($"Context = context;");
            }

        }

        sb.Commit();
    }


    private static void EmitShouldObjectAssertionMethods(SourceProductionContext context, ShouldObjectAssertionMethodsInput args)
    {
        const string ExpressionParamSuffix = "CallerArgumentExpression";

        string hintName;

        if (args.ShouldObjectActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
            hintName = $"ShouldObjects/{args.ShouldObjectActualValueType.Type.TypeArgs[0][0].Cref}/{args.ShouldMethodDefinitionType.TypeDefinition.Name}.cs";
        else
            hintName = $"ShouldObjects/{args.ShouldObjectActualValueType.Type.Cref}/{args.ShouldMethodDefinitionType.TypeDefinition.Name}.cs";

        using var sb = new SourceBuilder(context, $"{hintName}.cs");

        if (!string.IsNullOrWhiteSpace(args.WarningMessage))
            sb.AppendLine($"#warning {args.WarningMessage}");

        using (sb.BeginTypeDefinitionBlock(args.PartialDefinitionType.TypeDefinition))
        {
            if (args.ShouldAssertionContextType is null)
            {
                sb.AppendLine($"#warning ソース生成時の{nameof(args.ShouldAssertionContextType)}がnullです。");
            }
            else if (args.ShouldMethodDefinitionActualValueType is null)
            {
                sb.AppendLine($"#warning ソース生成時の{nameof(args.ShouldMethodDefinitionActualValueType)}がnullです。");
            }
            else
            {
                foreach (var shouldMethod in args.ShouldMethods.Values)
                {
                    var paramsBuilder = ImmutableArray.CreateBuilder<CsMethodParam>(shouldMethod.Params.Length * 2);
                    paramsBuilder.AddRange(shouldMethod.Params);
                    foreach (var param in shouldMethod.Params.Values)
                    {
                        // [System.Runtime.CompilerServices.CallerArgumentExpressionAttribute("xxx")]
                        var callerArgumentExpressionParam = new CsMethodParamWithDefaultValue(
                            args.StringType.WithNullability(true),
                            $"{param.Name}{ExpressionParamSuffix}",
                            DefaultValue: null,
                            Attributes: ImmutableArray.Create(new CsAttribute(args.CallerArgumentExpressionAttributeType, [param.Name]))
                            );

                        paramsBuilder.Add(callerArgumentExpressionParam);
                    }

                    const string ShouldMethodPrefix = "Should";

                    var methodNameInShouldExtensionObjectType = shouldMethod.Name;
                    if (methodNameInShouldExtensionObjectType.StartsWith(ShouldMethodPrefix, StringComparison.OrdinalIgnoreCase) && methodNameInShouldExtensionObjectType.Length > ShouldMethodPrefix.Length)
                        methodNameInShouldExtensionObjectType = methodNameInShouldExtensionObjectType.Substring(ShouldMethodPrefix.Length);

                    var extendedShouldMethod = shouldMethod with
                    {
                        Name = methodNameInShouldExtensionObjectType,
                        Params = paramsBuilder.MoveToImmutable(),
                    };

                    // void ShouldBe(..., [CallerArgumentExpression(..)] ..) のような検証メソッド
                    sb.AppendLine($"#pragma warning disable CS0693"); // 型定義の型パラメータとメソッド定義の型パラメータの名前の重複に対する警告を抑止
                    using (sb.BeginMethodDefinitionBlock(extendedShouldMethod, isPartial: false))
                    {
                        sb.AppendLine($"#pragma warning restore CS0693");

                        string actualValueRefSymbolName = "Actual";

                        if (args.ShouldMethodDefinitionActualValueType is not { IsNullable: true, Type.TypeDefinition.IsReferenceType: true })
                        {
                            // ShouldAssertionContextのActualがnull許容の参照型でない場合は、必要に応じて事前にnullチェックを実施してから、
                            // 本来の検証内容の呼出しを行う

                            if (args.ShouldObjectActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT) && !args.ShouldMethodDefinitionActualValueType.Value.Type.TypeDefinition.Is(CsSpecialType.NullableT))
                            {
                                // ShouldAssertionContextのActualがnull非許容の値型で、検証対象の実際値の型がNullable<T>

                                using (sb.BeginBlock($"if (!Actual.HasValue)"))
                                {
                                    sb.AppendLineWithFirstIndent($"throw {ExceptionCreateCall}($\"`{{ActualExpression ?? \"Actual\"}}` is null.\");");
                                }
                                sb.AppendLineWithFirstIndent($"var rawActualValue = Actual.Value;");
                                sb.AppendLine();

                                actualValueRefSymbolName = "rawActualValue";
                            }
                            else if (args.ShouldObjectActualValueType is { IsNullable: true, Type.TypeDefinition.IsReferenceType: true } && args.ShouldMethodDefinitionActualValueType is { IsNullable: false, Type.TypeDefinition.IsReferenceType: true })
                            {
                                // ShouldAssertionContextのActualがnull非許容の参照型で、検証対象の実際値の型がnull許容の参照型

                                using (sb.BeginBlock($"if (Actual is null)"))
                                {
                                    sb.AppendLineWithFirstIndent($"throw {ExceptionCreateCall}($\"`{{ActualExpression ?? \"Actual\"}}` is null.\");");
                                }
                                sb.AppendLine();
                            }
                        }

                        if (args.ActualValueConvertMethodName is not null)
                        {
                            sb.AppendLineWithFirstIndent($"var __convertedActualValue = {args.ActualValueConvertMethodName}({actualValueRefSymbolName});");
                            actualValueRefSymbolName = "__convertedActualValue";
                        }

                        sb.AppendLineWithFirstIndent($"var __context = new {args.ShouldAssertionContextType.GlobalReference}(");
                        sb.AppendLineWithFirstIndent($"    {actualValueRefSymbolName},");
                        sb.AppendLineWithFirstIndent($"    ActualExpression ?? {SymbolDisplay.FormatLiteral("Actual", quote: true)},");
                        for (int i = 0; i < 3; i++)
                        {
                            if (!shouldMethod.Params.IsDefaultOrEmpty && i < shouldMethod.Params.Length)
                            {
                                var paramNameLiteral = SymbolDisplay.FormatLiteral(shouldMethod.Params[i].Name, quote: true);
                                sb.AppendLineWithFirstIndent($"    ({paramNameLiteral}, {shouldMethod.Params[i].Name}{ExpressionParamSuffix}),");
                            }
                            else
                            {
                                sb.AppendLineWithFirstIndent($"    default,");
                            }
                        }
                        if (!shouldMethod.Params.IsDefaultOrEmpty && 3 < shouldMethod.Params.Length)
                        {
                            sb.AppendLineWithFirstIndent($"    new global::System.Collection.Generic.Dictionary<string, string>");
                            sb.AppendLineWithFirstIndent($"    {{");
                            for (int i = 3; i < shouldMethod.Params.Length; i++)
                            {
                                var paramNameLiteral = SymbolDisplay.FormatLiteral(shouldMethod.Params[i].Name, quote: true);
                                sb.AppendLineWithFirstIndent($"        [{paramNameLiteral}] = {shouldMethod.Params[i].Name}{ExpressionParamSuffix} ?? {paramNameLiteral},");
                            }
                            sb.AppendLineWithFirstIndent($"    }}");
                        }
                        else
                        {
                            sb.AppendLineWithFirstIndent($"    null);");
                        }
                        sb.AppendLine();
                        sb.AppendLineWithFirstIndent($"var __assertMethod = new {args.ShouldMethodDefinitionType.GlobalReference}(__context);");
                        sb.AppendLine();
                        sb.PutIndentSpace();
                        if (!shouldMethod.IsVoidLikeMethod)
                            sb.Append($"return ");
                        if (shouldMethod.IsAsync)
                            sb.Append($"await ");
                        sb.Append($"__assertMethod.{shouldMethod.Name}");
                        if (!shouldMethod.GenericTypeParams.IsDefaultOrEmpty)
                        {
                            sb.Append($"<");
                            bool isFirstParam = true;
                            foreach (var typeParam in shouldMethod.GenericTypeParams.Values)
                            {
                                if (!isFirstParam)
                                    sb.Append(", ");

                                isFirstParam = false;

                                sb.Append(typeParam.Name);
                            }
                            sb.Append($">");
                        }
                        sb.Append($"(");
                        {
                            bool isFirstParam = true;
                            foreach (var param in shouldMethod.Params.Values)
                            {
                                if (!isFirstParam)
                                    sb.Append(", ");

                                isFirstParam = false;

                                if (param.Modifier == CsParamModifier.Ref)
                                    sb.Append("ref ");
                                else if (param.Modifier == CsParamModifier.Out)
                                    sb.Append("out ");

                                sb.Append(param.Name);
                            }
                        }
                        sb.Append($")");
                        if (shouldMethod.IsAsync)
                            sb.Append($".ConfigureAwait(false)");
                        sb.AppendLine($";");
                    }
                }
            }
        }

        sb.Commit();
    }
}

