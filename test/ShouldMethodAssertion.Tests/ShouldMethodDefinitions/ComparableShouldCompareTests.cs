using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ComparableShouldCompareTests
{
    sealed class ReverseComparer<T> : IComparer<T>
    {
        private IComparer<T> _comparer;

        public ReverseComparer(IComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public int Compare(T? x, T? y) => -_comparer.Compare(x!, y!);
    }

    [Fact]
    public void ShouldEqualLessOrGreater_NotFail()
    {
        1.Should().BeLessThan(2);
        1.Should().BeLessThan(2, Comparer<int>.Default);
        1.Should().BeGreaterThan(2, new ReverseComparer<int>(Comparer<int>.Default));

        2.Should().BeGreaterThan(1);
        2.Should().BeGreaterThan(1, Comparer<int>.Default);
        2.Should().BeLessThan(1, new ReverseComparer<int>(Comparer<int>.Default));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    public void ShouldLessOrGreater_Fail(int smallOrEqual, int bigOrEqual)
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => smallOrEqual.Should().BeGreaterThan(bigOrEqual));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => smallOrEqual.Should().BeGreaterThan(bigOrEqual, Comparer<int>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => smallOrEqual.Should().BeLessThan(bigOrEqual, new ReverseComparer<int>(Comparer<int>.Default)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => bigOrEqual.Should().BeLessThan(smallOrEqual));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => bigOrEqual.Should().BeLessThan(smallOrEqual, Comparer<int>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => bigOrEqual.Should().BeGreaterThan(smallOrEqual, new ReverseComparer<int>(Comparer<int>.Default)));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    public void ShouldEqualLessOrGreaterWithinEqual_NotFail(int smallOrEqual, int bigOrEqual)
    {
        smallOrEqual.Should().BeLessThanOrEqual(bigOrEqual);
        smallOrEqual.Should().BeLessThanOrEqual(bigOrEqual, Comparer<int>.Default);
        smallOrEqual.Should().BeGreaterThanOrEqual(bigOrEqual, new ReverseComparer<int>(Comparer<int>.Default));

        bigOrEqual.Should().BeGreaterThanOrEqual(smallOrEqual);
        bigOrEqual.Should().BeGreaterThanOrEqual(smallOrEqual, Comparer<int>.Default);
        bigOrEqual.Should().BeLessThanOrEqual(smallOrEqual, new ReverseComparer<int>(Comparer<int>.Default));
    }

    [Fact]
    public void ShouldEqualLessOrGreaterWithinEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeGreaterThanOrEqual(2));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeGreaterThanOrEqual(2, Comparer<int>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeLessThanOrEqual(2, new ReverseComparer<int>(Comparer<int>.Default)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 2.Should().BeLessThanOrEqual(1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 2.Should().BeLessThanOrEqual(1, Comparer<int>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 2.Should().BeGreaterThanOrEqual(1, new ReverseComparer<int>(Comparer<int>.Default)));
    }
}
