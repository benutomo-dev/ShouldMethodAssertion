using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldHaveCount(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldHaveCount");

        writer.WriteLine($"## Should().HaveCount()");

        writer.WriteLine($"### string[]");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().HaveCount(1);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue>");

        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().HaveCount(1);
        });

        writer.WriteLine($"## Should().NotHaveCount()");

        writer.WriteLine($"### string[]");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotHaveCount(3);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue>");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().NotHaveCount(3);
        });
    }
}
