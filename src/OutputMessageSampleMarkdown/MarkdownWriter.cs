using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace OutputMessageSampleMarkdown;

internal sealed partial class MarkdownWriter : IDisposable
{
    private StreamWriter _writer;

    private static string s_userProfileDir;
    private static string s_dummyUserProfileDir;

    static MarkdownWriter()
    {
        s_userProfileDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        s_dummyUserProfileDir = s_userProfileDir.Replace(Environment.UserName, "SOME_USER");
    }

    public MarkdownWriter(string file, string title)
    {
        Stream? stream = null;
        try
        {
            stream = new FileStream(file, FileMode.Create, FileAccess.Write);
            _writer = new StreamWriter(stream, Encoding.UTF8);

            WriteLine($"# {title}");
        }
        catch
        {
            _writer?.Dispose();
            stream?.Dispose();
            throw;
        }
    }

    public void Dispose()
    {
        ((IDisposable)_writer).Dispose();
    }

    public void WriteLine(string line)
    {
        _writer.WriteLine(line);
        _writer.WriteLine();
    }

    [GeneratedRegex(@"\A(\s*|static|async)*(\([\s\w,]*\)|\w+)\s*=>\s*{(\r?\n)?(?<body>(.|\r?\n)*?)(\r?\n)?}\s*\z", RegexOptions.Multiline)]
    private static partial Regex GetLambdaExpressionRegex();

    [GeneratedRegex(@"\r?\n")]
    private static partial Regex GetLineSeparatorRegex();

    public void EmitSuccessSample(Action assertAction, [CallerArgumentExpression(nameof(assertAction))] string assertActionExpression = "")
    {
        EmitTestCode(assertActionExpression);

        try
        {
            assertAction();
        }
        catch (Exception ex)
        {
            _writer.WriteLine($@"**<span style=""color:red; font-weight: bold;"">Message</span>**");
            _writer.WriteLine();

            EmitMessageBody(ex.Message);
        }
    }

    public void EmitMessageSample(Action assertAction, [CallerArgumentExpression(nameof(assertAction))] string assertActionExpression = "")
    {
        EmitTestCode(assertActionExpression);

        try
        {
            _writer.WriteLine($"**Message**");
            _writer.WriteLine();

            assertAction();

            _writer.WriteLine(@"<span style=""color:red; font-weight: bold;"">No Messsage.</span>");
            _writer.WriteLine();
        }
        catch (Exception ex)
        {
            EmitMessageBody(ex.Message);
        }
    }

    public void EmitMessageSample(Action<string> assertAction, [CallerArgumentExpression(nameof(assertAction))] string assertActionExpression = "")
    {
        EmitTestCode(assertActionExpression);

        var guid = Guid.NewGuid().ToString();

        var tempDir = Path.Combine(Path.GetTempPath(), guid);
        Directory.CreateDirectory(tempDir);

        try
        {
            _writer.WriteLine($"**Message**");
            _writer.WriteLine();

            assertAction(tempDir);

            _writer.WriteLine(@"<span style=""color:red; font-weight: bold;"">No Messsage.</span>");
            _writer.WriteLine();
        }
        catch (Exception ex)
        {
            var fixedGuid = "c7397e81-e829-41c3-a755-ee5b39e0fce4";

            EmitMessageBody(ex.Message.Replace(guid, fixedGuid));
        }
        finally
        {
            try
            {
                Directory.Delete(tempDir, recursive: true);
            }
            catch { }
        }
    }

    public async Task EmitSuccessSampleAsync(Func<Task> assertAction, [CallerArgumentExpression(nameof(assertAction))] string assertActionExpression = "")
    {
        EmitTestCode(assertActionExpression);

        try
        {
            await assertAction().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _writer.WriteLine($@"**<span style=""color:red; font-weight: bold;"">Message</span>**");
            _writer.WriteLine();

            EmitMessageBody(ex.Message);
        }
    }

    public async Task EmitMessageSampleAsync(Func<Task> assertAction, [CallerArgumentExpression(nameof(assertAction))] string assertActionExpression = "")
    {
        EmitTestCode(assertActionExpression);

        try
        {
            _writer.WriteLine($"**Message**");
            _writer.WriteLine();

            await assertAction().ConfigureAwait(false);

            _writer.WriteLine(@"<span style=""color:red; font-weight: bold;"">No Messsage.</span>");
            _writer.WriteLine();
        }
        catch (Exception ex)
        {
            EmitMessageBody(ex.Message);
        }
    }

    public async Task EmitMessageSampleAsync(Func<string, Task> assertAction, [CallerArgumentExpression(nameof(assertAction))] string assertActionExpression = "")
    {
        EmitTestCode(assertActionExpression);

        var guid = Guid.NewGuid().ToString();

        var tempDir = Path.Combine(Path.GetTempPath(), guid);
        Directory.CreateDirectory(tempDir);

        try
        {
            _writer.WriteLine($"**Message**");
            _writer.WriteLine();

            await assertAction(tempDir).ConfigureAwait(false);

            _writer.WriteLine(@"<span style=""color:red; font-weight: bold;"">No Messsage.</span>");
            _writer.WriteLine();
        }
        catch (Exception ex)
        {
            var fixedGuid = "c7397e81-e829-41c3-a755-ee5b39e0fce4";

            EmitMessageBody(ex.Message.Replace(guid, fixedGuid));
        }
        finally
        {
            try
            {
                Directory.Delete(tempDir, recursive: true);
            }
            catch { }
        }
    }

    private void EmitTestCode(string assertActionExpression)
    {
        var lambdaMatch = GetLambdaExpressionRegex().Match(assertActionExpression);
        if (lambdaMatch.Success)
        {
            var lines = GetLineSeparatorRegex().Split(lambdaMatch.Groups["body"].Value)
                .SkipWhile(v => string.IsNullOrWhiteSpace(v))
                .Reverse()
                .SkipWhile(v => string.IsNullOrWhiteSpace(v))
                .Reverse()
                .ToArray();

            var skipCount = lines.Select(countLeadingWhileSpace).DefaultIfEmpty(int.MaxValue).Min();

            assertActionExpression = string.Join("\r\n", lines.Select(v => $"{v.AsSpan(Math.Min(v.Length, skipCount))}"));

            static int countLeadingWhileSpace(string line)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] != ' ')
                        return i;
                }

                return int.MaxValue;
            }
        }

        _writer.WriteLine($"**TestCode**");
        _writer.WriteLine();
        _writer.WriteLine("```csharp");
        _writer.WriteLine(assertActionExpression);
        _writer.WriteLine("```");
        _writer.WriteLine();
    }

    private void EmitMessageBody(string message)
    {
        message = message.Replace(s_userProfileDir, s_dummyUserProfileDir, StringComparison.OrdinalIgnoreCase);

        _writer.WriteLine("```text");
        if (message.EndsWith('\n'))
            _writer.Write(message);
        else
            _writer.WriteLine(message);
        _writer.WriteLine("```");
        _writer.WriteLine();
    }
}
