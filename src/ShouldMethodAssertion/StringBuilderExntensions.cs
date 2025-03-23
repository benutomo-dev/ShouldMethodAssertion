namespace ShouldMethodAssertion;

internal static class StringBuilderExntensions
{
#if NETFRAMEWORK
    /// <remarks>
    /// Ignored <paramref name="formatProvider"/>
    /// </remarks>
    public static System.Text.StringBuilder Append(this System.Text.StringBuilder stringBuilder, IFormatProvider formatProvider, string text)
    {
        return stringBuilder.Append(text);
    }

    /// <remarks>
    /// Ignored <paramref name="formatProvider"/>
    /// </remarks>
    public static System.Text.StringBuilder AppendLine(this System.Text.StringBuilder stringBuilder, IFormatProvider formatProvider, string text)
    {
        return stringBuilder.AppendLine(text);
    }
#endif
}
