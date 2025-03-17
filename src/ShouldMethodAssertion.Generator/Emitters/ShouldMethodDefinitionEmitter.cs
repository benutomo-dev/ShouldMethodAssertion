using Microsoft.CodeAnalysis;
using SourceGeneratorCommons;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class ShouldMethodDefinitionEmitter
{
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