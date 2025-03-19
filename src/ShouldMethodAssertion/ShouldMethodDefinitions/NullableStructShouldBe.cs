using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Nullable<>))]
public partial struct NullableStructShouldBe<T> where T : struct
{
    public void ShouldBe(T? expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is null && expected is null)
            return;

        if (Actual is T actual && expected is T notNullExpected)
        {
            if (comparer.Equals(actual, notNullExpected))
                return;
        }

        throw AssertExceptionUtil.CreateSimpleIsStyleMessage(Actual, ActualExpression, expected, ParamExpressions.expected);
    }

    public void ShouldNotBe(T? expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is null && expected is null)
            throw AssertExceptionUtil.Create($"Both {ActualExpression.OneLine} and {ParamExpressions.expected.OneLine} are null.");

        if (Actual is T actual && expected is T notNullExpected)
        {
            if (comparer.Equals(actual, notNullExpected!))
                throw AssertExceptionUtil.CreateSimpleIsNotStyleMessage(Actual, ActualExpression, expected, ParamExpressions.expected);
        }
    }
}
