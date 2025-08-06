using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class DictionaryShouldAllSatisfyTests
{
    [Fact]
    public void ShouldAllSatisfy_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldAllSatisfy<int, string>(new Dictionary<int, string>() { { 1, "apple" } }, "actual", default).ShouldAllSatisfy((key, value) => throw new InvalidOperationException())
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldAllSatisfy<int, string>(new Dictionary<int, string>() { { 1, "apple" }, { 2, "banana" } }, "actual", default).ShouldAllSatisfy((key, value) => key.Should().Be(-1))
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldAllSatisfy<int, string>(new Dictionary<int, string>() { { 1, "apple" }, { 2, "banana" } }, "actual", default).ShouldAllSatisfy((key, value) => key.Should().Be(1))
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldAllSatisfy<int, string>(new Dictionary<int, string>() { { 1, "apple" }, { 2, "banana" } }, "actual", default).ShouldAllSatisfy((key, value) => key.Should().Be(2))
            );
    }

    [Fact]
    public void ShouldAllSatisfy_NotFail()
    {
        new DictionaryShouldAllSatisfy<int, string>(new Dictionary<int, string>(), "actual", default).ShouldAllSatisfy((key, value) => throw new InvalidOperationException());

        new DictionaryShouldAllSatisfy<int, string>(new Dictionary<int, string>(), "actual", default).ShouldAllSatisfy((key, value) => key.Should().Be(1));

        new DictionaryShouldAllSatisfy<int, string>(new Dictionary<int, string>() { { 1, "apple" }, { 2, "banana" } }, "actual", default).ShouldAllSatisfy((key, value) => { });
    }
}
