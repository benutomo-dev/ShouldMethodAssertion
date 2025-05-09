using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class DictionaryShouldContainTests
{
    [Fact]
    public void ShouldContain_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string>(), "actual", default).ShouldContain(1, "1")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string>(), "actual", default).ShouldContain(1, "1", StringComparer.OrdinalIgnoreCase)
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldContain(1, "1")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldContain(2, "2")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldContain(1, "1", StringComparer.OrdinalIgnoreCase)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldContain(2, "2", StringComparer.OrdinalIgnoreCase)
            );
    }

    [Fact]
    public void ShouldContain_NotFail()
    {
        new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldContain(1, "2");
        new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldContain(1, "asdf");
        new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldContain(1, "ASDF", StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void ShouldNotContain_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldNotContain(1, "2")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldNotContain(1, "asdf")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldNotContain(1, "ASDF", StringComparer.OrdinalIgnoreCase)
            );
    }

    [Fact]
    public void ShouldNotContain_NotFail()
    {
        new DictionaryShouldContain<int, string>(new Dictionary<int, string>(), "actual", default).ShouldNotContain(1, "1");
        new DictionaryShouldContain<int, string>(new Dictionary<int, string>(), "actual", default).ShouldNotContain(1, "1", StringComparer.OrdinalIgnoreCase);
        new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldNotContain(1, "1");
        new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldNotContain(2, "2");
        new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldNotContain(1, "1", StringComparer.OrdinalIgnoreCase);
        new DictionaryShouldContain<int, string>(new Dictionary<int, string> { { 1, "2" } }, "actual", default).ShouldNotContain(2, "2", StringComparer.OrdinalIgnoreCase);
    }
}
