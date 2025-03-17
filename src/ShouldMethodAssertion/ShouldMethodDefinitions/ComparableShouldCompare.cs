using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IComparable<>))]
public partial struct ComparableShouldCompare<T>
{
    public void ShouldLessThan(T expected, IComparer<T>? comparer = null)
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

        throw AssertExceptionUtil.Create($"{ActualExpression} is not less than {ParamExpressions.expected}.");
    }

    public void ShouldGreaterThan(T expected, IComparer<T>? comparer = null)
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

        throw AssertExceptionUtil.Create($"{ActualExpression} is not greater than {ParamExpressions.expected}.");
    }
}
