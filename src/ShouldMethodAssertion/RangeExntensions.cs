using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion;

internal static class RangeExntensions
{
    public static void ThrowIfInvalidAsCountRange(this Range range, [CallerArgumentExpression(nameof(range))] string? rangeExpression = null)
    {
        if (range.Start.IsFromEnd)
            throw new ArgumentOutOfRangeException(rangeExpression);

        if (range.End.IsFromEnd)
        {
            if (range.End.Value != 0)
                throw new ArgumentOutOfRangeException(rangeExpression);
        }
        else if (range.End.Value < range.Start.Value)
        {
            throw new ArgumentOutOfRangeException(rangeExpression);
        }
    }

    public static bool IsSingleValueRange(this Range range, out int value)
    {
        if (range.Start.Value == range.End.Value && !range.End.IsFromEnd)
        {
            value = range.Start.Value;
            return true;
        }

        value = ~0;
        return false;
    }


    public static bool IsInRange(this Range range, int value)
    {
        return (range.Start.Value <= value && value <= range.End.Value);
    }
}
