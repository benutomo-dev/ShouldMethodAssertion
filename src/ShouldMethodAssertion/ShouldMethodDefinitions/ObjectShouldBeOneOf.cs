using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object))]
public partial struct ObjectShouldBeOneOf
{
    public void ShouldBeOneOf<T>(ReadOnlySpan<T> expectedList, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        foreach (var expected in expectedList)
        {
            if (Context.Actual is T actual && comparer.Equals(actual, expected))
                return;
        }

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not one of `{Context.GetExpressionOf(nameof(expectedList))}`.");
    }

    public void ShouldNotBeOneOf<T>(ReadOnlySpan<T> expectedList, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        foreach (var expected in expectedList)
        {
            if (Context.Actual is T actual && comparer.Equals(actual, expected))
                throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is one of `{Context.GetExpressionOf(nameof(expectedList))}`.");
        }

        return;
    }
}
