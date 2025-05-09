using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.DictionaryShouldContainKey<TypeArg, TypeArg>)])]
public class DictionaryShouldContainKeyTests
{
    [Fact]
    public void ShouldContainKey_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContainKey<int, string>(new Dictionary<int, string>(), "actual", default).ShouldContainKey(1)
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContainKey<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldContainKey(2)
            );
    }

    [Fact]
    public void ShouldContainKey_NotFail()
    {
        new DictionaryShouldContainKey<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldContainKey(1);
        new DictionaryShouldContainKey<string, string>(new Dictionary<string, string> { { "asdf", "2" } }, "actual", default).ShouldContainKey("asdf");
        new DictionaryShouldContainKey<string, string>(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "asdf", "2" } }, "actual", default).ShouldContainKey("ASDF");
    }

    [Fact]
    public void ShouldNotContainKey_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContainKey<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldNotContainKey(1)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContainKey<string, string>(new Dictionary<string, string> { { "asdf", "2" } }, "actual", default).ShouldNotContainKey("asdf")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContainKey<string, string>(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { { "asdf", "2" } }, "actual", default).ShouldNotContainKey("ASDF")
            );
    }

    [Fact]
    public void ShouldNotContainKey_NotFail()
    {
        new DictionaryShouldContainKey<int, string>(new Dictionary<int, string>(), "actual", default).ShouldNotContainKey(1);
        new DictionaryShouldContainKey<string, string>(new Dictionary<string, string> { { "asdf", "2" } }, "actual", default).ShouldNotContainKey("ASDF");
    }
}
