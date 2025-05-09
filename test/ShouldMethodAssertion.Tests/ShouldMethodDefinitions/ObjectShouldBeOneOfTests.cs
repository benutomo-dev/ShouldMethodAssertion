using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public partial class ObjectShouldBeOneOfTests
{
    sealed record class Class(int Value);

    record struct Struct(int Value);

    [Fact]
    public void ShouldBeOneOf_NotFail()
    {
        var c1 = new Class(1);
        var c2 = new Class(2);

        var s1 = new Struct(1);
        var s2 = new Struct(2);

        new ObjectShouldBeOneOf(1, "actual", default).ShouldBeOneOf([1]);
        new ObjectShouldBeOneOf(1, "actual", default).ShouldBeOneOf([2,1]);
        new ObjectShouldBeOneOf(1, "actual", default).ShouldBeOneOf<object>(["1", c1, s1, 1]);

        new ObjectShouldBeOneOf("1", "actual", default).ShouldBeOneOf(["1"]);
        new ObjectShouldBeOneOf("1", "actual", default).ShouldBeOneOf(["2", "1"]);
        new ObjectShouldBeOneOf("1", "actual", default).ShouldBeOneOf<object>(["1", c1, s1, 1]);
        new ObjectShouldBeOneOf("abcd", "actual", default).ShouldBeOneOf(["2", "ABCD"], StringComparer.OrdinalIgnoreCase);

        new ObjectShouldBeOneOf(c1, "actual", default).ShouldBeOneOf([c1]);
        new ObjectShouldBeOneOf(c1, "actual", default).ShouldBeOneOf([c2, c1]);
        new ObjectShouldBeOneOf(c1, "actual", default).ShouldBeOneOf<object>(["1", c1, s1, 1]);

        new ObjectShouldBeOneOf(s1, "actual", default).ShouldBeOneOf([s1]);
        new ObjectShouldBeOneOf(s1, "actual", default).ShouldBeOneOf([s2, s1]);
        new ObjectShouldBeOneOf(s1, "actual", default).ShouldBeOneOf<object>(["1", c1, s1, 1]);
    }

    [Fact]
    public void ShouldBeOneOf_Fail()
    {
        var c1 = new Class(1);

        var s1 = new Struct(1);

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(1, "actual", default).ShouldBeOneOf(ReadOnlySpan<int>.Empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(1, "actual", default).ShouldBeOneOf([2]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(1, "actual", default).ShouldBeOneOf(["1"]));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf("1", "actual", default).ShouldBeOneOf(ReadOnlySpan<string>.Empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf("1", "actual", default).ShouldBeOneOf(["2"]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf("1", "actual", default).ShouldBeOneOf([1]));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(c1, "actual", default).ShouldBeOneOf(ReadOnlySpan<Class>.Empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(c1, "actual", default).ShouldBeOneOf(["2"]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(c1, "actual", default).ShouldBeOneOf([1]));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(s1, "actual", default).ShouldBeOneOf(ReadOnlySpan<Struct>.Empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(s1, "actual", default).ShouldBeOneOf(["2"]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(s1, "actual", default).ShouldBeOneOf([1]));
    }

    [Fact]
    public void ShouldNotBeOneOf_NotFail()
    {
        var c1 = new Class(1);

        var s1 = new Struct(1);

        new ObjectShouldBeOneOf(1, "actual", default).ShouldNotBeOneOf(ReadOnlySpan<int>.Empty);
        new ObjectShouldBeOneOf(1, "actual", default).ShouldNotBeOneOf([2]);
        new ObjectShouldBeOneOf(1, "actual", default).ShouldNotBeOneOf(["1"]);

        new ObjectShouldBeOneOf("1", "actual", default).ShouldNotBeOneOf(ReadOnlySpan<string>.Empty);
        new ObjectShouldBeOneOf("1", "actual", default).ShouldNotBeOneOf(["2"]);
        new ObjectShouldBeOneOf("1", "actual", default).ShouldNotBeOneOf([1]);

        new ObjectShouldBeOneOf(c1, "actual", default).ShouldNotBeOneOf(ReadOnlySpan<Class>.Empty);
        new ObjectShouldBeOneOf(c1, "actual", default).ShouldNotBeOneOf(["2"]);
        new ObjectShouldBeOneOf(c1, "actual", default).ShouldNotBeOneOf([1]);

        new ObjectShouldBeOneOf(s1, "actual", default).ShouldNotBeOneOf(ReadOnlySpan<Struct>.Empty);
        new ObjectShouldBeOneOf(s1, "actual", default).ShouldNotBeOneOf(["2"]);
        new ObjectShouldBeOneOf(s1, "actual", default).ShouldNotBeOneOf([1]);
    }

    [Fact]
    public void ShouldNotBeOneOf_Fail()
    {
        var c1 = new Class(1);
        var c2 = new Class(2);

        var s1 = new Struct(1);
        var s2 = new Struct(2);

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(1, "actual", default).ShouldNotBeOneOf([1]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(1, "actual", default).ShouldNotBeOneOf([2, 1]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(1, "actual", default).ShouldNotBeOneOf<object>(["1", c1, s1, 1]));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf("1", "actual", default).ShouldNotBeOneOf(["1"]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf("1", "actual", default).ShouldNotBeOneOf(["2", "1"]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf("1", "actual", default).ShouldNotBeOneOf<object>(["1", c1, s1, 1]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf("abcd", "actual", default).ShouldNotBeOneOf(["2", "ABCD"], StringComparer.OrdinalIgnoreCase));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(c1, "actual", default).ShouldNotBeOneOf([c1]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(c1, "actual", default).ShouldNotBeOneOf([c2, c1]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(c1, "actual", default).ShouldNotBeOneOf<object>(["1", c1, s1, 1]));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(s1, "actual", default).ShouldNotBeOneOf([s1]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(s1, "actual", default).ShouldNotBeOneOf([s2, s1]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOneOf(s1, "actual", default).ShouldNotBeOneOf<object>(["1", c1, s1, 1]));
    }
}
