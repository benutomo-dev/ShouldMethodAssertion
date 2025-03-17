using ShouldMethodAssertion.ShouldExtensions;

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
        Flags.None.Should().HaveFlag(Flags.None);
        Flags.A.Should().HaveFlag(Flags.None);
        Flags.B.Should().HaveFlag(Flags.B);
        Flags.BC.Should().HaveFlag(Flags.B);
        ((Flags)~0).Should().HaveFlag(Flags.BC);
    }

    [Fact]
    public void ShouldHaveFlag_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Flags.None.Should().HaveFlag(Flags.A));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Flags.BC.Should().HaveFlag(Flags.A));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => (Flags.A | Flags.B).Should().HaveFlag(Flags.BC));
    }

    [Fact]
    public void ShouldNotHaveFlag_NotFail()
    {
        Flags.None.Should().NotHaveFlag(Flags.A);
        Flags.BC.Should().NotHaveFlag(Flags.A);
        (Flags.A | Flags.B).Should().NotHaveFlag(Flags.BC);
    }

    [Fact]
    public void ShouldNotHaveFlag_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Flags.None.Should().NotHaveFlag(Flags.None));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Flags.A.Should().NotHaveFlag(Flags.None));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Flags.B.Should().NotHaveFlag(Flags.B));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Flags.BC.Should().NotHaveFlag(Flags.B));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Flags)~0).Should().NotHaveFlag(Flags.BC));
    }
}
