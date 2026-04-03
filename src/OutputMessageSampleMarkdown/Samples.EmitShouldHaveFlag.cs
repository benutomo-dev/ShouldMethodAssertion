using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    [Flags]
    enum SampleFlags
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
    }

    public static void EmitShouldHaveFlag(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldHaveFlag");

        writer.WriteLine($"## Should().HaveFlag()");

        writer.EmitMessageSample(() =>
        {
            var actualValue = SampleFlags.Read;
            var expectedValue = SampleFlags.Write;

            actualValue.Should().HaveFlag(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = SampleFlags.Read;

            actualValue.Should().HaveFlag(SampleFlags.Write);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = SampleFlags.Read | SampleFlags.Write;
            var expectedValue = SampleFlags.Read | SampleFlags.Execute;

            actualValue.Should().HaveFlag(expectedValue);
        });

        writer.WriteLine($"## Should().NotHaveFlag()");

        writer.EmitMessageSample(() =>
        {
            var actualValue = SampleFlags.Read | SampleFlags.Write;
            var expectedValue = SampleFlags.Write;

            actualValue.Should().NotHaveFlag(expectedValue);
        });
        writer.EmitMessageSample(() =>
        {
            var actualValue = SampleFlags.Read | SampleFlags.Write;

            actualValue.Should().NotHaveFlag(SampleFlags.Write);
        });
    }
}
