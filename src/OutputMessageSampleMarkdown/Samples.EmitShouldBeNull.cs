using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldBeNull(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldBeNull");

        writer.WriteLine($"## Should().BeNull()");

        writer.WriteLine($"### class");
        writer.EmitMessageSample(() =>
        {
            var actualValue = "apple";

            actualValue.Should().BeNull();
        });

        writer.WriteLine($"## Should().NotBeNull()");

        writer.WriteLine($"### class");
        writer.EmitMessageSample(() =>
        {
            var actualValue = default(string);

            actualValue.Should().NotBeNull();
        });
    }
}
