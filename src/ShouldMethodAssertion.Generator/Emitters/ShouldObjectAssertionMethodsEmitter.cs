using Microsoft.CodeAnalysis;
using SourceGeneratorCommons;
using SourceGeneratorCommons.CSharp.Declarations;
using System.Collections.Immutable;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class ShouldObjectAssertionMethodsEmitter
{
    private const string ExpressionParamSuffix = "CallerArgumentExpression";

    /// <summary>
    /// ShouldExtension属性を付与した型に対するxxx.Should().BeXxxx()メソッドの実装
    /// </summary>
    public static void Emit(SourceProductionContext context, ShouldObjectAssertionMethodsInput args)
    {
        string hintName;

        if (args.ShouldObjectType.ActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
            hintName = $"{NameSpaceNames.ShouldObjects}/{args.ShouldObjectType.PartialDefinitionType.SimpleCref}/{args.ShouldMethodDefinitionType.PartialDefinitionType.SimpleCref}/{args.ShouldObjectType.ActualValueType.Type.TypeArgs[0][0].SimpleCref}.cs";
        else
            hintName = $"{NameSpaceNames.ShouldObjects}/{args.ShouldObjectType.PartialDefinitionType.SimpleCref}/{args.ShouldMethodDefinitionType.PartialDefinitionType.SimpleCref}/{args.ShouldObjectType.ActualValueType.Type.SimpleCref}.cs";

        using var sb = new SourceBuilder(context, hintName);

        if (!string.IsNullOrWhiteSpace(args.WarningMessage))
            sb.AppendLine($"#warning {args.WarningMessage}");

        var options = new TypeDefinitionBlockOptions
        {
            TypeDeclarationLineTail = $" // {args.ShouldMethodDefinitionType.PartialDefinitionType.InternalReference}",
        };
        using (sb.BeginTypeDefinitionBlock(args.ShouldObjectType.PartialDefinitionType.TypeDefinition, options))
        {
            foreach (var shouldMethod in args.ShouldMethods.Values)
            {
                EmitMethod(sb, args, shouldMethod);
            }
        }

        sb.Commit();
    }

    private static void EmitMethod(SourceBuilder sb, ShouldObjectAssertionMethodsInput args, CsMethod shouldMethod)
    {
        var paramsBuilder = ImmutableArray.CreateBuilder<CsMethodParam>(shouldMethod.Params.Length * 2);
        paramsBuilder.AddRange(shouldMethod.Params);
        foreach (var param in shouldMethod.Params.Values)
        {
            // [System.Runtime.CompilerServices.CallerArgumentExpressionAttribute("xxx")]
            var callerArgumentExpressionParam = new CsMethodParamWithDefaultValue(
                args.StringType.WithAnnotation(isNullableIfRefereceType: true),
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

            if (args.ShouldMethodDefinitionType.ActualValueType is not { IsNullable: true, Type.TypeDefinition.IsReferenceType: true })
            {
                // ShouldAssertionContextのActualがnull許容の参照型でない場合は、必要に応じて事前にnullチェックを実施してから、
                // 本来の検証内容の呼出しを行う

                if (args.ShouldObjectType.ActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT) && !args.ShouldMethodDefinitionType.ActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
                {
                    // ShouldAssertionContextのActualがnull非許容の値型で、検証対象の実際値の型がNullable<T>

                    using (sb.BeginBlock($"if (!Actual.HasValue)"))
                    {
                        sb.AppendLineWithFirstIndent($"throw {GlobalReferences.ExceptionCreateMethod}($\"`{{ActualExpression ?? \"Actual\"}}` is null.\");");
                    }
                    sb.AppendLineWithFirstIndent($"var rawActualValue = Actual.Value;");
                    sb.AppendLine();

                    actualValueRefSymbolName = "rawActualValue";
                }
                else if (args.ShouldObjectType.ActualValueType is { IsNullable: true, Type.TypeDefinition.IsReferenceType: true } && args.ShouldMethodDefinitionType.ActualValueType is { IsNullable: false, Type.TypeDefinition.IsReferenceType: true })
                {
                    // ShouldAssertionContextのActualがnull非許容の参照型で、検証対象の実際値の型がnull許容の参照型

                    using (sb.BeginBlock($"if (Actual is null)"))
                    {
                        sb.AppendLineWithFirstIndent($"throw {GlobalReferences.ExceptionCreateMethod}($\"`{{ActualExpression ?? \"Actual\"}}` is null.\");");
                    }
                    sb.AppendLine();
                }
            }

            if (args.ActualValueConvertMethodName is not null)
            {
                sb.AppendLineWithFirstIndent($"var __convertedActualValue = {args.ActualValueConvertMethodName}({actualValueRefSymbolName});");
                sb.AppendLine();
                actualValueRefSymbolName = "__convertedActualValue";
            }

            using (sb.BeginBlock($"var __parameterExpressions = new {args.ShouldMethodDefinitionType.PartialDefinitionType.GlobalReference}.{InternalTypeNames.ParameterExpressions}"))
            {
                if (!shouldMethod.Params.IsDefaultOrEmpty)
                {
                    foreach (var param in shouldMethod.Params.Values)
                        sb.AppendLineWithFirstIndent($"{param.Name} = {param.Name}{ExpressionParamSuffix}!,");
                }
            }
            sb.AppendLineWithFirstIndent($";");
            sb.AppendLine();
            sb.AppendLineWithFirstIndent($"var __assertMethod = new {args.ShouldMethodDefinitionType.PartialDefinitionType.GlobalReference}({actualValueRefSymbolName}, ActualExpression ?? \"Actual\", __parameterExpressions);");
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