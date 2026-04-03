using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldSameContentTo(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldSameContentTo");

        writer.WriteLine($"## Should().SameContentTo() / File sizes do not match");

        writer.EmitMessageSample(tempDir =>
        {
            var file1 = Path.Combine(tempDir, "file1.txt");
            var file2 = Path.Combine(tempDir, "file2.txt");
            File.WriteAllText(file1, "hello");
            File.WriteAllText(file2, "hello world");

            var actualValue = new FileInfo(file1);
            var otherFile = new FileInfo(file2);

            actualValue.Should().SameContentTo(otherFile);
        });

        writer.WriteLine($"## Should().SameContentTo() / File contents do not match");

        writer.EmitMessageSample(tempDir =>
        {
            var file1 = Path.Combine(tempDir, "file1.txt");
            var file2 = Path.Combine(tempDir, "file2.txt");
            File.WriteAllText(file1, "hello");
            File.WriteAllText(file2, "world");

            var actualValue = new FileInfo(file1);
            var otherFile = new FileInfo(file2);

            actualValue.Should().SameContentTo(otherFile);
        });

        writer.WriteLine($"## Should().NotSameContentTo()");

        writer.EmitMessageSample(tempDir =>
        {
            var file1 = Path.Combine(tempDir, "file1.txt");
            var file2 = Path.Combine(tempDir, "file2.txt");
            File.WriteAllText(file1, "hello");
            File.WriteAllText(file2, "hello");

            var actualValue = new FileInfo(file1);
            var otherFile = new FileInfo(file2);

            actualValue.Should().NotSameContentTo(otherFile);
        });
    }
}
