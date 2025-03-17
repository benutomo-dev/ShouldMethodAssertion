using Microsoft.CodeAnalysis;
using SourceGeneratorCommons;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class ShouldObjectEmitter
{
    /// <summary>
    /// ShouldExtension属性を付与した型に対する以下の実装補完
    /// <code>
    /// readonly ref partial struct ShoudXxx
    /// {
    ///     private Xxx Actual { get; }
    ///     private string? ActualExpression { get; }
    ///     public ShoudXxx(Xxx actual, string? actualExpression) { ... }
    /// }
    /// </code>
    /// </summary>
    public static void Emit(SourceProductionContext context, ShouldObjectAndExtensionInput args)
    {
        var hintName = $"{NameSpaceNames.ShouldObjects}/{args.PartialDefinitionType.TypeDefinition.MakeStandardHintName()}.cs";

        using var sb = new SourceBuilder(context, hintName);

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
}
