using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ReadOnlySpanShouldEqualTests
{
    [Fact]
    public void ShouldEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal([1]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal([1, 2, 3]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal([1, 3]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal([2, 1]));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal([1], ignoreOrder: true));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal([1, 2, 3], ignoreOrder: true));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal([1, 3], ignoreOrder: true));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal([1, 2, 2], ignoreOrder: true));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<string>)["1", "2"]).Should().Equal(["1", "3"], comparer: StringComparer.InvariantCultureIgnoreCase));
    }

    [Fact]
    public void ShouldEqual_NotFail()
    {
        Array.Empty<int>().Should().Equal([]);
        Array.Empty<int>().Should().Equal([], ignoreOrder: true);

        ((ReadOnlySpan<int>)[1, 2]).Should().Equal([1, 2]);
        ((ReadOnlySpan<int>)[1, 2, 3]).Should().Equal([2, 3, 1], ignoreOrder: true);
        ((ReadOnlySpan<string>)["abc", "DEF"]).Should().Equal(["ABC", "def"], comparer: StringComparer.OrdinalIgnoreCase);
        ((ReadOnlySpan<string>)["abc", "DEF"]).Should().Equal(["def", "ABC"], ignoreOrder: true, comparer: StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void ShouldNotEqual_Fail()
    {
        ((ReadOnlySpan<int>)[1, 2]).Should().NotEqual([1]);
        ((ReadOnlySpan<int>)[1, 2]).Should().NotEqual([1, 2, 3]);
        ((ReadOnlySpan<int>)[1, 2]).Should().NotEqual([1, 3]);
        ((ReadOnlySpan<int>)[1, 2]).Should().NotEqual([2, 1]);

        ((ReadOnlySpan<int>)[1, 2]).Should().NotEqual([1], ignoreOrder: true);
        ((ReadOnlySpan<int>)[1, 2]).Should().NotEqual([1, 2, 3], ignoreOrder: true);
        ((ReadOnlySpan<int>)[1, 2]).Should().NotEqual([1, 2, 2], ignoreOrder: true);
        
        ((ReadOnlySpan<string>)["1", "2"]).Should().NotEqual(["1", "3"], comparer: StringComparer.InvariantCultureIgnoreCase);
    }

    [Fact]
    public void ShouldNotEqual_NotFail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Array.Empty<int>().Should().NotEqual([]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Array.Empty<int>().Should().NotEqual([], ignoreOrder: true));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2]).Should().NotEqual([1, 2]));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<int>)[1, 2, 3]).Should().NotEqual([2, 3, 1], ignoreOrder: true));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<string>)["abc", "DEF"]).Should().NotEqual(["ABC", "def"], comparer: StringComparer.OrdinalIgnoreCase));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((ReadOnlySpan<string>)["abc", "DEF"]).Should().NotEqual(["def", "ABC"], ignoreOrder: true, comparer: StringComparer.OrdinalIgnoreCase));
    }
}
