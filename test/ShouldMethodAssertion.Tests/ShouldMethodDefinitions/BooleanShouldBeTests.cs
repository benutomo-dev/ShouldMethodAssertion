using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class BooleanShouldBeTests
{
    [Theory]
    [CombinatorialData]
    public void ShouldBeXxx_NotFail(bool value)
    {
        if (value)
            value.Should().BeTrue();
        else
            value.Should().BeFalse();
    }

    [Theory]
    [CombinatorialData]
    public void ShouldBeXxx_Fail(bool value)
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            if (value)
                value.Should().BeFalse();
            else
                value.Should().BeTrue();
        });
    }
}
