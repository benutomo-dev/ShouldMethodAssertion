using ShouldMethodAssertion.ShouldExtensions;
using System.Globalization;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldSatisfy(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldSatisfy");

        writer.WriteLine($"## ShouldSatisfy() / Fail by assertion");
        writer.EmitMessageSample(() =>
        {
            var actual = "hello";

            actual.ShouldSatisfy(v =>
            {
                v.Should().Be("world");
            });
        });

        writer.WriteLine($"## ShouldSatisfy() / Fail by exception");
        writer.EmitMessageSample(() =>
        {
            var actual = "not a number";

            actual.ShouldSatisfy(v =>
            {
                int.Parse(v, CultureInfo.InvariantCulture);
            });
        });

        writer.WriteLine($"## ShouldSatisfy() / Nested ShouldSatisfy");
        writer.EmitMessageSample(() =>
        {
            var actual = (Name: "Alice", Age: 15);

            actual.ShouldSatisfy(v =>
            {
                v.Name.ShouldSatisfy(name =>
                {
                    name.Should().Be("Bob");
                });
            });
        });
    }
}
