using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.NullableStructShouldHaveValue<StructTypeArg>)])]
public class NullableStructShouldHaveValueTests
{
    [Fact]
    public void ShouldNotHaveValue_NotFail()
    {
        default(int?).Should().NotHaveValue();
    }

    [Fact]
    public void ShouldNotHaveValue_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((int?)1).Should().NotHaveValue());
    }

    [Fact]
    public void ShouldHaveValue_NotFail()
    {
        ((int?)1).Should().HaveValue();
    }

    [Fact]
    public void ShouldHaveValue_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int?).Should().HaveValue());
    }
}
