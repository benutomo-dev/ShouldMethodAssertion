using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.ObjectShouldBeNull)])]
public class ObjectShouldBeNullTests
{
    [Fact]
    public void ShouldBeNull_NotFail()
    {
        default(object).Should().BeNull();
        default(string).Should().BeNull();
    }

    [Fact]
    public void ShouldBeNull_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeNull() );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "asdf".Should().BeNull());

#pragma warning disable SMAssertion0001 // Inappropriate use of BeNull on value types
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeNull());
#pragma warning restore SMAssertion0001 // Inappropriate use of BeNull on value types
    }

    [Fact]
    public void ShouldNotBeNull_NotFail()
    {
        new object().Should().NotBeNull();
        "asdf".Should().NotBeNull();

#pragma warning disable SMAssertion0002 // Inappropriate use of NotBeNull on value types
        1.Should().NotBeNull();
#pragma warning restore SMAssertion0002 // Inappropriate use of NotBeNull on value types
    }

    [Fact]
    public void ShouldNotBeNull_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(object).Should().NotBeNull());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(string).Should().NotBeNull());
    }
}
