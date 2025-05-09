using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class NullableStructShouldHaveValueTests
{
    [Fact]
    public void ShouldNotHaveValue_NotFail()
    {
        new NullableStructShouldHaveValue<int>(default(int?), "actual", default).ShouldNotHaveValue();
    }

    [Fact]
    public void ShouldNotHaveValue_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldHaveValue<int>(((int?)1), "actual", default).ShouldNotHaveValue());
    }

    [Fact]
    public void ShouldHaveValue_NotFail()
    {
        new NullableStructShouldHaveValue<int>(((int?)1), "actual", default).ShouldHaveValue();
    }

    [Fact]
    public void ShouldHaveValue_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldHaveValue<int>(default(int?), "actual", default).ShouldHaveValue());
    }
}
