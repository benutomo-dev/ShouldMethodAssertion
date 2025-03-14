using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Enum))]
public partial struct EnumShouldHaveFlag
{
    public void ShouldHaveFlag<T>(T expected) where T : struct, Enum
    {
        if (Context.Actual.HasFlag(expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` not has `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldNotHaveFlag<T>(T expected) where T : struct, Enum
    {
        if (!Context.Actual.HasFlag(expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` has `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}
