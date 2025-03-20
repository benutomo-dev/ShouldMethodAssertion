using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object), AcceptNullReference = true)]
public partial struct ObjectShouldBe<T>
{
    public void ShouldBe(T? expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is null && expected is null)
            return;

        if (Actual is T actual && comparer.Equals(actual, expected!))
            return;

        throw AssertExceptionUtil.CreateSimpleIsStyleMessage(Actual, ActualExpression, expected, ParamExpressions.expected);
    }

    public void ShouldNotBe(T? expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is null && expected is null)
            throw AssertExceptionUtil.Create($"Both {ActualExpression.OneLine} and {ParamExpressions.expected.OneLine} are null.");

        if (Actual is not T actual || !comparer.Equals(actual, expected!))
            return;

        throw AssertExceptionUtil.CreateSimpleIsNotStyleMessage(Actual, ActualExpression, expected, ParamExpressions.expected);
    }
}
