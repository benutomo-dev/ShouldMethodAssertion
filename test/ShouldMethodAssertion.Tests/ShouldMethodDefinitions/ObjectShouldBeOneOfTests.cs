using ShouldMethodAssertion.ShouldExtensions;
using System.Collections;

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

        1.Should().BeOneOf([1]);
        1.Should().BeOneOf([2,1]);
        1.Should().BeOneOf<object>(["1", c1, s1, 1]);

        "1".Should().BeOneOf(["1"]);
        "1".Should().BeOneOf(["2", "1"]);
        "1".Should().BeOneOf<object>(["1", c1, s1, 1]);
        "abcd".Should().BeOneOf(["2", "ABCD"], StringComparer.OrdinalIgnoreCase);

        c1.Should().BeOneOf([c1]);
        c1.Should().BeOneOf([c2, c1]);
        c1.Should().BeOneOf<object>(["1", c1, s1, 1]);

        s1.Should().BeOneOf([s1]);
        s1.Should().BeOneOf([s2, s1]);
        s1.Should().BeOneOf<object>(["1", c1, s1, 1]);
    }

    [Fact]
    public void ShouldBeOneOf_Fail()
    {
        var c1 = new Class(1);

        var s1 = new Struct(1);

        Assert.ThrowsAny<Exception>(() => 1.Should().BeOneOf(ReadOnlySpan<int>.Empty));
        Assert.ThrowsAny<Exception>(() => 1.Should().BeOneOf([2]));
        Assert.ThrowsAny<Exception>(() => 1.Should().BeOneOf(["1"]));

        Assert.ThrowsAny<Exception>(() => "1".Should().BeOneOf(ReadOnlySpan<string>.Empty));
        Assert.ThrowsAny<Exception>(() => "1".Should().BeOneOf(["2"]));
        Assert.ThrowsAny<Exception>(() => "1".Should().BeOneOf([1]));

        Assert.ThrowsAny<Exception>(() => c1.Should().BeOneOf(ReadOnlySpan<Class>.Empty));
        Assert.ThrowsAny<Exception>(() => c1.Should().BeOneOf(["2"]));
        Assert.ThrowsAny<Exception>(() => c1.Should().BeOneOf([1]));

        Assert.ThrowsAny<Exception>(() => s1.Should().BeOneOf(ReadOnlySpan<Struct>.Empty));
        Assert.ThrowsAny<Exception>(() => s1.Should().BeOneOf(["2"]));
        Assert.ThrowsAny<Exception>(() => s1.Should().BeOneOf([1]));
    }

    [Fact]
    public void ShouldNotBeOneOf_NotFail()
    {
        var c1 = new Class(1);

        var s1 = new Struct(1);

        1.Should().NotBeOneOf(ReadOnlySpan<int>.Empty);
        1.Should().NotBeOneOf([2]);
        1.Should().NotBeOneOf(["1"]);

        "1".Should().NotBeOneOf(ReadOnlySpan<string>.Empty);
        "1".Should().NotBeOneOf(["2"]);
        "1".Should().NotBeOneOf([1]);

        c1.Should().NotBeOneOf(ReadOnlySpan<Class>.Empty);
        c1.Should().NotBeOneOf(["2"]);
        c1.Should().NotBeOneOf([1]);

        s1.Should().NotBeOneOf(ReadOnlySpan<Struct>.Empty);
        s1.Should().NotBeOneOf(["2"]);
        s1.Should().NotBeOneOf([1]);
    }

    [Fact]
    public void ShouldNotBeOneOf_Fail()
    {
        var c1 = new Class(1);
        var c2 = new Class(2);

        var s1 = new Struct(1);
        var s2 = new Struct(2);

        Assert.ThrowsAny<Exception>(() => 1.Should().NotBeOneOf([1]));
        Assert.ThrowsAny<Exception>(() => 1.Should().NotBeOneOf([2, 1]));
        Assert.ThrowsAny<Exception>(() => 1.Should().NotBeOneOf<object>(["1", c1, s1, 1]));

        Assert.ThrowsAny<Exception>(() => "1".Should().NotBeOneOf(["1"]));
        Assert.ThrowsAny<Exception>(() => "1".Should().NotBeOneOf(["2", "1"]));
        Assert.ThrowsAny<Exception>(() => "1".Should().NotBeOneOf<object>(["1", c1, s1, 1]));
        Assert.ThrowsAny<Exception>(() => "abcd".Should().NotBeOneOf(["2", "ABCD"], StringComparer.OrdinalIgnoreCase));

        Assert.ThrowsAny<Exception>(() => c1.Should().NotBeOneOf([c1]));
        Assert.ThrowsAny<Exception>(() => c1.Should().NotBeOneOf([c2, c1]));
        Assert.ThrowsAny<Exception>(() => c1.Should().NotBeOneOf<object>(["1", c1, s1, 1]));

        Assert.ThrowsAny<Exception>(() => s1.Should().NotBeOneOf([s1]));
        Assert.ThrowsAny<Exception>(() => s1.Should().NotBeOneOf([s2, s1]));
        Assert.ThrowsAny<Exception>(() => s1.Should().NotBeOneOf<object>(["1", c1, s1, 1]));
    }
}
