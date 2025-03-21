using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(TypeArg1))]
public partial struct StructShouldBe<T> where T : struct
{
    public void ShouldBe(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (comparer.Equals(Actual, expected))
            return;

        throw AssertExceptionUtil.CreateBasicShouleBeFailMessage(Actual, ActualExpression, expected, ParamExpressions.expected);
    }

    public void ShouldNotBe(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (!comparer.Equals(Actual, expected))
            return;

        throw AssertExceptionUtil.CreateBasicShouldNotBeFailMessage(Actual, ActualExpression, expected, ParamExpressions.expected);
    }
}
