using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldEmpty(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldEmpty");

        writer.WriteLine($"## Should().BeEmpty()");

        writer.WriteLine($"### string[] / Small number of elements");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().BeEmpty();
        });

        writer.WriteLine($"### string[] / Large number of elements");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange", "A", "B", "C", "D", "E", "F", "G", "H", "I"];

            actualValue.Should().BeEmpty();
        });


        writer.WriteLine($"### Dictionary<TKey, TValue> / Small number of elements");

        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().BeEmpty();
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Large number of elements");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, string> { { "apple", "APPLE" }, { "banana", "BANANA" }, { "orange", "ORANGE" }, { "a", "A" }, { "b", "B" }, { "c", "C" }, { "d", "D" }, { "e", "E" }, { "f", "F" }, { "g", "G" }, { "h", "H" }, { "i", "I" } };

            actualValue.Should().BeEmpty();
        });

        writer.WriteLine($"## Should().NotBeEmpty()");

        writer.WriteLine($"### string[]");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])[];

            actualValue.Should().NotBeEmpty();
        });

        writer.WriteLine($"### Dictionary<TKey, TValue>");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { };

            actualValue.Should().NotBeEmpty();
        });
    }
}
