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

        writer.WriteLine($"### struct (System.Nullable&lt;T&gt;)");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (int?)1;

            actualValue.Should().BeNull();
        });

        writer.WriteLine($"## Should().NotBeNull()");

        writer.WriteLine($"### class");
        writer.EmitMessageSample(() =>
        {
            var actualValue = default(string);

            actualValue.Should().NotBeNull();
        });

        writer.WriteLine($"### struct (System.Nullable&lt;T&gt;)");
        writer.EmitMessageSample(() =>
        {
            var actualValue = default(int?);

            actualValue.Should().NotBeNull();
        });
    }
}
