using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;
using System;
using System.Diagnostics;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(ReadOnlySpan<>))]
public partial struct ReadOnlySpanShouldEqual<T> // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    public void ShouldEqual(ReadOnlySpan<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (ignoreOrder)
            MatchWithoutOrdering(expected, comparer);
        else
            MatchWithOrdering(expected, comparer);
    }

    public void ShouldNotEqual(ReadOnlySpan<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (ignoreOrder)
            NotMatchWithoutOrdering(expected, comparer);
        else
            NotMatchWithOrdering(expected, comparer);
    }

    [StackTraceHidden]
    private void MatchWithOrdering(ReadOnlySpan<T> expected, IEqualityComparer<T> comparer)
    {
        var expectedEnumerator = expected.GetEnumerator();
        var actualEnumerator = Actual.GetEnumerator();

        SequenceHelper.MatchWithOrderingCore(
            new CommonEnumerator<T>(actualEnumerator),
            new CommonEnumerator<T>(expectedEnumerator),
            comparer,
            ActualExpression,
            ParamExpressions.expected,
            ParamExpressions.comparer);
    }

    [StackTraceHidden]
    private void MatchWithoutOrdering(ReadOnlySpan<T> expected, IEqualityComparer<T> comparer)
    {
        var actualValuesHistgram = ToValueHistgram(Actual, comparer);
        var expectedValuesHistgram = ToValueHistgram(expected, comparer);

        SequenceHelper.MatchWithoutOrderingCore(
            actualValuesHistgram,
            expectedValuesHistgram,
            ActualExpression,
            ParamExpressions.expected,
            ParamExpressions.comparer);
    }

    private void NotMatchWithOrdering(ReadOnlySpan<T> expected, IEqualityComparer<T> comparer)
    {
        if (!SequenceEqual(Actual, expected, comparer))
            return;

        SequenceHelper.NotMatchWithOrderingCore(
            ActualExpression,
            ParamExpressions.expected,
            ParamExpressions.comparer);
    }

    private void NotMatchWithoutOrdering(ReadOnlySpan<T> expected, IEqualityComparer<T> comparer)
    {
        var actualValuesHistgram = ToValueHistgram(Actual, comparer);
        var expectedValuesHistgram = ToValueHistgram(expected, comparer);

        SequenceHelper.NotMatchWithoutOrderingCore(
            actualValuesHistgram,
            expectedValuesHistgram,
            ActualExpression,
            ParamExpressions.expected,
            ParamExpressions.comparer);
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

#nullable disable warnings
    private static (Dictionary<T, int> valueCountTable, int nullCount) ToValueHistgram(ReadOnlySpan<T> values, IEqualityComparer<T> comparer)
    {
        var valueCountTable = new Dictionary<T, int>(comparer);
        int nullCount = 0;
        foreach (var value in values)
        {
            if (value is null)
            {
                nullCount++;
                continue;
            }

#if NETFRAMEWORK
            if (valueCountTable.TryGetValue(value, out var valueCount))
                valueCount++;
            else
                valueCount = 1;

            valueCountTable[value] = valueCount;
#else
            ref var valueCountRef = ref System.Runtime.InteropServices.CollectionsMarshal.GetValueRefOrAddDefault(valueCountTable, value, out var exists);
            valueCountRef++;
#endif
        }

        return (valueCountTable, nullCount);
    }
#nullable restore
}
