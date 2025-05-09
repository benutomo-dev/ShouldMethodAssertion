using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;
using System.Diagnostics;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(string))]
public partial struct StringShouldMatch
{
    public const char DefaultSingleMatchChar = '?';
    public const char DefaultMultipleMatchChar = '*';

    public void ShouldMatch(string text, bool ignoreCase = false, char singleMatchChar = DefaultSingleMatchChar, char multipleMatchWildcardChar = DefaultMultipleMatchChar)
    {
        ShouldMatch(text.AsSpan(), ignoreCase, singleMatchChar, multipleMatchWildcardChar);
    }

    public void ShouldMatch(ReadOnlySpan<char> text, bool ignoreCase = false, char singleMatchChar = DefaultSingleMatchChar, char multipleMatchWildcardChar = DefaultMultipleMatchChar)
    {
        if (IsMatch(Actual.AsSpan(), text, ignoreCase, singleMatchWildcardChar: '?', multipleMatchWildcardChar: '*'))
            return;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} is not match to expected.

            [Actual]
            {Actual}

            [Expected]
            {text}
            """);
    }

    public void ShouldNotMatch(string text, bool ignoreCase = false, char singleMatchChar = DefaultSingleMatchChar, char multipleMatchWildcardChar = DefaultMultipleMatchChar)
    {
        ShouldNotMatch(text.AsSpan(), ignoreCase, singleMatchChar, multipleMatchWildcardChar);
    }

    public void ShouldNotMatch(ReadOnlySpan<char> text, bool ignoreCase = false, char singleMatchChar = DefaultSingleMatchChar, char multipleMatchWildcardChar = DefaultMultipleMatchChar)
    {
        if (!IsMatch(Actual.AsSpan(), text, ignoreCase, singleMatchWildcardChar: '?', multipleMatchWildcardChar: '*'))
            return;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} is match to expected.

            [Actual]
            {Actual}

            [Expected]
            {text}
            """);
    }

    private static bool IsMatch(ReadOnlySpan<char> actual, ReadOnlySpan<char> expected, bool ignoreCase, char singleMatchWildcardChar, char multipleMatchWildcardChar)
    {
        if (expected.IsEmpty)
            return actual.IsEmpty;

        if (expected[0] == multipleMatchWildcardChar)
        {
            // ワイルドカードから判定を開始
            return IsMatchWildcardStartPart(actual, expected, ignoreCase, singleMatchWildcardChar, multipleMatchWildcardChar);
        }
        else
        {
            // 前方一致で判定を開始
            var expectedLeadingText = FetchTextPart(ref expected, multipleMatchWildcardChar);
            return IsMatchTextStartPart(actual, expectedLeadingText, expected, ignoreCase, singleMatchWildcardChar, multipleMatchWildcardChar);
        }
    }

    private static bool IsMatchWildcardStartPart(ReadOnlySpan<char> actual, ReadOnlySpan<char> expectedNextWildcardStartText, bool ignoreCase, char singleMatchWildcardChar, char multipleMatchWildcardChar)
    {
        var wildcardPart = FetchWildcardPart(ref expectedNextWildcardStartText, multipleMatchWildcardChar);

        if (expectedNextWildcardStartText.IsEmpty)
        {
            // 期待文字列が"*"または"**"の場合は基本的に任意の文字列に対して一致となる

            if (wildcardPart.Length == 1 && actual.IndexOf('\n') >= 0)
            {
                // "*"の場合は対象文字列に改行コードが含まれない場合のみOK
                return false;
            }
            
            return true;
        }

        int lastIndex = actual.Length;
        if (wildcardPart.Length == 1 && actual.IndexOf('\n') is >= 0 and { } lineFeedIndex)
        {
            lastIndex = lineFeedIndex;
        }

        var nextTextPart = FetchTextPart(ref expectedNextWildcardStartText, multipleMatchWildcardChar);

        for (int i = 0; i < lastIndex - nextTextPart.Length + 1; i++)
        {
            if (IsMatchTextStartPart(actual.Slice(i), nextTextPart, expectedNextWildcardStartText, ignoreCase, singleMatchWildcardChar, multipleMatchWildcardChar))
                return true;
        }

        return false;
    }

    private static bool IsMatchTextStartPart(ReadOnlySpan<char> actual, ReadOnlySpan<char> expectedLeadingText, ReadOnlySpan<char> expectedNextWildcardStartText, bool ignoreCase, char singleMatchWildcardChar, char multipleMatchWildcardChar)
    {
        Debug.Assert(expectedLeadingText.Length > 0);
        Debug.Assert(expectedLeadingText[0] != multipleMatchWildcardChar);

        var leadingTextMatchResult = startWithExpectedLeadingText(ref actual, expectedLeadingText, ignoreCase, singleMatchWildcardChar, multipleMatchWildcardChar);

        if (!leadingTextMatchResult)
            return false;

        if (expectedNextWildcardStartText.IsEmpty)
            return actual.IsEmpty;

        Debug.Assert(expectedNextWildcardStartText[0] == multipleMatchWildcardChar);

        return IsMatchWildcardStartPart(actual, expectedNextWildcardStartText, ignoreCase, singleMatchWildcardChar, multipleMatchWildcardChar);

        static bool startWithExpectedLeadingText(scoped ref ReadOnlySpan<char> actual, ReadOnlySpan<char> expectedLeadingText, bool ignoreCase, char singleMatchWildcardChar, char multipleMatchWildcardChar)
        {
            int actualIndex = 0;
            int expectedIndex = 0;

            while (expectedIndex < expectedLeadingText.Length)
            {
                if (actual.Length <= actualIndex)
                    return false;

                // "\r\n"の'\r'は無視する
                if (isCrLf(actual.Slice(actualIndex)))
                    actualIndex += 1;

                if (expectedLeadingText[expectedIndex] != singleMatchWildcardChar)
                {
                    if (isCrLf(expectedLeadingText.Slice(expectedIndex)))
                        expectedIndex += 1;

                    if (ignoreCase)
                    {
                        if (char.ToLowerInvariant(actual[actualIndex]) != char.ToLowerInvariant(expectedLeadingText[expectedIndex]))
                            return false;
                    }
                    else
                    {
                        if (actual[actualIndex] != expectedLeadingText[expectedIndex])
                            return false;
                    }
                }

                actualIndex += 1;
                expectedIndex += 1;
            }

            actual = actual.Slice(actualIndex);

            return true;
        }

        static bool isCrLf(ReadOnlySpan<char> text)
        {
            if (text[0] != '\r')
                return false;

            if (text.Length <= 1)
                return false;

            if (text[1] != '\n')
                return false;

            return true;
        }
    }

    private static ReadOnlySpan<char> FetchWildcardPart(scoped ref ReadOnlySpan<char> text, char wildcardChar)
    {
        if (text.IsEmpty)
            return ReadOnlySpan<char>.Empty;

        ReadOnlySpan<char> fetchText;

        for (int i = 1; i < text.Length; i++)
        {
            if (text[i] != wildcardChar)
            {
                fetchText = text.Slice(0, i);

                text = text.Slice(i);

                return fetchText;
            }
        }

        fetchText = text;
        text = text.Slice(text.Length);
        return fetchText;
    }

    private static ReadOnlySpan<char> FetchTextPart(scoped ref ReadOnlySpan<char> text, char wildcardChar)
    {
        if (text.IsEmpty)
            return ReadOnlySpan<char>.Empty;

        ReadOnlySpan<char> fetchText;

        var wildcardCharIndex = text.IndexOf(wildcardChar);

        if (wildcardCharIndex >= 0)
        {
            fetchText = text.Slice(0, wildcardCharIndex);

            text = text.Slice(wildcardCharIndex);

            return fetchText;
        }

        fetchText = text;
        text = text.Slice(text.Length);
        return fetchText;
    }
}
