using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldBeAssignableTo(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldBeAssignableTo");

        writer.WriteLine($"## Should().BeAssignableTo<T>()");

        writer.EmitMessageSample(() =>
        {
            object actualValue = "hello";

            actualValue.Should().BeAssignableTo<int>();
        });

        writer.WriteLine($"## Should().BeAssignableTo(Type)");

        writer.EmitMessageSample(() =>
        {
            object actualValue = "hello";
            var type = typeof(int);

            actualValue.Should().BeAssignableTo(type);
        });
        writer.EmitMessageSample(() =>
        {
            object actualValue = "hello";

            actualValue.Should().BeAssignableTo(typeof(int));
        });

        writer.WriteLine($"## Should().NotBeAssignableTo<T>()");

        writer.EmitMessageSample(() =>
        {
            object actualValue = "hello";

            actualValue.Should().NotBeAssignableTo<string>();
        });

        writer.WriteLine($"## Should().NotBeAssignableTo(Type)");

        writer.EmitMessageSample(() =>
        {
            object actualValue = "hello";
            var type = typeof(string);

            actualValue.Should().NotBeAssignableTo(type);
        });
        writer.EmitMessageSample(() =>
        {
            object actualValue = "hello";

            actualValue.Should().NotBeAssignableTo(typeof(string));
        });
    }
}
