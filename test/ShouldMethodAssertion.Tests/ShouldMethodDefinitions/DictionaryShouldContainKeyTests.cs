using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.DictionaryShouldContainKey<TypeArg, TypeArg>)])]
public class DictionaryShouldContainKeyTests
{
    [Fact]
    public void ShouldContainKey_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string>().Should().ContainKey(1)
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "2" } }.Should().ContainKey(2)
            );
    }

    [Fact]
    public void ShouldContainKey_NotFail()
    {
        new Dictionary<int, string> { { 1, "2" } }.Should().ContainKey(1);
        new Dictionary<string, string> { { "asdf", "2" } }.Should().ContainKey("asdf");
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "asdf", "2" } }.Should().ContainKey("ASDF");
    }

    [Fact]
    public void ShouldNotContainKey_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "2" } }.Should().NotContainKey(1)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<string, string> { { "asdf", "2" } }.Should().NotContainKey("asdf")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "asdf", "2" } }.Should().NotContainKey("ASDF")
            );
    }

    [Fact]
    public void ShouldNotContainKey_NotFail()
    {
        new Dictionary<int, string>().Should().NotContainKey(1);
        new Dictionary<string, string> { { "asdf", "2" } }.Should().NotContainKey("ASDF");
    }
}
