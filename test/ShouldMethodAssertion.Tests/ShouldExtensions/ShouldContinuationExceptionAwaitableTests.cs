using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldExtensions;

public class ShouldContinuationExceptionAwaitableTests
{
    [Fact]
    public async Task Result()
    {
        var ex = new IOException("messageA");

        Assert.Same(ex, await new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex)));
    }

    [Fact]
    public async Task AndSatisfy_Success()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex));

        await shouldContinuation.AndSatisfy(v => v.Should().SameReferenceAs(ex));
    }

    [Fact]
    public async Task AndSatisfy_Success_WithConfigureAwait()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex));

        await shouldContinuation.AndSatisfy(v => v.Should().SameReferenceAs(ex)).ConfigureAwait(true);
    }

    [Fact]
    public async Task AndSatisfy_Fail()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex));

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () => await shouldContinuation.AndSatisfy(v => v.Should().NotSameReferenceAs(ex)));
    }

    [Fact]
    public async Task AndSatisfy_Fail_WithConfigureAwait()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex));

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () => await shouldContinuation.AndSatisfy(v => v.Should().NotSameReferenceAs(ex)).ConfigureAwait(false));
    }

    [Fact]
    public async Task AndMessageMatch_Success()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex));

        await shouldContinuation.AndMessageMatch("*A");
    }

    [Fact]
    public async Task AndMessageMatch_Success_WithConfigureAwait()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex));

        await shouldContinuation.AndMessageMatch("*A").ConfigureAwait(true);
    }

    [Fact]
    public async Task AndMessageMatch_Fail()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex));

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () => await shouldContinuation.AndMessageMatch("*B"));
    }

    [Fact]
    public async Task AndMessageMatch_Fail_WithConfigureAwait()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex));

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () => await shouldContinuation.AndMessageMatch("*B").ConfigureAwait(false));
    }

    [Theory]
    [CombinatorialData]
    public async Task Chaining([CombinatorialRange(0, 4)] int failPoint)
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldContinuationExceptionAwaitable<IOException>(Task.FromResult(ex))
            .AndMessageMatch(failPoint == 0 ? "" : "*")
            .AndSatisfy(v => { if (failPoint == 1) throw new Xunit.Sdk.ShouldMethodAssertionException(); })
            .AndMessageMatch(failPoint == 2 ? "" : "*")
            .AndSatisfy(v => { if (failPoint == 3) throw new Xunit.Sdk.ShouldMethodAssertionException(); });

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () => await shouldContinuation);
    }
}
