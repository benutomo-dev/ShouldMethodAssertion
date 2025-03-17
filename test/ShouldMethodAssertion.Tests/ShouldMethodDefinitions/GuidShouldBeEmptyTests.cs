using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class GuidShouldBeEmptyTests
{
    [Fact]
    public void ShouldBeEmpty_NotFail()
    {
        default(Guid).Should().BeEmpty();
        Guid.Empty.Should().BeEmpty();
    }

    [Fact]
    public void ShouldBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Guid.NewGuid().Should().BeEmpty());
    }

    [Fact]
    public void ShouldNotBeEmpty_NotFail()
    {
        Guid.NewGuid().Should().NotBeEmpty();
    }

    [Fact]
    public void ShouldNotBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(Guid).Should().NotBeEmpty());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Guid.Empty.Should().NotBeEmpty());
    }
}
