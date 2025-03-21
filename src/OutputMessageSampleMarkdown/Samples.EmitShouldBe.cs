using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldBe(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldBe");

        writer.WriteLine($"## Should().Be()");

        writer.WriteLine($"### int");

        writer.EmitMessageSample(() =>
        {
            var actualValue = 1;
            var expectedValue = 2;

            actualValue.Should().Be(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = 1;

            actualValue.Should().Be(2);
        });

        writer.WriteLine($"### string");

        writer.EmitMessageSample(() =>
        {
            var actualValue = "apple";
            var expectedValue = "banana";

            actualValue.Should().Be(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = "apple";

            actualValue.Should().Be("banana");
        });

        writer.WriteLine($"### Guid");

        writer.EmitMessageSample(() =>
        {
            var actualValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");
            var expectedValue = Guid.Parse("2968a94a-febf-4939-b90e-c763a9fefba4");

            actualValue.Should().Be(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");

            actualValue.Should().Be(Guid.Parse("2968a94a-febf-4939-b90e-c763a9fefba4"));
        });

        writer.WriteLine($"## Should().NotBe()");

        writer.WriteLine($"### int");

        writer.EmitMessageSample(() =>
        {
            var actualValue = 1;
            var expectedValue = 1;

            actualValue.Should().NotBe(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = 1;

            actualValue.Should().NotBe(1);
        });

        writer.WriteLine($"### string");

        writer.EmitMessageSample(() =>
        {
            var actualValue = "apple";
            var expectedValue = "apple";

            actualValue.Should().NotBe(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = "apple";

            actualValue.Should().NotBe("apple");
        });

        writer.WriteLine($"### Guid");

        writer.EmitMessageSample(() =>
        {
            var actualValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");
            var expectedValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");

            actualValue.Should().NotBe(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");

            actualValue.Should().NotBe(Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b"));
        });
    }
}
