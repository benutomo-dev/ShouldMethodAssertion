using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(TypeArg1))]
public partial struct StructShouldBe<T> where T : struct
{
    public void ShouldBe(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is T actual && comparer.Equals(actual, expected!))
            return;

        throw AssertExceptionUtil.CreateSimpleIsStyleMessage(Actual, ActualExpression, expected, ParamExpressions.expected);
    }

    public void ShouldNotBe(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (!comparer.Equals(Actual, expected!))
            return;

        throw AssertExceptionUtil.CreateSimpleIsNotStyleMessage(Actual, ActualExpression, expected, ParamExpressions.expected);
    }
}
