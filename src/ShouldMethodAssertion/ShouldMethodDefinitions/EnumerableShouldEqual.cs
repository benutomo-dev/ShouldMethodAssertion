﻿using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ExpressionUtils;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IEnumerable<>))]
[StructLayout(LayoutKind.Auto)]
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

        if (!Actual.SequenceEqual(expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is {ParamExpressions.expected}.");
    }


    [StackTraceHidden]
    private void MatchWithOrdering(IEnumerable<T> expected, IEqualityComparer<T> comparer)
    {
        using var expectedEnumerator = expected.GetEnumerator();
        using var actualEnumerator = Actual.GetEnumerator();

        var comparerAnnotation = ParamExpressions.comparer.HasValue
            ? $"{ParamExpressions.comparer}による比較で"
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
                        {ActualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

                        {ParamExpressions.expected}
                        
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
                        {ActualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

                        {ParamExpressions.expected}
                        
                        {ActualExpression.OneLine}の要素数が期待値より不足しています。
                        """);
            }
            else
            {
                Debug.Assert(actualHasValue);

                throw AssertExceptionUtil.Create($"""
                        {ActualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

                        {ParamExpressions.expected}
                        
                        {ActualExpression.OneLine}の要素数が期待値より余分に存在します。
                        """);
            }
        }
    }

    [StackTraceHidden]
    private void MatchWithoutOrdering(IEnumerable<T> expected, IEqualityComparer<T> comparer)
    {
        var actualValuesHistgram = toValueHistgram(Actual, comparer);
        var expectedValuesHistgram = toValueHistgram(expected, comparer);

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

        if (differenceValueList.Count == 0)
            return;


        var differeceListTextBuilder = new StringBuilder();
        const int maxListingCount = 10;
        foreach (var entry in differenceValueList.Take(maxListingCount))
            differeceListTextBuilder.AppendLine(CultureInfo.InvariantCulture, $"[{entry.countInActual}, {entry.countInExpected}] : {ExpressionUtil.ToOneLineValueString(entry.value)}");

        if (differenceValueList.Count > maxListingCount)
            differeceListTextBuilder.AppendLine(CultureInfo.InvariantCulture, $"他、{differenceValueList.Count - maxListingCount}個の要素の格納数に相違がありました。");

        var comparingDescription = ParamExpressions.comparer.HasValue
            ? $"`{ParamExpressions.comparer}`による比較で並び順を無視して"
            : "並び順を無視した比較で";

        throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine}は{comparingDescription}以下と一致しなければなりませんが、一致しませんでした。

                {ParamExpressions.expected}
                
                以下にそれぞれのコレクションの差異を同じ項目に対する格納数の違いで表示します。
                [実際のコレクションに含まれている数, 期待値側のコレクションに含まれている数] : 対象項目
                {differeceListTextBuilder}
                """);

#nullable disable warnings
        static (Dictionary<T, int> valueCountTable, int nullCount) toValueHistgram(IEnumerable<T> values, IEqualityComparer<T> comparer)
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
                ref var valueCountRef = ref CollectionsMarshal.GetValueRefOrAddDefault(valueCountTable, value, out var exists);
                valueCountRef++;
#endif
            }

            return (valueCountTable, nullCount);
        }
#nullable restore
    }
}
