using System.CodeDom;
using System.Diagnostics;

namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

internal static class SequenceHelper
{
    public const int MaxListingCount = 10;

    public static void ShouldEqualWithOrderingCore<T>(CommonEnumerator<T> actualEnumerator, CommonEnumerator<T> expectedEnumerator, IEqualityComparer<T> comparer, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
    {
        for (int i = 0; true; i++)
        {
            var expectedHasValue = expectedEnumerator.MoveNext();
            var actualHasValue = actualEnumerator.MoveNext();

            if (expectedHasValue && actualHasValue)
            {
                var expectedCurrent = expectedEnumerator.Current;
                var actualCurrent = actualEnumerator.Current;

                if (comparer.Equals(expectedCurrent, actualCurrent))
                {
                    continue;
                }

                throw AssertExceptionUtil.CreateBasicShouldEqualFailMessageByDifferentNthElement(
                    i,
                    actualCurrent,
                    expectedCurrent,
                    actualExpression,
                    expectedExpression,
                    comparerExpression);
            }
            else if (!expectedHasValue && !actualHasValue)
            {
                return;
            }
            else if (expectedHasValue)
            {
                throw AssertExceptionUtil.CreateBasicShouldEqualFailMessageByDifferentOfCount(
                    AssertExceptionUtil.CountNotMatchReason.ActualCountLessThanExpectedCount,
                    actualExpression,
                    expectedExpression,
                    isIncludingTheOrder: true,
                    comparerExpression);
            }
            else
            {
                Debug.Assert(actualHasValue);

                throw AssertExceptionUtil.CreateBasicShouldEqualFailMessageByDifferentOfCount(
                    AssertExceptionUtil.CountNotMatchReason.ActualCountMoreThanExpectedCount,
                    actualExpression,
                    expectedExpression,
                    isIncludingTheOrder: true,
                    comparerExpression);
            }
        }
    }

#nullable disable warnings
    public static void ShouldEqualWithoutOrderingCore<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistgram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistgram, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
#nullable restore
    {
        var differenceValueList = SequenceHelper.MakeDifferenceValueList(actualValuesHistgram, expectedValuesHistgram);

        if (differenceValueList.Count == 0)
            return;

        throw AssertExceptionUtil.CreateBasicShouldEqualFailMessageByOrderIgnoredElementSet(differenceValueList, actualExpression, expectedExpression, comparerExpression);
    }

    public static void ShouldNotEqualWithOrderingCore(ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
    {
        throw AssertExceptionUtil.CreateBasicShouldNotEqualFailMessage(actualExpression, expectedExpression, isIncludingTheOrder: true, comparerExpression);
    }

#nullable disable warnings
    public static void ShouldNotEqualWithoutOrderingCore<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistgram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistgram, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
#nullable restore
    {
        if (!SequenceHelper.EqualsValuesHistgram(actualValuesHistgram, expectedValuesHistgram))
            return;

        throw AssertExceptionUtil.CreateBasicShouldNotEqualFailMessage(actualExpression, expectedExpression, isIncludingTheOrder: false, comparerExpression);
    }

#nullable disable warnings
    public static List<(T? value, int countInActual, int countInExpected)> MakeDifferenceValueList<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistgram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistgram)
#nullable restore
    {
        var differenceValueList = new List<(T? value, int countInActual, int countInExpected)>();

        if (actualValuesHistgram.nullCount != expectedValuesHistgram.nullCount)
            differenceValueList.Add((default(T), actualValuesHistgram.nullCount, expectedValuesHistgram.nullCount));

        foreach (var expectedValueEntry in expectedValuesHistgram.valueCountTable)
        {
#if NETFRAMEWORK
#nullable disable warnings
#endif
            if (!actualValuesHistgram.valueCountTable.Remove(expectedValueEntry.Key, out var actualValueCount))
                actualValueCount = 0;
#if NETFRAMEWORK
#nullable restore
#endif

            if (expectedValueEntry.Value != actualValueCount)
                differenceValueList.Add((expectedValueEntry.Key, actualValueCount, expectedValueEntry.Value));
        }

        foreach (var actualValueEntry in actualValuesHistgram.valueCountTable)
            differenceValueList.Add((actualValueEntry.Key, actualValueEntry.Value, 0));

        return differenceValueList;
    }

#nullable disable warnings
    public static bool EqualsValuesHistgram<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistgram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistgram)
#nullable restore
    {
        if (actualValuesHistgram.nullCount != expectedValuesHistgram.nullCount)
            return false;

        if (actualValuesHistgram.valueCountTable.Count != expectedValuesHistgram.valueCountTable.Count)
            return false;

        foreach (var actualValueEntry in actualValuesHistgram.valueCountTable)
        {
            if (expectedValuesHistgram.valueCountTable.TryGetValue(actualValueEntry.Key, out var expectedCount))
            {
                if (actualValueEntry.Value != expectedCount)
                    return false;
            }
        }

        return true;
    }
}
