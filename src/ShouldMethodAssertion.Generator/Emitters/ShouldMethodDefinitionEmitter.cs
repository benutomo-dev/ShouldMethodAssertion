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
}