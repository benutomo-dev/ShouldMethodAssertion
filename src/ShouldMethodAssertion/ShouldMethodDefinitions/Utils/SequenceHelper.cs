using System.Diagnostics;
using System.Text;

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
    public static void ShouldEqualWithoutOrderingCore<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistogram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistogram, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
#nullable restore
    {
        var differenceValueList = SequenceHelper.MakeDifferenceValueList(actualValuesHistogram, expectedValuesHistogram);

        if (differenceValueList.Count == 0)
            return;

        throw AssertExceptionUtil.CreateBasicShouldEqualFailMessageByOrderIgnoredElementSet(differenceValueList, actualExpression, expectedExpression, comparerExpression);
    }

    public static void ShouldNotEqualWithOrderingCore(ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
    {
        throw AssertExceptionUtil.CreateBasicShouldNotEqualFailMessage(actualExpression, expectedExpression, isIncludingTheOrder: true, comparerExpression);
    }

#nullable disable warnings
    public static void ShouldNotEqualWithoutOrderingCore<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistogram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistogram, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
#nullable restore
    {
        if (!SequenceHelper.EqualsValuesHistogram(actualValuesHistogram, expectedValuesHistogram))
            return;

        throw AssertExceptionUtil.CreateBasicShouldNotEqualFailMessage(actualExpression, expectedExpression, isIncludingTheOrder: false, comparerExpression);
    }

#nullable disable warnings
    public static List<(T? value, int countInActual, int countInExpected)> MakeDifferenceValueList<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistogram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistogram)
#nullable restore
    {
        var differenceValueList = new List<(T? value, int countInActual, int countInExpected)>();

        if (actualValuesHistogram.nullCount != expectedValuesHistogram.nullCount)
            differenceValueList.Add((default(T), actualValuesHistogram.nullCount, expectedValuesHistogram.nullCount));

        foreach (var expectedValueEntry in expectedValuesHistogram.valueCountTable)
        {
#if NETFRAMEWORK
#nullable disable warnings
#endif
            if (!actualValuesHistogram.valueCountTable.Remove(expectedValueEntry.Key, out var actualValueCount))
                actualValueCount = 0;
#if NETFRAMEWORK
#nullable restore
#endif

            if (expectedValueEntry.Value != actualValueCount)
                differenceValueList.Add((expectedValueEntry.Key, actualValueCount, expectedValueEntry.Value));
        }

        foreach (var actualValueEntry in actualValuesHistogram.valueCountTable)
            differenceValueList.Add((actualValueEntry.Key, actualValueEntry.Value, 0));

        return differenceValueList;
    }

#nullable disable warnings
    public static bool EqualsValuesHistogram<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistogram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistogram)
#nullable restore
    {
        if (actualValuesHistogram.nullCount != expectedValuesHistogram.nullCount)
            return false;

        if (actualValuesHistogram.valueCountTable.Count != expectedValuesHistogram.valueCountTable.Count)
            return false;

        foreach (var actualValueEntry in actualValuesHistogram.valueCountTable)
        {
            if (expectedValuesHistogram.valueCountTable.TryGetValue(actualValueEntry.Key, out var expectedCount))
            {
                if (actualValueEntry.Value != expectedCount)
                    return false;
            }
            else
            {
                return false;
            }
        }

        foreach (var expectedTableEntry in expectedValuesHistogram.valueCountTable)
        {
            if (!expectedValuesHistogram.valueCountTable.TryGetValue(expectedTableEntry.Key, out _))
                return false;
        }

        return true;
    }

    internal static (List<T>? HeadValues, bool HasMoreValues, int? EnumeratedCount) GetHeadValues<T>(CommonEnumerator<T> firstHasValueEnumerator, int? nonEnumeratedCount)
    {
        var stringBuilder = new StringBuilder();

        const int maxHeadValueCount = 10;

        var headValues = (nonEnumeratedCount > 0) ? new List<T>(maxHeadValueCount) : null;

        bool hasMoreValues = true;

        for (int i = 0; i < maxHeadValueCount; i++)
        {
            if (!firstHasValueEnumerator.MoveNext())
            {
                hasMoreValues = false;
                break;
            }

            headValues ??= new List<T>(maxHeadValueCount);

            headValues.Add(firstHasValueEnumerator.Current);
        }

        var enumeratedCount = hasMoreValues ? nonEnumeratedCount : headValues?.Count;

        return (headValues, hasMoreValues, enumeratedCount);
    }
}
