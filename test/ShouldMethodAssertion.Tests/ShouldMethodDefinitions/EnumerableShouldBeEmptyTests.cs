using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class EnumerableShouldBeEmptyTests
{
    [Fact]
    public void ShouldBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldBeEmpty<int>([1], "actual", default).ShouldBeEmpty()
            );
    }

    [Fact]
    public void ShouldBeEmpty_NotFail()
    {
        new EnumerableShouldBeEmpty<int>([], "actual", default).ShouldBeEmpty();
    }

    [Fact]
    public void ShouldNotBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldBeEmpty<int>([], "actual", default).ShouldNotBeEmpty()
            );
    }

    [Fact]
    public void ShouldNotBeEmpty_NotFail()
    {
        new EnumerableShouldBeEmpty<int>([1], "actual", default).ShouldNotBeEmpty();
    }
}
