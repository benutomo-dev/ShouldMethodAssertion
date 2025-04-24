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
        var hintName = $"{NameSpaceNames.ShouldExtensions}/ShouldExtension.cs";

        using var sb = new SourceBuilder(context, hintName);

        using (args.ShouldMethodDefinitionNameSpace is null ? default : sb.BeginBlock($"namespace {args.ShouldMethodDefinitionNameSpace}"))
        {
            using (sb.BeginBlock($"internal static partial class {args.ShouldMethodDefinitionClassName}"))
            {
                var callerArgumentExpressionAttribute = new CsAttribute(args.CallerArgumentExpressionAttributeType, [ActualParamName]);

                for (int i = 0; i < args.ShouldExtensionMethodInfos.Length; i++)
                {
                    EmitMethod(sb, args.ShouldExtensionMethodInfos[i], i, args.StringType, args.NotNullAttributeType, callerArgumentExpressionAttribute);
                    sb.AppendLine();
                }
            }
        }
        sb.Commit();
    }

    private static void EmitMethod(SourceBuilder sb, ShouldExtensionMethodInfo shouldExtensionMethodInfo, int methodIndex, CsTypeRef stringType, CsTypeRef? notNullAttributeType, CsAttribute callerArgumentExpressionAttribute)
    {
        var (shouldObjectType, overloadResolutionPriority) = shouldExtensionMethodInfo;

        var actualValueParamAttributes = EquatableArray<CsAttribute>.Empty;

        // Should拡張メソッドが呼び出された時点でnullの可能性はなくなったものとして扱わせる。
        // (明示的にnullであることが確認されたか、null検証に失敗するかのどちらかとなる)
        if (notNullAttributeType is not null && (shouldObjectType.ActualValueType.IsNullable || shouldObjectType.ActualValueType.Type.TypeDefinition.Is(CsSpecialType.NullableT)))
            actualValueParamAttributes = EquatableArray.Create(new CsAttribute(notNullAttributeType));

        var dummyArgTypeName = $"AvoidOverloadConflictionDummyArgT{methodIndex}";

        var dummyArgType = new CsTypeRefWithAnnotation(CsTypeRef.CreateFrom(new CsClass(null, dummyArgTypeName, accessibility: CsAccessibility.Public)), isNullableIfRefereceType: true);

        var method = new CsExtensionMethod(
            "Should",
            shouldObjectType.PartialDefinitionType.WithAnnotation(isNullableIfRefereceType: false),
            Params: EquatableArray.Create(
                new CsMethodParam(shouldObjectType.ActualValueType, ActualParamName, Attributes: actualValueParamAttributes),
                new CsMethodParamWithDefaultValue(stringType.WithAnnotation(isNullableIfRefereceType: true), ActualExpressionParamName, DefaultValue: null, Attributes: EquatableArray.Create(callerArgumentExpressionAttribute)),
                new CsMethodParamWithDefaultValue(dummyArgType, "dummyArg", null)
            ),
            GenericTypeParams: shouldObjectType.PartialDefinitionType.TypeDefinition.GenericTypeParams,
            Accessibility: CsAccessibility.Public
            );

        sb.AppendLineWithFirstIndent($"public sealed class {dummyArgTypeName} {{ private {dummyArgTypeName}() {{}} }}");
        if (overloadResolutionPriority != 0)
        {
            sb.AppendLineWithFirstIndent($"[{MetadataNames.OverloadResolutionPriorityAttribute}({overloadResolutionPriority})]");
        }
        using (sb.BeginMethodDefinitionBlock(method, isPartial: false))
        {
            sb.AppendLine($"#pragma warning disable CS8777"); // null許容性のチェックに対して発生する警告をもみ消す
            sb.AppendLineWithFirstIndent($"return new {shouldObjectType.PartialDefinitionType.GlobalReference}({ActualParamName}, {ActualExpressionParamName});");
            sb.AppendLine($"#pragma warning restore CS8777");
        }
    }
}