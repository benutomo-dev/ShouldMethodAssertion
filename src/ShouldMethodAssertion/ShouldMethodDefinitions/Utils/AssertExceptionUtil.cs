using ShouldMethodAssertion.ShouldMethodDefinitions.Exceptions;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

public static partial class AssertExceptionUtil
{
    private interface IExceptionFactory
    {
        Exception Create(string message, Exception? exception, string? stackTrace);
    }

    private sealed class DefaultExceptionFactory : IExceptionFactory
    {
        public Exception Create(string message, Exception? exception, string? stackTrace)
        {
            var assersionException = exception is null
                ? new Exceptions.ShouldMethodAssertionException(message)
                : new Exceptions.ShouldMethodAssertionException(message, exception);

            if (stackTrace is null)
                return assersionException;
            else
                return assersionException.WithStackTrace(stackTrace);
        }
    }

    private sealed class XunitExceptionFactory : IExceptionFactory
    {
        public Exception Create(string message, Exception? exception, string? stackTrace)
        {
            var assersionException = exception is null
                ? new Xunit.Sdk.ShouldMethodAssertionException(message)
                : new Xunit.Sdk.ShouldMethodAssertionException(message, exception);

            if (stackTrace is null)
                return assersionException;
            else
                return assersionException.WithStackTrace(stackTrace);
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

    internal static Exception CreateBasicShouldBeFailMessage<TActual, TExpected>(TActual actualValue, ValueExpression actualExpression, TExpected expectedValue, ValueExpression expectedExpression)
    {
        var stringBuilder = new StringBuilder();

        var actualValueText = ExpressionUtil.FormatValue(actualValue);
        var expectedValueText = ExpressionUtil.FormatValue(expectedValue);

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

        return Create(stringBuilder.ToString());
    }

    internal static Exception CreateBasicShouldNotBeFailMessage<TActual, TExpected>(TActual actualValue, ValueExpression actualExpression, TExpected expectedValue, ValueExpression expectedExpression)
    {
        var stringBuilder = new StringBuilder();

        var actualValueText = ExpressionUtil.FormatValue(actualValue);

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

        return Create(stringBuilder.ToString());
    }

    internal static Exception CreateBasicShouldEqualFailMessageByDifferentNthElement<TActual, TExpected>(int differentElementIndex, TActual actualElementValue, TExpected expectedElementValue, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
    {
        var comparerAnnotation = comparerExpression.HasValue
            ? $" when compared using {comparerExpression}"
            : $"";

        var actualValueText = ExpressionUtil.FormatValue(actualElementValue);
        var expectedValueText = ExpressionUtil.FormatValue(expectedElementValue);

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

        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"The content of {actualExpression.OneLine}[{differentElementIndex}] is different.");
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

        return Create(stringBuilder.ToString());

        //$"""
        //{actualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

        //{expectedExpression}

        //{count}番目の要素の内容が異なっています。
        //期待値: {ExpressionUtil.ToOneLineValueString(expectedCurrent)}
        //実際値: {ExpressionUtil.ToOneLineValueString(actualCurrent)}
        //"""
    }

    internal static Exception CreateBasicShouldEqualFailMessageByOrderIgnoredElementSet<T>(List<(T? value, int countInActual, int countInExpected)> differenceValueList, ValueExpression actualExpression, ValueExpression expectedExpression, NullableValueExpression comparerExpression)
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

        return AssertExceptionUtil.Create(stringBuilder.ToString());


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

    internal static Exception CreateBasicShouldEqualFailMessageByDifferentOfCount(CountNotMatchReason reason, ValueExpression actualExpression, ValueExpression expectedExpression, bool? isIncludingTheOrder, NullableValueExpression comparerExpression)
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
            : $" fewer elements than expected";


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

        return Create(stringBuilder.ToString());

        //$"""
        //{actualExpression.OneLine}は並び順を含めて{comparerAnnotation}以下と一致しなければなりませんが、一致しませんでした。

        //{expectedExpression}

        //{actualExpression.OneLine}の要素数が期待値より不足しています。
        //"""
    }

    internal static Exception CreateBasicShouldNotEqualFailMessage(ValueExpression actualExpression, ValueExpression expectedExpression, bool? isIncludingTheOrder, NullableValueExpression comparerExpression)
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

        return Create(stringBuilder.ToString());
    }

    public static Exception CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(Exception actualException, Type expectedExceptionType, ValueExpression actualExpression)
    {
        return Create($"""
            Wrong exception type thrown by {actualExpression.OneLine}.

            Expected: `{expectedExceptionType.FullName}`
            Actual: `{actualException.GetType().FullName}` with message "{actualException.Message}"
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
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} has {enumeratedCount} elements.");

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
            stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} has {enumeratedCount} entries.");

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


    internal static Exception CreateBasicShouldSatisfyFail(Exception fail, ValueExpression actualExpression, string? actionCallerArgumentExpression, [CallerFilePath] string callerSourceFilePath = "unknown path", [CallerMemberName] string callerMemberName = "unknown member")
    {
        const string IndentSpace = "  ";

        var builder = new StringBuilder();

        var variableExpressionPart = toVariableExpressionPart(actionCallerArgumentExpression);

        if (fail is IShouldMethodAssertionException)
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine}{variableExpressionPart} failed the verification.");
            builder.AppendLine("");

            appendWithIndent(builder, fail.Message);

            var message = builder.ToString();

            if (fail.StackTrace is not null)
            {
                builder.Clear();

                var lines = LineFeedRegex().Split(fail.StackTrace);

                foreach (var stackFrame in lines)
                {
                    if (stackFrame.Contains(callerSourceFilePath, StringComparison.OrdinalIgnoreCase) && stackFrame.Contains(callerMemberName, StringComparison.Ordinal))
                        break;

                    builder.AppendLine(stackFrame);
                }

                var stackFrames = new StackTrace(skipFrames: 2, fNeedFileInfo: true).GetFrames().Where(v => v.GetFileName() is not null && v.GetMethod() is not null);

#if NET8_0_OR_GREATER
                return Create(message, null, $"{builder}{new StackTrace(stackFrames)}");
#else
                foreach (var stackFrame in stackFrames)
                    builder.AppendLine(new StackTrace(stackFrame).ToString());

                return Create(message, null, builder.ToString());
#endif
            }
            else
            {
                return Create(message);
            }
        }
        else
        {
            builder.AppendLine(CultureInfo.InvariantCulture, $"An exception occurred while verifying {actualExpression.OneLine}{variableExpressionPart}.");
            builder.AppendLine("");

            appendWithIndent(builder, $"{fail.GetType().FullName}: {fail.Message}");

            return Create(builder.ToString(), fail);
        }



        static void appendWithIndent(StringBuilder builder, string text)
        {
            var lines = LineFeedRegex().Split(text);
            foreach (var line in lines)
            {
                builder.Append(IndentSpace);
                builder.AppendLine(line);
            }
        }

        static string? toVariableExpressionPart(string? actionCallerArgumentExpression)
        {
            if (actionCallerArgumentExpression is null)
                return null;

            var match = Regex.Match(actionCallerArgumentExpression, @"\A\s*(((?<ArgName1>[\w_]+)|\(([\w_]+\s+)(?<ArgName2>[\w_]+)\))\s*=>|(?<ArgName3>\w+)_[\w_]+\s*\z)");

            if (!match.Success)
                return null;

            if (match.Groups["ArgName1"].Value is { Length: > 0 } argName1)
                return $"(⇒ `{argName1}`)";

            if (match.Groups["ArgName2"].Value is { Length: > 0 } argName2)
                return $"(⇒ `{argName2}`)";

            if (match.Groups["ArgName3"].Value is { Length: > 0 } argName3)
                return $"(⇒ `{argName3}`)";

            return null;
        }
    }


    internal static Exception CreateBasicShouldAllSatisfyFail<TKey, TValue>(List<(TKey key, TValue value, Exception exception)> fails, ValueExpression actualExpression, [CallerFilePath] string callerSourceFilePath = "unknown path")
    {
        const string IndentSpace = "  ";

        var builder = new StringBuilder();

        builder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} has unsatisfied elements.");
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

    public static Exception Create(string message) => _exceptionFactory.Value.Create(message, exception: null, stackTrace: null);

    public static Exception Create(string message, Exception? exception = null, string? stackTrace = null) =>  _exceptionFactory.Value.Create(message, exception, stackTrace);

}
