using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object))]
public partial struct ObjectShouldBeOneOf
{
    public void ShouldBeOneOf<T>(ReadOnlySpan<T> expectedList, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        foreach (var expected in expectedList)
        {
            if (Actual is T actual && comparer.Equals(actual, expected))
                return;
        }

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not one of {ParamExpressions.expectedList.OneLine}.");
    }

    public void ShouldNotBeOneOf<T>(ReadOnlySpan<T> expectedList, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        foreach (var expected in expectedList)
        {
            if (Actual is T actual && comparer.Equals(actual, expected))
                throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is one of {ParamExpressions.expectedList.OneLine}.");
        }

        return;
    }
}
