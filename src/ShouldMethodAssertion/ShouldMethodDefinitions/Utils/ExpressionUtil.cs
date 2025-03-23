using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

public static partial class ExpressionUtil
{
    private const string HasBracketsRegexValue = /* language=regex */ @"\A(@$""|$@""|[@$]""|$*""{2,}|[\[(`""])";

    private const string LineSeparatorRegexValue = /* language=regex */ @"\r?\n";

    private const string LineSeparatorWithBeforeAndAfterWhiteSpaceRegexValue = /* language=regex */ @"\s*\r?\n\s*";

#if NET8_0_OR_GREATER
    [GeneratedRegex(HasBracketsRegexValue)]
    private static partial Regex HasBracketsRegex();


    [GeneratedRegex(LineSeparatorRegexValue)]
    private static partial Regex LineSeparatorRegex();

    [GeneratedRegex(LineSeparatorWithBeforeAndAfterWhiteSpaceRegexValue)]
    private static partial Regex LineSeparatorWithBeforeAndAfterWhiteSpaceRegex();
#else
    private static Regex HasBracketsRegex() => _hasBracketsRegex;
    private static Regex _hasBracketsRegex = new Regex(HasBracketsRegexValue, RegexOptions.Compiled);


    private static Regex LineSeparatorRegex() => _lineSeparatorRegex;
    private static Regex _lineSeparatorRegex = new Regex(LineSeparatorRegexValue, RegexOptions.Compiled);

    private static Regex LineSeparatorWithBeforeAndAfterWhiteSpaceRegex() => _lineSeparatorWithBeforeAndAfterWhiteSpaceRegex;
    private static Regex _lineSeparatorWithBeforeAndAfterWhiteSpaceRegex = new Regex(LineSeparatorWithBeforeAndAfterWhiteSpaceRegexValue, RegexOptions.Compiled);
#endif

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

    internal static string FormartValue<TValue>(TValue value)
    {
        return value switch
        {
            null => "null",
            string { Length: 0 } => "",
            string => $"\"{value}\"",
            Type type => GetOutputFullName(type),
            _ => $"`{value}`"
        };
    }

    internal static string FormatValueAsOneline<TValue>(TValue? value, int maxLength = 160)
    {
        const string ellipse = " ...";

        if (value is null)
            return "null";

        if (value is Type type)
            return GetOutputFullName(type);

        var sourceValueText = value.ToString() ?? "";

        if (sourceValueText.Length <= maxLength && !sourceValueText.Any(needReplacementChar))
        {
            if (value is string)
                return $"\"{sourceValueText}\"";
            else
                return sourceValueText;
        }

        var capacity = Math.Min(sourceValueText.Length, maxLength + 20);
        var oneLineValueTextBuilder = new StringBuilder(capacity);

        if (value is string)
            oneLineValueTextBuilder.Append('"');

        bool prevCharWasReplacementChar = false;
        foreach (var ch in sourceValueText)
        {
            if (oneLineValueTextBuilder.Length >= maxLength - ellipse.Length)
            {
                oneLineValueTextBuilder.Append(ellipse);
                oneLineValueTextBuilder.Append(CultureInfo.InvariantCulture, $"(totallength: {sourceValueText.Length})");
                break;
            }

            if (needReplacementChar(ch))
            {
                if (!prevCharWasReplacementChar)
                    oneLineValueTextBuilder.Append(' ');

                prevCharWasReplacementChar = true;
                continue;
            }
            prevCharWasReplacementChar = false;

            oneLineValueTextBuilder.Append(ch);
        }

        if (value is string)
            oneLineValueTextBuilder.Append('"');

        Debug.Assert(oneLineValueTextBuilder.Length <= capacity);

        return oneLineValueTextBuilder.ToString();

        static bool needReplacementChar(char ch) => ch is '\r' or '\n' or '\t';
    }

    private static string GetOutputFullName(Type type)
    {
        var stringBuilder = new StringBuilder();

        append(stringBuilder, type);

        return stringBuilder.ToString();


        static void append(StringBuilder stringBuilder, Type type)
        {
            if (type.IsArray && type.GetElementType() is { } elementType)
            {
                var rank = type.GetArrayRank();

                append(stringBuilder, elementType);
                stringBuilder.Append('[');
                for (int i = 1; i < rank; i++)
                    stringBuilder.Append(',');
                stringBuilder.Append(']');
                return;
            }

            if (getKeywordType(type) is { } typeKeyword)
            {
                stringBuilder.Append(typeKeyword);
                return;
            }

            if (!string.IsNullOrEmpty(type.Namespace))
            {
                stringBuilder.Append(type.Namespace);
                stringBuilder.Append('.');
            }

            var index = type.Name.IndexOf('`');
            if (index >= 0)
                stringBuilder.Append(type.Name, 0, index);
            else
                stringBuilder.Append(type.Name);

            if (type.IsGenericType)
            {
                stringBuilder.Append('<');
                bool isFirstArg = true;
                foreach (var typeArg in type.GenericTypeArguments)
                {
                    if (!isFirstArg)
                        stringBuilder.Append(',');
                    isFirstArg = false;

                    append(stringBuilder, typeArg);
                }
                stringBuilder.Append('>');
            }
        }

        static string? getKeywordType(Type type)
        {
            if (type == typeof(char))
                return "char";
            else if (type == typeof(byte))
                return "byte";
            else if (type == typeof(sbyte))
                return "sbyte";
            else if (type == typeof(short))
                return "short";
            else if (type == typeof(int))
                return "int";
            else if (type == typeof(long))
                return "long";
            else if (type == typeof(ushort))
                return "ushort";
            else if (type == typeof(uint))
                return "uint";
            else if (type == typeof(ulong))
                return "ulong";
            else if (type == typeof(string))
                return "string";
            else if (type == typeof(bool))
                return "bool";
            else if (type == typeof(object))
                return "object";
            else
                return null;
        }
    }
}
