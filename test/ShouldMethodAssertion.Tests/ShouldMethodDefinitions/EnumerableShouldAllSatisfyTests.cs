using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class EnumerableShouldAllSatisfyTests
{
    [Fact]
    public void ShouldAllSatisfy_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldAllSatisfy<int>([1], "actual", default).ShouldAllSatisfy(v => throw new InvalidOperationException())
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldAllSatisfy<int>([1, 2], "actual", default).ShouldAllSatisfy(v => v.Should().Be(-1))
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldAllSatisfy<int>([1, 2], "actual", default).ShouldAllSatisfy(v => v.Should().Be(1))
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new EnumerableShouldAllSatisfy<int>([1, 2], "actual", default).ShouldAllSatisfy(v => v.Should().Be(2))
            );
    }

    [Fact]
    public void ShouldAllSatisfy_NotFail()
    {
        new EnumerableShouldAllSatisfy<int>([], "actual", default).ShouldAllSatisfy(v => throw new InvalidOperationException());

        new EnumerableShouldAllSatisfy<int>([], "actual", default).ShouldAllSatisfy(v => v.Should().Be(1));

        new EnumerableShouldAllSatisfy<int>([1, 2, 3], "actual", default).ShouldAllSatisfy(v => { });
    }
}
