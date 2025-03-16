using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object), AcceptNullReference = true)]
public partial struct ObjectShouldBe
{
    public void ShouldBe<T>(T? expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Context.Actual is null && expected is null)
            return;

        if (Context.Actual is T actual && comparer.Equals(actual, expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldNotBe<T>(T? expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Context.Actual is null && expected is null)
            throw AssertExceptionUtil.Create($"Both `{Context.ActualExpression}` and `{Context.GetExpressionOf(nameof(expected))}` are `null`.");

        if (Context.Actual is not T actual || !comparer.Equals(actual, expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}
