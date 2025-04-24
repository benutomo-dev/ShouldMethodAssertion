using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.ObjectShouldBeDefault)])]
public class ObjectShouldBeDefaultTests
{
    private struct RawStruct(int value)
    {
        public int Value { get; } = value;
    }

    [Fact]
    public void ShouldBeDefault_NotFail()
    {
        default(int).Should().BeDefault();
        default(RawStruct).Should().BeDefault();
    }

    [Fact]
    public void ShouldBeDefault_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new RawStruct(1).Should().BeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeDefault());
    }

    [Fact]
    public void ShouldNotBeDefault_NotFail()
    {
        new RawStruct(1).Should().NotBeDefault();
        1.Should().NotBeDefault();
    }

    [Fact]
    public void ShouldNotBeDefault_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int).Should().NotBeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 0.Should().NotBeDefault()); // intの0はdefault(int)と等価
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(RawStruct).Should().NotBeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new RawStruct(0).Should().NotBeDefault()); // new RawStruct(0)はdefault(RawStruct)と等価
    }
}
