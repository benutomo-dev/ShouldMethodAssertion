using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace ShouldMethodAssertion.ExpressionUtils;

public static partial class ExpressionUtil
{
    [GeneratedRegex(@"\A(@$""|$@""|[@$]""|$*""{2,}|[\[(`""])")]
    private static partial Regex HasBracketsRegex();


    [GeneratedRegex(@"\r?\n")]
    private static partial Regex LineSeparatorRegex();

    [GeneratedRegex(@"\s*\r?\n\s*")]
    private static partial Regex LineSeparatorWithBeforeAndAfterWhiteSpaceRegex();

    [return: NotNullIfNotNull(nameof(expression))]
    public static string? AdjustExpressionIndent(string? expression, bool withComplementBruckets)
    {
        if (expression is null)
            return null;

        if (!expression.Contains('\n'))
        {
            if (withComplementBruckets && !HasBracketsExpression(expression))
                return $"`{expression}`";
            else
                return expression;
        }

        var lines = LineSeparatorRegex().Split(expression);

        if (lines.Length == 2)
        {
            if (withComplementBruckets && !HasBracketsExpression(expression))
            {
                return $"""
                    `{lines[0]}
                    {lines[1].TrimStart()}`
                    """;
            }
            else
            {
                return $"""
                    {lines[0]}
                    {lines[1].TrimStart()}
                    """;
            }
        }

        int trimCharCount = 0;
        while (true)
        {
            if (lines[1].Length <= trimCharCount)
                goto BREAK_TRIM_CHAR_COUNT_LOOP;

            var ch = lines[1][trimCharCount];

            for (int i = 2; i < lines.Length; i++)
            {
                if (lines[i].Length <= trimCharCount)
                    goto BREAK_TRIM_CHAR_COUNT_LOOP;

                var otherCh = lines[i][trimCharCount];

                if (otherCh != ch || !char.IsWhiteSpace(otherCh))
                    goto BREAK_TRIM_CHAR_COUNT_LOOP;
            }

            trimCharCount++;
        }
    BREAK_TRIM_CHAR_COUNT_LOOP:

        if (trimCharCount == 0)
            return expression;

        var putBruckets = withComplementBruckets && !HasBracketsExpression(expression);

        var fixedExpressionBuilder = new StringBuilder(putBruckets ? expression.Length + 2 : expression.Length); 

        if (putBruckets)
            fixedExpressionBuilder.Append('`');

        fixedExpressionBuilder.Append(lines[0]);
        for (int i = 1; i < lines.Length; i++)
        {
            fixedExpressionBuilder.AppendLine();
            fixedExpressionBuilder.Append(lines[i].AsSpan(trimCharCount));
        }

        if (putBruckets)
            fixedExpressionBuilder.Append('`');

        return fixedExpressionBuilder.ToString();
    }

    [return: NotNullIfNotNull(nameof(expression))]
    public static bool HasBracketsExpression(string? expression)
    {
        if (expression is null)
            return false;

        return HasBracketsRegex().IsMatch(expression);
    }

    [return: NotNullIfNotNull(nameof(expression))]
    public static string? ToOneLineExpression(string? expression, bool withComplementBruckets)
    {
        if (expression is null)
            return null;

        if (!expression.Contains('\n'))
        {
            if (withComplementBruckets && !HasBracketsExpression(expression))
                return $"`{expression}`";
            else
                return expression;
        }

        if (withComplementBruckets && !HasBracketsExpression(expression))
            return $"`{LineSeparatorWithBeforeAndAfterWhiteSpaceRegex().Replace(expression, _ => "")}`";
        else
            return LineSeparatorWithBeforeAndAfterWhiteSpaceRegex().Replace(expression, _ => "");
    }

    public static string ToOneLineValueString<TValue>(TValue? value)
    {
        const int maxLength = 80;
        const string ellipse = " ...";

        if (value is null)
            return "null";

        var sourceValueText = value?.ToString() ?? "";

        if (sourceValueText.Length <= maxLength && !sourceValueText.Any(needReplacementChar))
            return sourceValueText;

        var capacity = Math.Min(sourceValueText.Length, maxLength);
        var oneLineValueTextBuilder = new StringBuilder(capacity);
        //oneLineValueTextBuilder.Append('`');

        foreach (var ch in sourceValueText)
        {
            if (oneLineValueTextBuilder.Length >= maxLength - ellipse.Length)
            {
                oneLineValueTextBuilder.Append(ellipse);
                break;
            }

            if (needReplacementChar(ch))
            {
                oneLineValueTextBuilder.Append(' ');
                continue;
            }

            oneLineValueTextBuilder.Append(ch);
        }

        //oneLineValueTextBuilder.Append('`');

        Debug.Assert(oneLineValueTextBuilder.Length <= capacity);

        return oneLineValueTextBuilder.ToString();

        static bool needReplacementChar(char ch) => ch is '\r' or '\n' or '\t';
    }
}
