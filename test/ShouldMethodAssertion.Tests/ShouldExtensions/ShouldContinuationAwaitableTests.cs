using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldExtensions;

public class ShouldContinuationAwaitableTests
{
    [Fact]
    public async Task Result()
    {
        Assert.Equal(1, await new ShouldContinuationAwaitable<int>(Task.FromResult(1)));
    }

    [Fact]
    public async Task AndSatisfy_Success()
    {
        var shouldContinuation = new ShouldContinuationAwaitable<int>(Task.FromResult(1));

        await shouldContinuation.AndSatisfy(v => v.Should().Be(1));
    }

    [Fact]
    public async Task AndSatisfy_Success_WithConfigureAwait()
    {
        var shouldContinuation = new ShouldContinuationAwaitable<int>(Task.FromResult(1));

        await shouldContinuation.AndSatisfy(v => v.Should().Be(1)).ConfigureAwait(true);
    }

    [Fact]
    public async Task AndSatisfy_Fail()
    {
        var shouldContinuation = new ShouldContinuationAwaitable<int>(Task.FromResult(1));

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () => await shouldContinuation.AndSatisfy(v => v.Should().Be(2)));
    }

    [Fact]
    public async Task AndSatisfy_Fail_WithConfigureAwait()
    {
        var shouldContinuation = new ShouldContinuationAwaitable<int>(Task.FromResult(1));

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () => await shouldContinuation.AndSatisfy(v => v.Should().Be(2)).ConfigureAwait(false));
    }
}
