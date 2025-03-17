using Microsoft.CodeAnalysis;
using SourceGeneratorCommons;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class ShouldMethodDefinitionEmitter
{
    /// <summary>
    /// ShouldMethodDefinition属性を付与した型に対する以下の実装補完
    /// <code>
    /// ref partial struct XxxShouldBeYyy
    /// {
    ///   private global::ShouldMethodAssertion.ShouldAssertionContexts.ShouldAssertionContext&lt;Xxx&gt; Context { get; init; }
    /// 
    ///   public XxxShouldBeYyy(global::ShouldMethodAssertion.ShouldAssertionContexts.ShouldAssertionContext&lt;Xxx&gt; context)
    ///   {
    ///     Context = context;
    ///   }
    /// }
    /// </code>
    /// </summary>
    public static void Emit(SourceProductionContext context, ShouldMethodDefinitionInput args)
    {
        using var sb = new SourceBuilder(context, $"{NameSpaceNames.ShouldMethodDefinitions}/{args.PartialDefinitionType.TypeDefinition.MakeStandardHintName()}.cs");

        using (sb.BeginTypeDefinitionBlock(args.PartialDefinitionType.TypeDefinition))
        {
            sb.AppendLine("#pragma warning disable CA1707");
            using (sb.BeginBlock("public struct __ParameterExpressions"))
            {
                sb.AppendLine("#pragma warning restore CA1707");

                foreach (var parameter in args.MethodParameters.Values.Where(v => !v.MayBeNull))
                    sb.AppendLineWithFirstIndent($"public {GlobalReferences.ValueExpression} {parameter.Name} {{ get; init; }}");

                sb.AppendLine();

                foreach (var parameter in args.MethodParameters.Values.Where(v => v.MayBeNull))
                    sb.AppendLineWithFirstIndent($"public {GlobalReferences.NullableValueExpression} {parameter.Name} {{ get; init; }}");
            }
            sb.AppendLine();
            sb.AppendLineWithFirstIndent($"public {args.ActualValueType.GlobalReference} Actual {{ get; }}");
            sb.AppendLineWithFirstIndent($"public {GlobalReferences.ValueExpression} ActualExpression {{ get; }}");
            sb.AppendLine();
            sb.AppendLineWithFirstIndent($"private __ParameterExpressions ParamExpressions {{ get; init; }}");
            sb.AppendLine();
            sb.AppendLineWithFirstIndent($"public {args.PartialDefinitionType.TypeDefinition.Name}({args.ActualValueType.GlobalReference} actual, {GlobalReferences.ValueExpression} actualExpression, __ParameterExpressions parameterExpressions)");
            using (sb.BeginBlock())
            {
                sb.AppendLineWithFirstIndent($"Actual = actual;");
                sb.AppendLineWithFirstIndent($"ActualExpression = actualExpression;");
                sb.AppendLineWithFirstIndent($"ParamExpressions = parameterExpressions;");
            }
        }

        sb.Commit();
    }
}