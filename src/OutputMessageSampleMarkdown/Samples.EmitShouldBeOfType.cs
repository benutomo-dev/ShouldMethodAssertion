using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldBeOfType(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldBeOfType");

        writer.WriteLine($"## Should().BeOfType()");
        writer.EmitMessageSample(() =>
        {
            IEnumerable<string> actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().BeOfType<IEnumerable<string>>();
        });
        writer.EmitMessageSample(() =>
        {
            IEnumerable<string> actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().BeOfType(typeof(IEnumerable<string>));
        });


        writer.WriteLine($"## Should().NotBeOfType()");
        writer.EmitMessageSample(() =>
        {
            IEnumerable<string> actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotBeOfType<string[]>();
        });
        writer.EmitMessageSample(() =>
        {
            IEnumerable<string> actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotBeOfType(typeof(string[]));
        });
    }
}
