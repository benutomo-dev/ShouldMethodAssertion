using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldExtensions;

public class ShouldContinuationTests
{
    [Fact]
    public void Result()
    {
        Assert.Equal(1, new ShouldContinuation<int>(1).Result);
    }

    [Fact]
    public void AndSatisfy_Success()
    {
        var shouldContinuation = new ShouldContinuation<int>(1);

        shouldContinuation.AndSatisfy(v => v.Should().Be(1));
    }

    [Fact]
    public void AndSatisfy_Fail()
    {
        var shouldContinuation = new ShouldContinuation<int>(1);

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => shouldContinuation.AndSatisfy(v => v.Should().Be(2)));
    }
}
