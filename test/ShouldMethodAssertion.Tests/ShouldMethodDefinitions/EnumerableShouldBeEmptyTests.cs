using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.EnumerableShouldBeEmpty<TypeArg>)])]
public class EnumerableShouldBeEmptyTests
{
    [Fact]
    public void ShouldBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1 }.Should().BeEmpty()
            );
    }

    [Fact]
    public void ShouldBeEmpty_NotFail()
    {
        Array.Empty<int>().Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => Array.Empty<int>().Should().NotBeEmpty()
            );
    }

    [Fact]
    public void ShouldNotBeEmpty_NotFail()
    {
        new[] { 1 }.Should().NotBeEmpty();
    }
}
