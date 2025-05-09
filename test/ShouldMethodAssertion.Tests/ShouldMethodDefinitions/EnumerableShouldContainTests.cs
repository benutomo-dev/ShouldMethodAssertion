using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class EnumerableShouldContainTests
{
    [Fact]
    public void ShouldContain_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldContain<int>([], "actual", default).ShouldContain(1)
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldContain<int>([1], "actual", default).ShouldContain(2)
            );
    }

    [Fact]
    public void ShouldContain_NotFail()
    {
        new EnumerableShouldContain<int>([1], "actual", default).ShouldContain(1);
        new EnumerableShouldContain<string>(["asdf"], "actual", default).ShouldContain("asdf");
        new EnumerableShouldContain<string>(["asdf"], "actual", default).ShouldContain("ASDF", StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void ShouldNotContain_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldContain<int>([1], "actual", default).ShouldNotContain(1)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldContain<string>(["asdf"], "actual", default).ShouldNotContain("asdf")
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldContain<string>(["asdf"], "actual", default).ShouldNotContain("ASDF", StringComparer.OrdinalIgnoreCase)
            );
    }

    [Fact]
    public void ShouldNotContain_NotFail()
    {
        new EnumerableShouldContain<int>([], "actual", default).ShouldNotContain(1);
        new EnumerableShouldContain<string>(["asdf"], "actual", default).ShouldNotContain("ASDF");
    }
}
