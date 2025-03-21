﻿using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;
using System.Diagnostics;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IEnumerable<>))]
public partial struct EnumerableShouldEqual<T> // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    public void ShouldEqual(IEnumerable<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (ignoreOrder)
            MatchWithoutOrdering(expected, comparer);
        else
            MatchWithOrdering(expected, comparer);
    }

    public void ShouldNotEqual(IEnumerable<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (ignoreOrder)
            NotMatchWithoutOrdering(expected, comparer);
        else
            NotMatchWithOrdering(expected, comparer);
    }


    [StackTraceHidden]
    private void MatchWithOrdering(IEnumerable<T> expected, IEqualityComparer<T> comparer)
    {
        using var expectedEnumerator = expected.GetEnumerator();
        using var actualEnumerator = Actual.GetEnumerator();

        SequenceHelper.MatchWithOrderingCore(
            new CommonEnumerator<T>(actualEnumerator),
            new CommonEnumerator<T>(expectedEnumerator),
            comparer,
            ActualExpression,
            ParamExpressions.expected,
            ParamExpressions.comparer);
    }

    [StackTraceHidden]
    private void MatchWithoutOrdering(IEnumerable<T> expected, IEqualityComparer<T> comparer)
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

    private void NotMatchWithOrdering(IEnumerable<T> expected, IEqualityComparer<T> comparer)
    {
        if (!Actual.SequenceEqual(expected, comparer))
            return;

        SequenceHelper.NotMatchWithOrderingCore(
            ActualExpression,
            ParamExpressions.expected,
            ParamExpressions.comparer);
    }

    private void NotMatchWithoutOrdering(IEnumerable<T> expected, IEqualityComparer<T> comparer)
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

#nullable disable warnings
    private static (Dictionary<T, int> valueCountTable, int nullCount) ToValueHistgram(IEnumerable<T> values, IEqualityComparer<T> comparer)
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
