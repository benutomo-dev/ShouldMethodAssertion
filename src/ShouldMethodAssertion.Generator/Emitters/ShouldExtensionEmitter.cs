using Microsoft.CodeAnalysis;
using SourceGeneratorCommons;
using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class ShouldExtensionEmitter
{
    private const string ActualParamName = "actual";

    private const string ActualExpressionParamName = "actualExpression";

    /// <summary>
    /// ShouldExtension属性を付与した型を返すxxx.Should()拡張メソッドの実装
    /// </summary>
    public static void Emit(SourceProductionContext context, ShouldExtensionInput args)
    {
        string hintName;

        if (args.ActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT))
            hintName = $"{NameSpaceNames.ShouldExtensions}/{args.PartialDefinitionType.TypeDefinition.Name}/{args.ActualValueType.Type.TypeArgs[0][0].Type.Cref}.cs";
        else
            hintName = $"{NameSpaceNames.ShouldExtensions}/{args.PartialDefinitionType.TypeDefinition.Name}/{args.ActualValueType.Type.Cref}.cs";

        using var sb = new SourceBuilder(context, hintName);

        using (sb.BeginBlock($"namespace {NameSpaces.ShouldExtensions}"))
        {
            using (sb.BeginBlock($"public static partial class {args.PartialDefinitionType.TypeDefinition.Name}Extension"))
            {
                var callerArgumentExpressionAttribute = new CsAttribute(args.CallerArgumentExpressionAttributeType, [ActualParamName]);

                EmitMethod(sb, args.PartialDefinitionType, args.ActualValueType, args.StringType, args.NotNullAttributeType, callerArgumentExpressionAttribute);
            }
        }

        sb.Commit();
    }

    private static void EmitMethod(SourceBuilder sb, CsTypeRef partialDefinitionType, CsTypeRefWithAnnotation actualValueType, CsTypeRef stringType, CsTypeRef? notNullAttributeType, CsAttribute callerArgumentExpressionAttribute)
    {
        var actualValueParamAttributes = EquatableArray<CsAttribute>.Empty;

        // Should拡張メソッドが呼び出された時点でnullの可能性はなくなったものとして扱わせる。
        // (明示的にnullであることが確認されたか、null検証に失敗するかのどちらかとなる)
        if (notNullAttributeType is not null && (actualValueType.IsNullable || actualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT)))
            actualValueParamAttributes = EquatableArray.Create(new CsAttribute(notNullAttributeType));

        var method = new CsExtensionMethod(
            "Should",
            partialDefinitionType.WithNullability(false),
            Params: EquatableArray.Create(
                new CsMethodParam(actualValueType, ActualParamName, Attributes: actualValueParamAttributes),
                new CsMethodParamWithDefaultValue(stringType.WithNullability(true), ActualExpressionParamName, DefaultValue: null, Attributes: EquatableArray.Create(callerArgumentExpressionAttribute))
            ),
            GenericTypeParams: partialDefinitionType.TypeDefinition.GenericTypeParams,
            Accessibility: CsAccessibility.Public
            );

        using (sb.BeginMethodDefinitionBlock(method, isPartial: false))
        {
            sb.AppendLine($"#pragma warning disable CS8777"); // null許容性のチェックに対して発生する警告をもみ消す
            sb.AppendLineWithFirstIndent($"return new {partialDefinitionType.GlobalReference}({ActualParamName}, {ActualExpressionParamName});");
            sb.AppendLine($"#pragma warning restore CS8777");
        }
    }
}