using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.DictionaryShouldContain<TypeArg, TypeArg>)])]
public class DictionaryShouldContainTests
{
    [Fact]
    public void ShouldContain_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string>().Should().Contain(1, "1")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string>().Should().Contain(1, "1", StringComparer.OrdinalIgnoreCase)
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "2" } }.Should().Contain(1, "1")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "2" } }.Should().Contain(2, "2")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "2" } }.Should().Contain(1, "1", StringComparer.OrdinalIgnoreCase)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "2" } }.Should().Contain(2, "2", StringComparer.OrdinalIgnoreCase)
            );
    }

    [Fact]
    public void ShouldContain_NotFail()
    {
        new Dictionary<int, string> { { 1, "2" } }.Should().Contain(1, "2");
        new Dictionary<int, string> { { 1, "asdf" } }.Should().Contain(1, "asdf");
        new Dictionary<int, string> { { 1, "asdf" } }.Should().Contain(1, "ASDF", StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void ShouldNotContain_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "2" } }.Should().NotContain(1, "2")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "asdf" } }.Should().NotContain(1, "asdf")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "asdf" } }.Should().NotContain(1, "ASDF", StringComparer.OrdinalIgnoreCase)
            );
    }

    [Fact]
    public void ShouldNotContain_NotFail()
    {
        new Dictionary<int, string>().Should().NotContain(1, "1");
        new Dictionary<int, string>().Should().NotContain(1, "1", StringComparer.OrdinalIgnoreCase);
        new Dictionary<int, string> { { 1, "2" } }.Should().NotContain(1, "1");
        new Dictionary<int, string> { { 1, "2" } }.Should().NotContain(2, "2");
        new Dictionary<int, string> { { 1, "2" } }.Should().NotContain(1, "1", StringComparer.OrdinalIgnoreCase);
        new Dictionary<int, string> { { 1, "2" } }.Should().NotContain(2, "2", StringComparer.OrdinalIgnoreCase);
    }
}
