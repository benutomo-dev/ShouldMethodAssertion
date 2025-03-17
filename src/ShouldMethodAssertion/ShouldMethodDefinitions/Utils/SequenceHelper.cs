using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

internal static class SequenceHelper
{
    public const int MaxListingCount = 10;

    public static void MatchWithOrderingCore<T>(CommonEnumerator<T> actualEnumerator, CommonEnumerator<T> expectedEnumerator, IEqualityComparer<T> comparer, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
    {
        var comparerAnnotation = comparerExpression.HasValue
            ? $"{comparerExpression}による比較で"
            : "";

        int count = 0;
        while (true)
        {
            var expectedHasValue = expectedEnumerator.MoveNext();
            var actualHasValue = actualEnumerator.MoveNext();

            if (expectedHasValue && actualHasValue)
            {
                count++;

                var expectedCurrent = expectedEnumerator.Current;
                var actualCurrent = actualEnumerator.Current;

                if (comparer.Equals(expectedCurrent, actualCurrent))
                {
                    continue;
                }

                throw AssertExceptionUtil.Create($"""
                        {actualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

                        {expectedExpression}
                        
                        {count}番目の要素の内容が異なっています。
                        期待値: {ExpressionUtil.ToOneLineValueString(expectedCurrent)}
                        実際値: {ExpressionUtil.ToOneLineValueString(actualCurrent)}
                        """);
            }
            else if (!expectedHasValue && !actualHasValue)
            {
                return;
            }
            else if (expectedHasValue)
            {
                throw AssertExceptionUtil.Create($"""
                        {actualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

                        {expectedExpression}
                        
                        {actualExpression.OneLine}の要素数が期待値より不足しています。
                        """);
            }
            else
            {
                Debug.Assert(actualHasValue);

                throw AssertExceptionUtil.Create($"""
                        {actualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

                        {expectedExpression}
                        
                        {actualExpression.OneLine}の要素数が期待値より余分に存在します。
                        """);
            }
        }
    }

#nullable disable warnings
    public static void MatchWithoutOrderingCore<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistgram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistgram, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
#nullable restore
    {
        var differenceValueList = SequenceHelper.MakeDifferenceValueList(actualValuesHistgram, expectedValuesHistgram);

        if (differenceValueList.Count == 0)
            return;

        var differeceListTextBuilder = new StringBuilder();

        foreach (var entry in differenceValueList.Take(SequenceHelper.MaxListingCount))
            differeceListTextBuilder.AppendLine(CultureInfo.InvariantCulture, $"[{entry.countInActual}, {entry.countInExpected}] : {ExpressionUtil.ToOneLineValueString(entry.value)}");

        if (differenceValueList.Count > SequenceHelper.MaxListingCount)
            differeceListTextBuilder.AppendLine(CultureInfo.InvariantCulture, $"他、{differenceValueList.Count - SequenceHelper.MaxListingCount}個の要素の格納数に相違がありました。");

        var comparingDescription = comparerExpression.HasValue
            ? $"{comparerExpression}による比較で並び順を無視して"
            : "並び順を無視した比較で";

        throw AssertExceptionUtil.Create($"""
                {actualExpression.OneLine}は{comparingDescription}以下と一致しなければなりませんが、一致しませんでした。

                {expectedExpression}
                
                以下にそれぞれのコレクションの差異を同じ項目に対する格納数の違いで表示します。
                [実際のコレクションに含まれている数, 期待値側のコレクションに含まれている数] : 対象項目
                {differeceListTextBuilder}
                """);
    }

    public static void NotMatchWithOrderingCore(ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
    {
        var comparerAnnotation = comparerExpression.HasValue
            ? $"{comparerExpression}による比較で"
            : "";

        throw AssertExceptionUtil.Create($"""
            {actualExpression.OneLine}は{comparerAnnotation}以下と一致しています。

            {expectedExpression}
            """);
    }

#nullable disable warnings
    public static void NotMatchWithoutOrderingCore<T>((Dictionary<T, int> valueCountTable, int nullCount) actualValuesHistgram, (Dictionary<T, int> valueCountTable, int nullCount) expectedValuesHistgram, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
#nullable restore
    {
        if (!SequenceHelper.EqualsValuesHistgram(actualValuesHistgram, expectedValuesHistgram))
            return;

        var comparerAnnotation = comparerExpression.HasValue
            ? $"{comparerExpression}による並び順を無視した比較で"
            : "";

        throw AssertExceptionUtil.Create($"""
            {actualExpression.OneLine}は{comparerAnnotation}以下と一致しています。

            {expectedExpression}
            """);
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
