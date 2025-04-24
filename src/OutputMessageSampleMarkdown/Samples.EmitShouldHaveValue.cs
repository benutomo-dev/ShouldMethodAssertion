using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldHaveValue(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldHaveValue");

        writer.WriteLine($"## Should().HaveValue()");

        writer.WriteLine($"### struct (System.Nullable&lt;T&gt;)");
        writer.EmitMessageSample(() =>
        {
            var actualValue = default(int?);

            actualValue.Should().HaveValue();
        });

        writer.WriteLine($"## Should().NotHaveValue()");

        writer.WriteLine($"### struct (System.Nullable&lt;T&gt;)");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (int?)1;

            actualValue.Should().NotHaveValue();
        });
    }
}
