using Microsoft.CodeAnalysis;
using SourceGeneratorCommons;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class ShouldObjectEmitter
{
    /// <summary>
    /// ShouldExtension属性を付与した型に対する以下の実装補完
    /// <code>
    /// readonly ref partial struct ShouldXxx
    /// {
    ///     private Xxx Actual { get; }
    ///     private string? ActualExpression { get; }
    ///     public ShouldXxx(Xxx actual, string? actualExpression) { ... }
    /// }
    /// </code>
    /// </summary>
    public static void Emit(SourceProductionContext context, ShouldObjectInput args)
    {
        var hintName = $"{NameSpaceNames.ShouldObjects}/{args.ShouldObjectType.PartialDefinitionType.SimpleCref}.cs";

        using var sb = new SourceBuilder(context, hintName);

        EmitCore(sb, args.ShouldObjectType);

        // 構造体に対するNullable<T>版の出力
        if (args.NullableTShouldObjectType.HasValue)
            EmitCore(sb, args.NullableTShouldObjectType.Value);

        sb.Commit();
    }

    private static void EmitCore(SourceBuilder sb, PartialDefinitionTypeWithActualValueType shouldObjectType)
    {
        using (sb.BeginTypeDefinitionBlock(shouldObjectType.PartialDefinitionType.TypeDefinition))
        {
            sb.AppendLineWithFirstIndent($"private {shouldObjectType.ActualValueType.GlobalReference} Actual {{ get; }}");
            sb.AppendLineWithFirstIndent($"private string? ActualExpression {{ get; }}");
            sb.AppendLine();

            using (sb.BeginBlock($"public {shouldObjectType.PartialDefinitionType.TypeDefinition.Name}({shouldObjectType.ActualValueType.GlobalReference} actual, string? actualExpression)"))
            {
                sb.AppendLineWithFirstIndent($"Actual = actual;");
                sb.AppendLineWithFirstIndent($"ActualExpression = actualExpression;");
            }
        }

    }

}
