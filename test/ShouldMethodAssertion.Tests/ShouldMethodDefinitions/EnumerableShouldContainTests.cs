using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.EnumerableShouldContain<TypeArg>)])]
public class EnumerableShouldContainTests
{
    [Fact]
    public void ShouldContain_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => Array.Empty<int>().Should().Contain(1)
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] {1}.Should().Contain(2)
            );
    }

    [Fact]
    public void ShouldContain_NotFail()
    {
        new[] { 1 }.Should().Contain(1);
        new[] { "asdf" }.Should().Contain("asdf");
        new[] { "asdf" }.Should().Contain("ASDF", StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void ShouldNotContain_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1 }.Should().NotContain(1)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { "asdf" }.Should().NotContain("asdf")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { "asdf" }.Should().NotContain("ASDF", StringComparer.OrdinalIgnoreCase)
            );
    }

    [Fact]
    public void ShouldNotContain_NotFail()
    {
        Array.Empty<int>().Should().NotContain(1);
        new[] { "asdf" }.Should().NotContain("ASDF");
    }
}
