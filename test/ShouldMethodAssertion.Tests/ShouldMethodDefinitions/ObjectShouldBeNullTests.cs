using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldBeNullTests
{
    [Fact]
    public void ShouldBeNull_NotFail()
    {
        new ObjectShouldBeNull(default(object), "actual", default).ShouldBeNull();
        new ObjectShouldBeNull(default(string), "actual", default).ShouldBeNull();
    }

    [Fact]
    public void ShouldBeNull_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeNull(new object(), "actual", default).ShouldBeNull() );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeNull("asdf", "actual", default).ShouldBeNull());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeNull(1, "actual", default).ShouldBeNull());
    }

    [Fact]
    public void ShouldNotBeNull_NotFail()
    {
        new ObjectShouldBeNull(new object(), "actual", default).ShouldNotBeNull();
        new ObjectShouldBeNull("asdf", "actual", default).ShouldNotBeNull();
        new ObjectShouldBeNull(1, "actual", default).ShouldNotBeNull();
    }

    [Fact]
    public void ShouldNotBeNull_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeNull(default(object), "actual", default).ShouldNotBeNull());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeNull(default(string), "actual", default).ShouldNotBeNull());
    }
}
