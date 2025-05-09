using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class EnumShouldHaveFlagTests
{
    [Flags]
    enum Flags
    {
        None = 0,

        A = 0b0001,
        B = 0b0010,
        C = 0b0100,

        BC = 0b0110,
    }


    [Fact]
    public void ShouldHaveFlag_NotFail()
    {
        new EnumShouldHaveFlag(Flags.None, "actual", default).ShouldHaveFlag(Flags.None);
        new EnumShouldHaveFlag(Flags.A, "actual", default).ShouldHaveFlag(Flags.None);
        new EnumShouldHaveFlag(Flags.B, "actual", default).ShouldHaveFlag(Flags.B);
        new EnumShouldHaveFlag(Flags.BC, "actual", default).ShouldHaveFlag(Flags.B);
        new EnumShouldHaveFlag((Flags)~0, "actual", default).ShouldHaveFlag(Flags.BC);
    }

    [Fact]
    public void ShouldHaveFlag_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumShouldHaveFlag(Flags.None, "actual", default).ShouldHaveFlag(Flags.A));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumShouldHaveFlag(Flags.BC, "actual", default).ShouldHaveFlag(Flags.A));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumShouldHaveFlag(Flags.A | Flags.B, "actual", default).ShouldHaveFlag(Flags.BC));
    }

    [Fact]
    public void ShouldNotHaveFlag_NotFail()
    {
        new EnumShouldHaveFlag(Flags.None, "actual", default).ShouldNotHaveFlag(Flags.A);
        new EnumShouldHaveFlag(Flags.BC, "actual", default).ShouldNotHaveFlag(Flags.A);
        new EnumShouldHaveFlag(Flags.A | Flags.B, "actual", default).ShouldNotHaveFlag(Flags.BC);
    }

    [Fact]
    public void ShouldNotHaveFlag_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumShouldHaveFlag(Flags.None, "actual", default).ShouldNotHaveFlag(Flags.None));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumShouldHaveFlag(Flags.A, "actual", default).ShouldNotHaveFlag(Flags.None));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumShouldHaveFlag(Flags.B, "actual", default).ShouldNotHaveFlag(Flags.B));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumShouldHaveFlag(Flags.BC, "actual", default).ShouldNotHaveFlag(Flags.B));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumShouldHaveFlag((Flags)~0, "actual", default).ShouldNotHaveFlag(Flags.BC));
    }
}
