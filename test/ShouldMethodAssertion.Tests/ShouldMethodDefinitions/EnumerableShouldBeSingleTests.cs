using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class EnumerableShouldBeSingleTests
{
    [Fact]
    public void ShouldBeSingle_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldBeSingle<int>([], "actual", default).ShouldBeSingle()
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldBeSingle<int>([1, 2], "actual", default).ShouldBeSingle()
            );
    }

    [Fact]
    public void ShouldBeSingle_NotFail()
    {
        var shouldContinuation = new EnumerableShouldBeSingle<int>([1], "actual", default).ShouldBeSingle();

        Assert.Equal(1, shouldContinuation.Result);
    }

    [Fact]
    public void ShouldNotBeSingle_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldBeSingle<int>([1], "actual", default).ShouldNotBeSingle()
            );
    }

    [Fact]
    public void ShouldNotBeSingle_NotFail()
    {
        new EnumerableShouldBeSingle<int>([], "actual", default).ShouldNotBeSingle();
        new EnumerableShouldBeSingle<int>([1, 2], "actual", default).ShouldNotBeSingle();
    }
}
