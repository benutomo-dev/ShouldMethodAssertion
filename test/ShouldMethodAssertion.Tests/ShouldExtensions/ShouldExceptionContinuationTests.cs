using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldExtensions;

public class ShouldExceptionContinuationTests
{
    [Fact]
    public void Exception()
    {
        var ex = new IOException();

        Assert.Same(ex, new ShouldExceptionContinuation<IOException>(ex).Exception);
    }

    [Fact]
    public void AndSatisfy_Success()
    {
        var ex = new IOException();

        var shouldContinuation = new ShouldExceptionContinuation<IOException>(ex);

        shouldContinuation.AndSatisfy(v => v.Should().SameReferenceAs(ex));
    }

    [Fact]
    public void AndSatisfy_Fail()
    {
        var ex = new IOException();

        var shouldContinuation = new ShouldExceptionContinuation<IOException>(ex);

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => shouldContinuation.AndSatisfy(v => v.Should().NotSameReferenceAs(ex)));
    }

    [Fact]
    public void AndMessageMatch_Success()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldExceptionContinuation<IOException>(ex);

        shouldContinuation.AndMessageMatch("*A");
    }

    [Fact]
    public void AndMessageMatch_Fail()
    {
        var ex = new IOException("messageA");

        var shouldContinuation = new ShouldExceptionContinuation<IOException>(ex);

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => shouldContinuation.AndMessageMatch("*B"));
    }

    [Theory]
    [CombinatorialData]
    public void Chaining([CombinatorialRange(0, 4)] int failPoint)
    {
        var ex = new IOException("messageA");

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => {
            var shouldContinuation = new ShouldExceptionContinuation<IOException>(ex)
                .AndMessageMatch(failPoint == 0 ? "" : "*")
                .AndSatisfy(v => { if (failPoint == 1) throw new Xunit.Sdk.ShouldMethodAssertionException(); })
                .AndMessageMatch(failPoint == 2 ? "" : "*")
                .AndSatisfy(v => { if (failPoint == 3) throw new Xunit.Sdk.ShouldMethodAssertionException(); });
        });
    }
}
