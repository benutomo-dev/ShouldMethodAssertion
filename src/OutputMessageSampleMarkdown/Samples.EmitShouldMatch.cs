using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldMatch(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldMatch");

        writer.WriteLine($"## Should().Match()");

        writer.EmitMessageSample(() =>
        {
            var actualValue = "hello world";
            var expectedPattern = "hello*earth";

            actualValue.Should().Match(expectedPattern);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = "hello world";

            actualValue.Should().Match("hello*earth");
        });

        writer.WriteLine($"## Should().NotMatch()");

        writer.EmitMessageSample(() =>
        {
            var actualValue = "hello world";
            var expectedPattern = "hello*";

            actualValue.Should().NotMatch(expectedPattern);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = "hello world";

            actualValue.Should().NotMatch("hello*");
        });
    }
}
