using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IComparable<>))]
public partial struct ComparableShouldCompare<T>
{
    public void ShouldLessThan(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Context.Actual.CompareTo(expected) < 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Context.Actual, expected) < 0)
                return;
        }

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not less than `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldGreaterThan(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Context.Actual.CompareTo(expected) > 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Context.Actual, expected) > 0)
                return;
        }

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not greater than `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}
