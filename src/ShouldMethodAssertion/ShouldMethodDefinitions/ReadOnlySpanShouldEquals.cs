using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(ReadOnlySpan<>))]
public partial struct ReadOnlySpanShouldEqual<T> // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    public void ShouldEqual(ReadOnlySpan<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (SequenceEqual(Actual, expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is not {ParamExpressions.expected}.");
    }

    public void ShouldNotEqual(ReadOnlySpan<T> expected, bool ignoreOrder, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (!SequenceEqual(Actual, expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is {ParamExpressions.expected}.");
    }

    private static bool SequenceEqual(ReadOnlySpan<T> actual, ReadOnlySpan<T> expected, IEqualityComparer<T> comparer)
    {
#if NETFRAMEWORK
        if (actual.Length != expected.Length)
            return false;

        for (int i = 0; i < actual.Length; i++)
        {
            if (!comparer.Equals(actual[i], expected[i]))
                return false;
        }

        return true;
#else
        return actual.SequenceEqual(expected, comparer);
#endif
    }
}
