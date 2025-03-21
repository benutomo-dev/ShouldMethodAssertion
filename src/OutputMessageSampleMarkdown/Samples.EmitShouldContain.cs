using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldContain(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldContain");

        writer.WriteLine($"## Should().Contain()");

        writer.WriteLine($"### string[] / Default");

        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = "Apple";

            actualValue.Should().Contain(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().Contain("Apple");
        });

        writer.WriteLine($"### string[] / Not contain with comparer");

        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = "Grape";

            actualValue.Should().Contain(expectedValue, StringComparer.OrdinalIgnoreCase);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().Contain("Grape", StringComparer.OrdinalIgnoreCase);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Not contain by KeyValuePair<,>");

        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
            var expectedValue = new KeyValuePair<int, string>(1, "Apple");

            actualValue.Should().Contain(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().Contain(new KeyValuePair<int, string>(1, "Apple"));
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Not contain by ValueTuple");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
            var expectedValue = (1, "Apple");

            actualValue.Should().Contain(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().Contain((1, "Apple"));
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Not contain with value comparer");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple"}, { 2, "banana" }, { 3, "orange" } };
            var expectedValue = (3, "grape");

            actualValue.Should().Contain(expectedValue, valueComparer: StringComparer.OrdinalIgnoreCase);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().Contain((3, "grape"), valueComparer: StringComparer.OrdinalIgnoreCase);
        });

        writer.WriteLine($"## Should().NotContain()");

        writer.WriteLine($"### string[] / Defualt");

        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = "banana";

            actualValue.Should().NotContain(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotContain("banana");
        });

        writer.WriteLine($"### string[] / Contain with comparer");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = "Banana";

            actualValue.Should().NotContain(expectedValue, comparer: StringComparer.OrdinalIgnoreCase);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            
            actualValue.Should().NotContain("Banana", comparer: StringComparer.OrdinalIgnoreCase);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Contain by KeyValuePair<,>");

        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
            var expectedValue = new KeyValuePair<int, string>(1, "apple");

            actualValue.Should().NotContain(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().NotContain(new KeyValuePair<int, string>(1, "apple"));
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Contain by ValueTuple");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
            var expectedValue = (1, "apple");

            actualValue.Should().NotContain(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().NotContain((1, "apple"));
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Equal with value comparer");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
            var expectedValue = (1, "Apple");

            actualValue.Should().NotContain(expectedValue, valueComparer: StringComparer.OrdinalIgnoreCase);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().NotContain((1, "Apple"), valueComparer: StringComparer.OrdinalIgnoreCase);
        });
    }
}
