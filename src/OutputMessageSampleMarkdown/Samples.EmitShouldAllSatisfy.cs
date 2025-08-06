using ShouldMethodAssertion.ShouldExtensions;
using System.Globalization;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldAllSatisfy(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldAllSatisfy");

        writer.WriteLine($"## Should().AllSatisfy()");

        writer.WriteLine($"### string[] / Fail by assertion");
        writer.EmitMessageSample(() =>
        {
            var actualValues = (string[])["apple", "banana", "orange", "grape"];

            actualValues.Should().AllSatisfy(v =>
            {
                v.Should().BeOneOf(["apple", "banana"]);
            });
        });

        writer.WriteLine($"### string[] / Fail by exception");
        writer.EmitMessageSample(() =>
        {
            var actualValues = (string[])["apple", "1", "banana", "2"];

            actualValues.Should().AllSatisfy(v =>
            {
                int.Parse(v, CultureInfo.InvariantCulture);
            });
        });

        writer.WriteLine($"### Dictionary<TKey, TValue>");
        writer.EmitMessageSample(() =>
        {
            var actualValues = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValues.Should().AllSatisfy((key, value) =>
            {
                value.Should().BeOneOf(["apple", "banana"]);
            });
        });
    }
}
