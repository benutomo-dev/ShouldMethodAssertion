using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IEnumerable<>))]
public partial struct EnumerableShouldContain<T>
{
    public void ShouldContain(T value, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (!Actual.Contains(value, comparer))
        {
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is NOT contain {ParamExpressions.value.OneLine}.
                """);
        }
    }

    public void ShouldNotContain(T value, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual.Contains(value, comparer))
        {
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} contain {ParamExpressions.value.OneLine}.
                """);
        }
    }
}
