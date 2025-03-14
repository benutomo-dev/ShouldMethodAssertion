using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IComparable<>))]
public partial struct ComparableShouldCompare<T> // ShouldMethod属性で指定した型と同じ数と制約の型引数
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
