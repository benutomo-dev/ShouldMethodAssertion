using ShouldMethodAssertion.ShouldMethodDefinitions;

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
        new ComparableShouldCompare<int>(1, "actual", default).ShouldBeLessThan(2);
        new ComparableShouldCompare<int>(1, "actual", default).ShouldBeLessThan(2, Comparer<int>.Default);
        new ComparableShouldCompare<int>(1, "actual", default).ShouldBeGreaterThan(2, new ReverseComparer<int>(Comparer<int>.Default));

        new ComparableShouldCompare<int>(2, "actual", default).ShouldBeGreaterThan(1);
        new ComparableShouldCompare<int>(2, "actual", default).ShouldBeGreaterThan(1, Comparer<int>.Default);
        new ComparableShouldCompare<int>(2, "actual", default).ShouldBeLessThan(1, new ReverseComparer<int>(Comparer<int>.Default));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    public void ShouldLessOrGreater_Fail(int smallOrEqual, int bigOrEqual)
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(smallOrEqual, "actual", default).ShouldBeGreaterThan(bigOrEqual));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(smallOrEqual, "actual", default).ShouldBeGreaterThan(bigOrEqual, Comparer<int>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(smallOrEqual, "actual", default).ShouldBeLessThan(bigOrEqual, new ReverseComparer<int>(Comparer<int>.Default)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(bigOrEqual, "actual", default).ShouldBeLessThan(smallOrEqual));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(bigOrEqual, "actual", default).ShouldBeLessThan(smallOrEqual, Comparer<int>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(bigOrEqual, "actual", default).ShouldBeGreaterThan(smallOrEqual, new ReverseComparer<int>(Comparer<int>.Default)));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    public void ShouldEqualLessOrGreaterWithinEqual_NotFail(int smallOrEqual, int bigOrEqual)
    {
        new ComparableShouldCompare<int>(smallOrEqual, "actual", default).ShouldBeLessThanOrEqual(bigOrEqual);
        new ComparableShouldCompare<int>(smallOrEqual, "actual", default).ShouldBeLessThanOrEqual(bigOrEqual, Comparer<int>.Default);
        new ComparableShouldCompare<int>(smallOrEqual, "actual", default).ShouldBeGreaterThanOrEqual(bigOrEqual, new ReverseComparer<int>(Comparer<int>.Default));

        new ComparableShouldCompare<int>(bigOrEqual, "actual", default).ShouldBeGreaterThanOrEqual(smallOrEqual);
        new ComparableShouldCompare<int>(bigOrEqual, "actual", default).ShouldBeGreaterThanOrEqual(smallOrEqual, Comparer<int>.Default);
        new ComparableShouldCompare<int>(bigOrEqual, "actual", default).ShouldBeLessThanOrEqual(smallOrEqual, new ReverseComparer<int>(Comparer<int>.Default));
    }

    [Fact]
    public void ShouldEqualLessOrGreaterWithinEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(1, "actual", default).ShouldBeGreaterThanOrEqual(2));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(1, "actual", default).ShouldBeGreaterThanOrEqual(2, Comparer<int>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(1, "actual", default).ShouldBeLessThanOrEqual(2, new ReverseComparer<int>(Comparer<int>.Default)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(2, "actual", default).ShouldBeLessThanOrEqual(1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(2, "actual", default).ShouldBeLessThanOrEqual(1, Comparer<int>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ComparableShouldCompare<int>(2, "actual", default).ShouldBeGreaterThanOrEqual(1, new ReverseComparer<int>(Comparer<int>.Default)));
    }
}
