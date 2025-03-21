using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldEqual(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldEqual");

        writer.WriteLine($"## Should().Equal()");

        writer.WriteLine($"### string[] / Not equal by value");

        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = (string[])["apple", "banana", "grape"];

            actualValue.Should().Equal(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().Equal(["apple", "banana", "grape"]);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().Equal([
                "apple",
                "banana",
                "grape",
                ]);
        });

        writer.WriteLine($"### string[] / Not equal by count");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = (string[])["apple", "banana"];

            actualValue.Should().Equal(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().Equal(["apple", "banana"]);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().Equal([
                "apple",
                "banana",
                ]);
        });

        writer.WriteLine($"### string[] / Not equal without the order");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = (string[])["banana", "apple", "banana", "grape"];

            actualValue.Should().Equal(expectedValue, ignoreOrder: true);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().Equal([
                "A",
                "C",
                "D",
                "E",
                "F",
                "G",
                "H",
                "I",
                "J",
                "K",
                "L",
                "M",
            ], ignoreOrder: true);
        });

        writer.WriteLine($"### string[] / Not equal with comparer");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = (string[])["Apple", "Banana", "Grape"];

            actualValue.Should().Equal(expectedValue, comparer: StringComparer.OrdinalIgnoreCase);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().Equal(["Apple", "Banana", "Grape"], comparer: StringComparer.OrdinalIgnoreCase);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Not equal by value");
        
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };
            var expectedValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 2 } };

            actualValue.Should().Equal(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };

            actualValue.Should().Equal([("apple", 1), ("banana", 1), ("orange", 2)]);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Not equal by key");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };
            var expectedValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "grape", 1 } };

            actualValue.Should().Equal(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };

            actualValue.Should().Equal([("apple", 1), ("banana", 1), ("grape", 1)]);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Not equal by count");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };
            var expectedValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 } };

            actualValue.Should().Equal(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };

            actualValue.Should().Equal([("apple", 1), ("banana", 1)]);
        });

        writer.WriteLine($"## Should().NotEqual()");

        writer.WriteLine($"### string[] / Equal by defualt");

        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotEqual(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotEqual(["apple", "banana", "orange"]);
        });

        writer.WriteLine($"### string[] / Equal ignore order");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotEqual(expectedValue, ignoreOrder: true);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotEqual(["apple", "banana", "orange"], ignoreOrder: true);
        });

        writer.WriteLine($"### string[] / Equal with comparer");
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];
            var expectedValue = (string[])["Apple", "Banana", "Orange"];

            actualValue.Should().NotEqual(expectedValue, comparer: StringComparer.OrdinalIgnoreCase);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = (string[])["apple", "banana", "orange"];

            actualValue.Should().NotEqual(["Apple", "Banana", "Orange"], comparer: StringComparer.OrdinalIgnoreCase);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Equal by defualt");

        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 2 } };
            var expectedValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 2 } };

            actualValue.Should().NotEqual(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 2 } };

            actualValue.Should().NotEqual([("apple", 1), ("banana", 1), ("orange", 2)]);
        });

        writer.WriteLine($"### Dictionary<TKey, TValue> / Equal with value comparer");
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
            var expectedValue = new Dictionary<int, string> { { 1, "Apple" }, { 2, "Banana" }, { 3, "Orange" } };

            actualValue.Should().NotEqual(expectedValue, valueComparer: StringComparer.OrdinalIgnoreCase);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

            actualValue.Should().NotEqual([(1, "Apple"), (2, "Banana"), (3, "Orange")], valueComparer: StringComparer.OrdinalIgnoreCase);
        });
    }
}
