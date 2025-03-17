using Microsoft.CodeAnalysis;
using SourceGeneratorCommons;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class ShouldRefStructAssertionContextTypeEmitter
{
    public static void Emit(SourceProductionContext context, ShouldRefStructAssertionContextTypeInput args)
    {
        using var sb = new SourceBuilder(context, $"{NameSpaceNames.AssertionContextTypes}/{args.ActualValueType.Type.TypeDefinition.MakeStandardHintName()}.cs");

        using (sb.BeginTypeDefinitionBlock(args.ShouldAssertionContextType.TypeDefinition))
        {
            sb.AppendLineWithFirstIndent($"public {args.ActualValueType.GlobalReference} Actual {{ get; }}");
            sb.AppendLineWithFirstIndent($"public string ActualExpression {{ get; }}");
            sb.AppendLineWithFirstIndent($"");
            sb.AppendLineWithFirstIndent($"private readonly (string name, string? expression) _param1;");
            sb.AppendLineWithFirstIndent($"private readonly (string name, string? expression) _param2;");
            sb.AppendLineWithFirstIndent($"private readonly (string name, string? expression) _param3;");
            sb.AppendLineWithFirstIndent($"");
            sb.AppendLineWithFirstIndent($"private readonly global::System.Collections.Generic.Dictionary<string, string?>? _extraParamsExpressions;");
            sb.AppendLineWithFirstIndent($"");
            using (sb.BeginBlock($"public {args.ShouldAssertionContextType.TypeDefinition.Name}({args.ActualValueType.GlobalReference} actual, string actualExpression, (string name, string? expression) param1, (string name, string? expression) param2, (string name, string? expression) param3, global::System.Collections.Generic.Dictionary<string, string?>? extraParamsExpressions)"))
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
                sb.AppendLineWithFirstIndent($"throw new global::System.ArgumentException(null, nameof(paramName));");
            }
        }

        sb.Commit();
    }
}