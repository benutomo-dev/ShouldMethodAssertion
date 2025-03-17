using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldBeNullTests
{
    [Fact]
    public void ShouldBeNull_NotFail()
    {
        default(object).Should().BeNull();
        default(string).Should().BeNull();
        default(int?).Should().BeNull();
    }

    [Fact]
    public void ShouldBeNull_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeNull() );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "asdf".Should().BeNull());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeNull());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((int?)1).Should().BeNull());
    }

    [Fact]
    public void ShouldNotBeNull_NotFail()
    {
        new object().Should().NotBeNull();
        "asdf".Should().NotBeNull();
        1.Should().NotBeNull();
        ((int?)1).Should().NotBeNull();
    }

    [Fact]
    public void ShouldNotBeNull_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(object).Should().NotBeNull());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(string).Should().NotBeNull());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int?).Should().NotBeNull());
    }
}
