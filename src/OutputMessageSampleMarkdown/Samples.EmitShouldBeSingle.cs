using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldBeSingle(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldBeSingle");

        writer.WriteLine($"## Should().BeSingle()");

        writer.WriteLine($"### string[] / Empty");
        writer.EmitMessageSample(() =>
        {
            var actualValues = (string[])[];

            actualValues.Should().BeSingle();
        });

        writer.WriteLine($"### string[] / Two or more elements");
        writer.EmitMessageSample(() =>
        {
            var actualValues = (string[])["apple", "banana" ];

            actualValues.Should().BeSingle();
        });


        writer.WriteLine($"### Dictionary<TKey, TValue> / Empty");

        writer.EmitMessageSample(() =>
        {
            var actualValues = new Dictionary<int, string>();

            actualValues.Should().BeSingle();
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Two or more elements");
        writer.EmitMessageSample(() =>
        {
            var actualValues = new Dictionary<string, string> { { "apple", "APPLE" }, { "banana", "BANANA" } };

            actualValues.Should().BeSingle();
        });

        writer.WriteLine($"### string[] / Use result");
        writer.EmitSuccessSample(() =>
        {
            var actualValues = (string[])["apple"];

            var singleValue = actualValues.Should().BeSingle().Result;

            singleValue.Should().Be("apple");
        });


        writer.WriteLine($"## Should().NotBeSingle()");

        writer.WriteLine($"### string[]");
        writer.EmitMessageSample(() =>
        {
            var actualValues = (string[])["apple"];

            actualValues.Should().NotBeSingle();
        });

        writer.WriteLine($"### Dictionary<TKey, TValue>");
        writer.EmitMessageSample(() =>
        {
            var actualValues = new Dictionary<int, string> { {1, "apple" } };

            actualValues.Should().NotBeSingle();
        });
    }
}
