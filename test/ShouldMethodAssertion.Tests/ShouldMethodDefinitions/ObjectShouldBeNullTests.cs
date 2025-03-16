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
        Assert.ThrowsAny<Exception>(() => new object().Should().BeNull() );
        Assert.ThrowsAny<Exception>(() => "asdf".Should().BeNull());
        Assert.ThrowsAny<Exception>(() => 1.Should().BeNull());
        Assert.ThrowsAny<Exception>(() => ((int?)1).Should().BeNull());
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
        Assert.ThrowsAny<Exception>(() => default(object).Should().NotBeNull());
        Assert.ThrowsAny<Exception>(() => default(string).Should().NotBeNull());
        Assert.ThrowsAny<Exception>(() => default(int?).Should().NotBeNull());
    }
}
