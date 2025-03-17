using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IComparable<>))]
public partial struct ComparableShouldCompare<T>
{
    public void ShouldBeLessThan(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Actual.CompareTo(expected) < 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Actual, expected) < 0)
                return;
        }

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not less than {ParamExpressions.expected.OneLine}.");
    }

    public void ShouldBeLessThanOrEqual(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Actual.CompareTo(expected) <= 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Actual, expected) <= 0)
                return;
        }

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not less than or equal {ParamExpressions.expected.OneLine}.");
    }

    public void ShouldBeGreaterThan(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Actual.CompareTo(expected) > 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Actual, expected) > 0)
                return;
        }

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not greater than {ParamExpressions.expected.OneLine}.");
    }

    public void ShouldBeGreaterThanOrEqual(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Actual.CompareTo(expected) >= 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Actual, expected) >= 0)
                return;
        }

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not greater than or equal {ParamExpressions.expected.OneLine}.");
    }

    public void ShouldBeInRange(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Actual.CompareTo(expected) > 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Actual, expected) > 0)
                return;
        }

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not greater than {ParamExpressions.expected.OneLine}.");
    }
}
