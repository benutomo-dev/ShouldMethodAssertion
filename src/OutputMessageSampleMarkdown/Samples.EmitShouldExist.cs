using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldExist(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldExist");

        writer.WriteLine($"## Should().Exist()");

        writer.EmitMessageSample(() =>
        {
            var actualValue = new FileInfo("path/to/nonexistent-file.txt");

            actualValue.Should().Exist();
        });

        writer.WriteLine($"## Should().NotExist()");

        writer.EmitMessageSample(tempDir =>
        {
            var tempFile = Path.Combine(tempDir, "testfile.bin");
            try
            {
                var actualValue = new FileInfo(tempFile);

                actualValue.Should().NotExist();
            }
            finally
            {
                File.Delete(tempFile);
            }
        });
    }
}
