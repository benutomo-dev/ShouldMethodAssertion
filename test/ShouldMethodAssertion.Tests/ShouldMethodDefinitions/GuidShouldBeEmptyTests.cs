using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class GuidShouldBeEmptyTests
{
    [Fact]
    public void ShouldBeEmpty_NotFail()
    {
        new GuidShouldBeEmpty(default(Guid), "actual", default).ShouldBeEmpty();
        new GuidShouldBeEmpty(Guid.Empty, "actual", default).ShouldBeEmpty();
    }

    [Fact]
    public void ShouldBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new GuidShouldBeEmpty(Guid.NewGuid(), "actual", default).ShouldBeEmpty());
    }

    [Fact]
    public void ShouldNotBeEmpty_NotFail()
    {
        new GuidShouldBeEmpty(Guid.NewGuid(), "actual", default).ShouldNotBeEmpty();
    }

    [Fact]
    public void ShouldNotBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new GuidShouldBeEmpty(default(Guid), "actual", default).ShouldNotBeEmpty());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new GuidShouldBeEmpty(Guid.Empty, "actual", default).ShouldNotBeEmpty());
    }
}
