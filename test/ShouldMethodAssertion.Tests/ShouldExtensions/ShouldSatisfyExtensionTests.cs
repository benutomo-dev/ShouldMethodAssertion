using ShouldMethodAssertion.ShouldExtensions;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ShouldMethodAssertion.Tests.ShouldExtensions;

public class ShouldSatisfyExtensionTests
{
    [Fact]
    public void ShouldSatisfy_AssertFail()
    {
        var frames = new StackFrame[4];

        var exception = Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            frames[3] = new StackFrame();

            "asdf".ShouldSatisfy(actual =>
            {
                frames[2] = new StackFrame();

                actual.ShouldSatisfy(actual =>
                {
                    frames[1] = new StackFrame();

                    actual.ShouldSatisfy(actual =>
                    {
                        frames[0] = new StackFrame();

                        actual.Length.Should().Be(3);
                    });
                });
            });
        });

        Assert.Contains(@"`actual.Length` is `3`. But did not expect it to be.", exception.Message);

        var stackTraceLines = frames.Select(v => new StackTrace(v).ToString().Trim()).ToArray();

        foreach (var stackTraceLine in stackTraceLines)
        {
            Assert.Contains(stackTraceLine, exception.StackTrace);
        }

        var regexp = string.Join(@"(.|\r?\n)+?", stackTraceLines.Select(Regex.Escape));

        Assert.True(Regex.IsMatch(exception.StackTrace ?? "", regexp, RegexOptions.Multiline));
    }

    [Fact]
    public void ShouldSatisfy_Exception()
    {
        var frames = new StackFrame[3];

        var exception = Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            frames[2] = new StackFrame();

            "asdf".ShouldSatisfy(actual =>
            {
                frames[1] = new StackFrame();

                actual.ShouldSatisfy(actual =>
                {
                    frames[0] = new StackFrame();

                    actual.ShouldSatisfy(actual =>
                    {
                        throw new InvalidOperationException("ExpectedException");
                    });
                });
            });
        });

        Assert.Contains("InvalidOperationException", exception.Message);
        Assert.Contains("ExpectedException", exception.Message);

        Assert.IsType<InvalidOperationException>(exception.InnerException);

        var stackTraceLines = frames.Select(v => new StackTrace(v).ToString().Trim()).ToArray();

        foreach (var stackTraceLine in stackTraceLines)
        {
            Assert.Contains(stackTraceLine, exception.StackTrace);
        }

        var regexp = string.Join(@"(.|\r?\n)+?", stackTraceLines.Select(Regex.Escape));

        Assert.True(Regex.IsMatch(exception.StackTrace ?? "", regexp, RegexOptions.Multiline));
    }

    [Fact]
    public async Task ShouldSatisfyAsync_AssertFail()
    {
        var frames = new StackFrame[3];

        var exception = await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await "asdf".ShouldSatisfyAsync(async actual =>
            {
                await Task.Delay(1).ConfigureAwait(false);

                frames[2] = new StackFrame();

                await Task.Delay(1).ConfigureAwait(false);

                await actual.ShouldSatisfyAsync(async actual =>
                {
                    await Task.Delay(1).ConfigureAwait(false);

                    frames[1] = new StackFrame();

                    await Task.Delay(1).ConfigureAwait(false);

                    await actual.ShouldSatisfyAsync(async actual =>
                    {
                        await Task.Delay(1).ConfigureAwait(false);

                        frames[0] = new StackFrame();

                        await Task.Delay(1).ConfigureAwait(false);

                        actual.Length.Should().Be(3);
                    });
                });
            });
        });

        Assert.Contains(@"`actual.Length` is `3`. But did not expect it to be.", exception.Message);

        var stackTraceLines = frames.Select(v => new StackTrace(v).ToString().Trim()).ToArray();

        foreach (var stackTraceLine in stackTraceLines)
        {
            Assert.Contains(stackTraceLine, exception.StackTrace);
        }

        var regexp = string.Join(@"(.|\r?\n)+?", stackTraceLines.Select(Regex.Escape));

        Assert.True(Regex.IsMatch(exception.StackTrace ?? "", regexp, RegexOptions.Multiline));
    }

    [Fact]
    public async Task ShouldSatisfyAsync_Exception()
    {
        var frames = new StackFrame[2];

        var exception = await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await "asdf".ShouldSatisfyAsync(async actual =>
            {
                await Task.Delay(1).ConfigureAwait(false);

                frames[1] = new StackFrame();

                await Task.Delay(1).ConfigureAwait(false);

                await actual.ShouldSatisfyAsync(async actual =>
                {
                    await Task.Delay(1).ConfigureAwait(false);

                    frames[0] = new StackFrame();

                    await Task.Delay(1).ConfigureAwait(false);

                    await actual.ShouldSatisfyAsync(async actual =>
                    {
                        throw new InvalidOperationException("ExpectedException");
                    });
                });
            });
        });

        Assert.Contains("InvalidOperationException", exception.Message);
        Assert.Contains("ExpectedException", exception.Message);

        Assert.IsType<InvalidOperationException>(exception.InnerException);

        var stackTraceLines = frames.Select(v => new StackTrace(v).ToString().Trim()).ToArray();

        foreach (var stackTraceLine in stackTraceLines)
        {
            Assert.Contains(stackTraceLine, exception.StackTrace);
        }

        var regexp = string.Join(@"(.|\r?\n)+?", stackTraceLines.Select(Regex.Escape));

        Assert.True(Regex.IsMatch(exception.StackTrace ?? "", regexp, RegexOptions.Multiline));
    }
}
