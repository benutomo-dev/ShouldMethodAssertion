using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldBeDefaultTests
{
    private struct RawStruct(int value)
    {
        public int Value { get; } = value;
    }

    [Fact]
    public void ShouldBeDefault_NotFail()
    {
        new ObjectShouldBeDefault(default(int), "actual", default).ShouldBeDefault();
        new ObjectShouldBeDefault(default(RawStruct), "actual", default).ShouldBeDefault();
    }

    [Fact]
    public void ShouldBeDefault_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeDefault(new RawStruct(1), "actual", default).ShouldBeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeDefault(1, "actual", default).ShouldBeDefault());
    }

    [Fact]
    public void ShouldNotBeDefault_NotFail()
    {
        new ObjectShouldBeDefault(new RawStruct(1), "actual", default).ShouldNotBeDefault();
        new ObjectShouldBeDefault(1, "actual", default).ShouldNotBeDefault();
    }

    [Fact]
    public void ShouldNotBeDefault_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeDefault(default(int), "actual", default).ShouldNotBeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeDefault(0, "actual", default).ShouldNotBeDefault()); // intの0はdefault(int)と等価
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeDefault(default(RawStruct), "actual", default).ShouldNotBeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeDefault(new RawStruct(0), "actual", default).ShouldNotBeDefault()); // new RawStruct(0)はdefault(RawStruct)と等価
    }
}
