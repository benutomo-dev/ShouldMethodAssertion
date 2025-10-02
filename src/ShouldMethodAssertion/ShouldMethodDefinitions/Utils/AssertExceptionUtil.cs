using ShouldMethodAssertion.ShouldMethodDefinitions.Exceptions;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

public static partial class AssertExceptionUtil
{
    private interface IExceptionFactory
    {
        Exception Create(string message, Exception? exception);
    }

    private sealed class DefaultExceptionFactory : IExceptionFactory
    {
        public Exception Create(string message, Exception? exception)
        {
            if (exception is null)
                return new Exceptions.ShouldMethodAssertionException(message);
            else
                return new Exceptions.ShouldMethodAssertionException(message, exception);
        }
    }

    private sealed class XunitExceptionFactory : IExceptionFactory
    {
        public Exception Create(string message, Exception? exception)
        {
            if (exception is null)
                return new Xunit.Sdk.ShouldMethodAssertionException(message);
            else
                return new Xunit.Sdk.ShouldMethodAssertionException(message, exception);
        }
    }

    // lang=Regex
    private const string LineFeedRegexValue = @"\r?\n";

#if NET8_0_OR_GREATER
    [GeneratedRegex(LineFeedRegexValue)]
    private static partial Regex LineFeedRegex();
#else
    private static Regex s_lineFeedRegex = new Regex(LineFeedRegexValue);
    private static Regex LineFeedRegex() => s_lineFeedRegex;
#endif


    private static Lazy<IExceptionFactory> _exceptionFactory = new Lazy<IExceptionFactory>(CreateFactory);

    private static IExceptionFactory CreateFactory()
    {
        var x = AppDomain.CurrentDomain.GetAssemblies();

        if (AppDomain.CurrentDomain.GetAssemblies().Any(v => v.GetName().Name is "xunit.core" or "xunit.v3.core"))
        {
            return new XunitExceptionFactory();
        }
        else
        {
            return new DefaultExceptionFactory();
        }
    }

    internal static Exception CreateBasicShouleBeFailMessage<TActual, TExpected>(TActual actualValue, ValueExpression actualExpression, TExpected expectedValue, ValueExpression expectedExpression, Exception? exception = null)
    {
        var stringBuilder = new StringBuilder();

        var actualValueText = ExpressionUtil.FormartValue(actualValue);
        var expectedValueText = ExpressionUtil.FormartValue(expectedValue);

        if (expectedValueText.Contains('\n') || expectedValueText.Length > 30)
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} is not expected.");
            stringBuilder.AppendLine();
        }
        else
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} is {expectedValueText}. But did not expect it to be.");
            stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[Actual]");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualValueText}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[Expected]");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{expectedValueText}");

        return Create(stringBuilder.ToString(), exception);
    }

    internal static Exception CreateBasicShouldNotBeFailMessage<TActual, TExpected>(TActual actualValue, ValueExpression actualExpression, TExpected expectedValue, ValueExpression expectedExpression, Exception? exception = null)
    {
        var stringBuilder = new StringBuilder();

        var actualValueText = ExpressionUtil.FormartValue(actualValue);

        if (actualValueText.Contains('\n') || actualValueText.Length > 30)
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"The value of {actualExpression.OneLine} is not expected.");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[Actual]");
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualValueText}");
        }
        else
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} is {actualValueText}. But did not expect it to be.");
            stringBuilder.AppendLine();
        }

        return Create(stringBuilder.ToString(), exception);
    }

    internal static Exception CreateBasicShouldEqualFailMessageByDifferentNthElement<TActual, TExpected>(int defferentElementIndex, TActual actualElementValue, TExpected expectedElementValue, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression, Exception? exception = null)
    {
        var comparerAnnotation = comparerExpression.HasValue
            ? $" when compared using {comparerExpression}"
            : $"";

        var actualValueText = ExpressionUtil.FormartValue(actualElementValue);
        var expectedValueText = ExpressionUtil.FormartValue(expectedElementValue);

        var stringBuilder = new StringBuilder();

        if (expectedExpression.IsMultiLine || expectedExpression.OneLine.Length > 30)
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} must match the following{comparerAnnotation}, but it did not match.");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(expectedExpression.Default);
            stringBuilder.AppendLine();
        }
        else
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} must match {expectedExpression.OneLine}{comparerAnnotation}, but it did not match.");
            stringBuilder.AppendLine();
        }

        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"The content of {actualExpression.OneLine}[{defferentElementIndex}] is different.");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"");

        if (actualValueText.Contains('\n') || expectedValueText.Contains('\n'))
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"Actual:   {actualValueText}");
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"Expected: {expectedValueText}");
        }
        else
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[Actual]");
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualValueText}");
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"");
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[Expected]");
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{expectedValueText}");
        }

        return Create(stringBuilder.ToString(), exception);

        //$"""
        //{actualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

        //{expectedExpression}

        //{count}番目の要素の内容が異なっています。
        //期待値: {ExpressionUtil.ToOneLineValueString(expectedCurrent)}
        //実際値: {ExpressionUtil.ToOneLineValueString(actualCurrent)}
        //"""
    }

    internal static Exception CreateBasicShouldEqualFailMessageByOrderIgnoredElementSet<T>(List<(T? value, int countInActual, int countInExpected)> differenceValueList, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression, Exception? exception = null)
    {
        var comparerAnnotation = comparerExpression.HasValue
            ? $" and using {comparerExpression}"
            : $"";

        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} must match the following when compared with `ignoreOrder`{comparerAnnotation}, but it did not match.");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{expectedExpression.OneLine}");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"Below shows the differences between each collection in terms of the number of items for the same element.");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"Target item : {{Number contained in actual collection, Number contained in expected collection}}");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"");
        
        foreach (var entry in differenceValueList.Take(SequenceHelper.MaxListingCount))
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{ExpressionUtil.FormatValueAsOneline(entry.value, maxLength: 80)} : {{NumberInActual:{entry.countInActual}, NumberInExpected:{entry.countInExpected}}}");

        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"");


        if (differenceValueList.Count > SequenceHelper.MaxListingCount)
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"Additionally, there were differences in the count of {differenceValueList.Count - SequenceHelper.MaxListingCount} other elements.");

        return AssertExceptionUtil.Create(stringBuilder.ToString(), exception);


        //$$"""
        //{{actualExpression.OneLine}}は{{comparingDescription}}以下と一致しなければなりませんが、一致しませんでした。

        //{{expectedExpression}}
                
        //以下にそれぞれのコレクションの差異を同じ項目に対する格納数の違いで表示します。
        //対象項目 : {実際のコレクションに含まれている数, 期待値側のコレクションに含まれている数}
        //{{stringBuilder}}
        //"""
    }

    internal enum CountNotMatchReason
    {
        ActualCountLessThanExpectedCount,
        ActualCountMoreThanExpectedCount,
    }

    internal static Exception CreateBasicShouldEqualFailMessageByDifferentOfCount(CountNotMatchReason reason, ValueExpression actualExpression, ValueExpression expectedExpression, bool? isIncludingTheOrder, NullableValueExpression comparerExpression, Exception? exception = null)
    {
        var comparerAnnotation = comparerExpression.HasValue
            ? $" when compared using {comparerExpression}"
            : $"";

        var orderAnnotation = isIncludingTheOrder switch
        {
            true => $"including the order, ",
            false => $"without the order, ",
            _ => $"",
        };

        var countDiffereceMessage = reason == CountNotMatchReason.ActualCountMoreThanExpectedCount
            ? $" more elements than expected"
            : $" less elements than expected";


        var stringBuilder = new StringBuilder();

        if (expectedExpression.IsMultiLine || expectedExpression.OneLine.Length > 30)
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} must match the following, {orderAnnotation}but it did not match.");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(expectedExpression.Default);
            stringBuilder.AppendLine();
        }
        else
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} must match {expectedExpression.OneLine}, {orderAnnotation}but it did not match.");
            stringBuilder.AppendLine();
        }
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} contains{countDiffereceMessage}.");

        return Create(stringBuilder.ToString(), exception);

        //$"""
        //{actualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

        //{expectedExpression}

        //{actualExpression.OneLine}の要素数が期待値より不足しています。
        //"""
    }

    internal static Exception CreateBasicShouldNotEqualFailMessage(ValueExpression actualExpression, ValueExpression expectedExpression, bool? isIncludingTheOrder, NullableValueExpression comparerExpression, Exception? exception = null)
    {
        var comparerAnnotation = comparerExpression.HasValue
            ? $" when compared using {comparerExpression}"
            : $"";

        var orderAnnotation = isIncludingTheOrder switch
        {
            true => $" including the order,",
            false => $" without the order,",
            _ => $"",
        };

        var stringBuilder = new StringBuilder();

        if (expectedExpression.IsMultiLine || expectedExpression.OneLine.Length > 30)
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} matches the following{comparerAnnotation},{orderAnnotation}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(expectedExpression.Default);
            stringBuilder.AppendLine();
        }
        else
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} matches {expectedExpression.OneLine}{comparerAnnotation},{orderAnnotation}");
            stringBuilder.AppendLine();
        }

        return Create(stringBuilder.ToString(), exception);
    }

    public static Exception CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(Exception actualException, Type expectedExceptionType, ValueExpression actualExpression)
    {
        return Create($"""
            Wrong exception type thrown by {actualExpression.OneLine}.

            Expected: `{expectedExceptionType.FullName}`
            Actual: `{actualException.GetType().FullName}` with messge "{actualException.Message}"
            """, actualException);
    }

    public static Exception CreateBasicShouldThrowFailByNoThrownMessage(ValueExpression actualExpression)
    {
        return Create($"No exception was thrown by {actualExpression.OneLine}.");
    }

    public static Exception CreateBasicShouldNotThrowFailByUnexpectedExceptionThrownMessage(Exception actualException, ValueExpression actualExpression)
    {
        return Create($"""
            {actualExpression.OneLine} threw an exception of type `{actualException.GetType().FullName}`.

            Message: "{actualException.Message}"
            """, actualException);
    }

    const int ListingKeyStringMaxLength = 80;
    const int ListingValueStringMaxLength = 500;

    internal static Exception CreateBasicShouldEmptyFail<T>(List<T> headValues, bool hasMoreValues, int? enumeratedCount, ValueExpression actualExpression)
    {
        var stringBuilder = new StringBuilder();

        if (enumeratedCount is null)
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} is not empty.");
        else
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} have {enumeratedCount} elements.");

        stringBuilder.AppendLine();

        if (hasMoreValues)
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"List of first {headValues.Count} elements:");
        else
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"List of all elements:");

        for (int i = 0; i < headValues.Count; i++)
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[{i}] {ExpressionUtil.FormatValueAsOneline(headValues[i], ListingValueStringMaxLength)}");
        }

        return Create(stringBuilder.ToString());
    }

    internal static Exception CreateBasicShouldEmptyFail<TKey, TValue>(List<KeyValuePair<TKey, TValue>> headValues, bool hasMoreValues, int? enumeratedCount, ValueExpression actualExpression)
    {
        var stringBuilder = new StringBuilder();

        if (enumeratedCount is null)
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} is not empty.");
        else
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} have {enumeratedCount} entries.");

        stringBuilder.AppendLine();

        if (hasMoreValues)
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"List of first {headValues.Count} entries:");
        else
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"List of all entries:");

        for (int i = 0; i < headValues.Count; i++)
        {
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[{ExpressionUtil.FormatValueAsOneline(headValues[i].Key, ListingKeyStringMaxLength)}] {ExpressionUtil.FormatValueAsOneline(headValues[i].Value, ListingValueStringMaxLength)}");
        }

        return Create(stringBuilder.ToString());
    }

    internal static Exception CreateBasicShouldNotEmptyFail<T>(ValueExpression actualExpression)
    {
        return Create($"{actualExpression.OneLine} is empty.");
    }


    internal static Exception CreateBasicShouldAllSatisfyFail<TKey, TValue>(List<(TKey key, TValue value, Exception exception)> fails, ValueExpression actualExpression, [CallerFilePath] string callerSourceFilePath = "unknown path")
    {
        const string IndentSpace = "  ";

        var builder = new StringBuilder();

        builder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} has not satisfied element.");
        builder.AppendLine("");

        foreach (var fail in fails)
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"--- [{ExpressionUtil.FormatValueAsOneline(fail.key)}]: {ExpressionUtil.FormatValueAsOneline(fail.value)} ---");

            if (fail.exception is IShouldMethodAssertionException)
            {
                appendWithIndent(builder, fail.exception.Message);
            }
            else
            {
                appendWithIndent(builder, $"{fail.exception.GetType().FullName}: {fail.exception.Message}");
            }

            if (fail.exception.StackTrace is not null)
            {
                builder.AppendLine("");

                var lines = LineFeedRegex().Split(fail.exception.StackTrace);

                // 自分自身の呼出しと action の呼出し分のスタックトレース数
                var selfStackTraceCount = lines.AsEnumerable().Reverse().TakeWhile(v => v.Contains(callerSourceFilePath, StringComparison.InvariantCulture)).Count() + 1;

                foreach (var line in lines.Take(lines.Length - selfStackTraceCount))
                {
                    builder.Append(IndentSpace);
                    builder.AppendLine(line.TrimStart());
                }
            }
        }

        throw AssertExceptionUtil.Create(builder.ToString());


        static void appendWithIndent(StringBuilder builder, string text)
        {
            var lines = LineFeedRegex().Split(text);
            foreach (var line in lines)
            {
                builder.Append(IndentSpace);
                builder.AppendLine(line);
            }
        }
    }

    public static Exception Create(string message, Exception? exception = null) =>  _exceptionFactory.Value.Create(message, exception);

}
