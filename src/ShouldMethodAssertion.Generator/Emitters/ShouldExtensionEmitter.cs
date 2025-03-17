using Microsoft.CodeAnalysis;
using SourceGeneratorCommons;
using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class ShouldExtensionEmitter
{
    public static void Emit(SourceProductionContext context, ShouldObjectAndExtensionInput args)
    {
        const string actualParamName = "actual";
        const string actualExpressionParamName = "actualExpression";

        string hintName;

        if (args.ActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
            hintName = $"{NameSpaceNames.ShouldExtensions}/{args.ActualValueType.Type.TypeArgs[0][0].Type.Cref}.cs";
        else
            hintName = $"{NameSpaceNames.ShouldExtensions}/{args.ActualValueType.Type.Cref}.cs";

        using var sb = new SourceBuilder(context, hintName);

        using (sb.BeginBlock($"namespace {NameSpaces.ShouldExtensions}"))
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
}