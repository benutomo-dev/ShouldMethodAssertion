using System.Text;

namespace ShouldMethodAssertion.Tests;

public sealed class TempFile : IDisposable
{
    public string Path { get; }

    public TempFile()
        : this([])
    {
    }


    public TempFile(string content)
        : this(Encoding.UTF8.GetBytes(content))
    {
    }

    public TempFile(byte[] content)
    {
        Path = System.IO.Path.GetTempFileName();

        if (content.Length == 0)
            return;

        File.WriteAllBytes(Path, content);
    }

    public static implicit operator string(TempFile tempFile) => tempFile.Path;

    public void Dispose()
    {
        if (File.Exists(Path))
            File.Delete(Path);
    }
}
