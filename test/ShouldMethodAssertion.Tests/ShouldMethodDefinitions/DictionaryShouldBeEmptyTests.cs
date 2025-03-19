using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.DictionaryShouldBeEmpty<TypeArg, TypeArg>)])]
public class DictionaryShouldBeEmptyTests
{
    [Fact]
    public void ShouldBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().BeEmpty()
            );
    }

    [Fact]
    public void ShouldBeEmpty_NotFail()
    {
        new Dictionary<int, string>().Should().BeEmpty();
    }

    [Fact]
    public void ShouldNotBeEmpty_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string>().Should().NotBeEmpty()
            );
    }

    [Fact]
    public void ShouldNotBeEmpty_NotFail()
    {
        new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().NotBeEmpty();
    }
}
