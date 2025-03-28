using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(string))]
public partial struct StringShouldContain
{
    public void ShouldContain(ReadOnlySpan<char> text, int containedCounts = -1, bool ignoreCase = false)
    {
        if (containedCounts == 0)
        {
            ShouldNotContain(text, ignoreCase);
            return;
        }

        var ignoreCaseAnnotation = ignoreCase
            ? ", with case ignored"
            : "";

        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        if (containedCounts < 0)
        {
            if (!Actual.AsSpan().Contains(text, stringComparison))
                throw AssertExceptionUtil.Create($"""
                    {ActualExpression.OneLine} is not contain {ExpressionUtil.FormartValue(text)}{ignoreCaseAnnotation}.

                    [Actual]
                    {ExpressionUtil.FormartValue(Actual)}
                    """);

            return;
        }

        var actualCount = GetActualContainedCount(text, stringComparison);

        if (actualCount != containedCounts)
        {
            if (actualCount == 0)
                throw AssertExceptionUtil.Create($"""
                    {ActualExpression.OneLine} is not contain {ExpressionUtil.FormartValue(text)}.
                    
                    [Actual]
                    {ExpressionUtil.FormartValue(Actual)}
                    """);

            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is contain {actualCount} {ExpressionUtil.FormartValue(text)}{ignoreCaseAnnotation}. But expected count is {containedCounts}.
                
                [Actual]
                {ExpressionUtil.FormartValue(Actual)}
                """);
        }
    }

    public void ShouldContain(string text, int containedCounts = -1, bool ignoreCase = false)
    {
        ShouldContain(text.AsSpan(), containedCounts, ignoreCase);
    }

    public void ShouldContain(ReadOnlySpan<char> text, Range containedCountsRange, bool ignoreCase = false)
    {
        if (containedCountsRange.Start.IsFromEnd)
            throw new ArgumentOutOfRangeException(nameof(containedCountsRange));

        if (containedCountsRange.End.IsFromEnd)
            throw new ArgumentOutOfRangeException(nameof(containedCountsRange));

        if (containedCountsRange.Start.Value < 0)
            throw new ArgumentOutOfRangeException(nameof(containedCountsRange));

        if (containedCountsRange.End.Value < 0)
            throw new ArgumentOutOfRangeException(nameof(containedCountsRange));

        if (containedCountsRange.End.Value < containedCountsRange.Start.Value)
            throw new ArgumentOutOfRangeException(nameof(containedCountsRange));

        if (containedCountsRange.Start.Value == 0 && containedCountsRange.End.Value == 0)
        {
            ShouldNotContain(text, ignoreCase);
            return;
        }

        if (containedCountsRange.Start.Value == containedCountsRange.End.Value)
        {
            ShouldContain(text, containedCountsRange.Start.Value, ignoreCase);
            return;
        }

        var ignoreCaseAnnotation = ignoreCase
            ? ", with case ignored"
            : "";

        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        var actualCount = GetActualContainedCount(text, stringComparison);

        if (actualCount < containedCountsRange.Start.Value || containedCountsRange.End.Value < actualCount)
        {
            if (actualCount == 0)
                throw AssertExceptionUtil.Create($"""
                    {ActualExpression.OneLine} is not contain {ExpressionUtil.FormartValue(text)}{ignoreCaseAnnotation}.
                    
                    [Actual]
                    {ExpressionUtil.FormartValue(Actual)}
                    """);

            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is contain {actualCount} {ExpressionUtil.FormartValue(text)}{ignoreCaseAnnotation}. But expected count is in range of {containedCountsRange.Start.Value}～{containedCountsRange.End.Value}.
                
                [Actual]
                {ExpressionUtil.FormartValue(Actual)}
                """);
        }
    }

    public void ShouldContain(string text, Range containedCountsRange, bool ignoreCase = false)
    {
        ShouldContain(text.AsSpan(), containedCountsRange, ignoreCase);
    }


    public void ShouldNotContain(ReadOnlySpan<char> text, bool ignoreCase = false)
    {
        var ignoreCaseAnnotation = ignoreCase
            ? ", with case ignored"
            : "";

        var stringComparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        var actualCount = GetActualContainedCount(text, stringComparison);

        if (actualCount != 0)
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is contain {actualCount} {ExpressionUtil.FormartValue(text)}{ignoreCaseAnnotation}. But expected count is 0.
                
                [Actual]
                {ExpressionUtil.FormartValue(Actual)}
                """);
    }

    public void ShouldNotContain(string text, bool ignoreCase = false)
    {
        ShouldNotContain(text.AsSpan(), ignoreCase);
    }

    private int GetActualContainedCount(ReadOnlySpan<char> text, StringComparison stringComparison)
    {
        int startIndex = 0;
        int actualCount = 0;

        while (startIndex + text.Length <= Actual.Length)
        {
            var index = Actual.AsSpan(startIndex).IndexOf(text, stringComparison);

            if (index < 0)
                break;

            actualCount++;

            startIndex += index + text.Length;
        }

        return actualCount;
    }
}
