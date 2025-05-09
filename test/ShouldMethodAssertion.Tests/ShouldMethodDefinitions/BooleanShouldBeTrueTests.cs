using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class BooleanShouldBeTrueTests
{
    [Theory]
    [CombinatorialData]
    public void ShouldBeXxx_NotFail(bool value)
    {
        if (value)
            new BooleanShouldBeTrue(value, "actual", default).ShouldBeTrue();
        else
            new BooleanShouldBeTrue(value, "actual", default).ShouldBeFalse();
    }

    [Theory]
    [CombinatorialData]
    public void ShouldBeXxx_Fail(bool value)
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            if (value)
                new BooleanShouldBeTrue(value, "actual", default).ShouldBeFalse();
            else
                new BooleanShouldBeTrue(value, "actual", default).ShouldBeTrue();
        });
    }
}
