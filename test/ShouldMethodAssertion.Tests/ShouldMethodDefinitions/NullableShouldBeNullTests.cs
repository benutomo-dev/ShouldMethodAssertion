using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.NullableShouldBeNull<StructTypeArg>)])]
public class NullableShouldBeNullTests
{
    [Fact]
    public void ShouldBeNull_NotFail()
    {
        default(int?).Should().BeNull();
    }

    [Fact]
    public void ShouldBeNull_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((int?)1).Should().BeNull());
    }

    [Fact]
    public void ShouldNotBeNull_NotFail()
    {
        ((int?)1).Should().NotBeNull();
    }

    [Fact]
    public void ShouldNotBeNull_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int?).Should().NotBeNull());
    }
}
