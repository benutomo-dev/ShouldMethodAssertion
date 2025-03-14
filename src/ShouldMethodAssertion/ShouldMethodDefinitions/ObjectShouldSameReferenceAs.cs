using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object))]
public partial struct ObjectShouldSameReferenceAs
{
    public void ShouldSameReferenceAs<T>(T expected) where T : class
    {
        if (ReferenceEquals(Context.Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not same as `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldNotSameReferenceAs<T>(T expected) where T : class
    {
        if (!ReferenceEquals(Context.Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is same as `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}
