using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.EnumerableShouldEqual<TypeArg>)])]
public class EnumerableShouldEqualTests
{
    [Fact]
    public void ShouldEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().Equal([1]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().Equal([1, 2, 3]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().Equal([1, 3]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().Equal([2, 1]));

		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().Equal([1],       ignoreOrder: true));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().Equal([1, 2, 3], ignoreOrder: true));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().Equal([1, 3],    ignoreOrder: true));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().Equal([1, 2, 2], ignoreOrder: true));

		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { "1", "2" }.Should().Equal(["1", "3"], comparer: StringComparer.InvariantCultureIgnoreCase));
	}

	[Fact]
	public void ShouldEqual_NotFail()
	{
		Array.Empty<int>().Should().Equal([]);
		Array.Empty<int>().Should().Equal([], ignoreOrder: true);

		new[] { 1, 2 }.Should().Equal([1, 2]);
		new[] { 1, 2, 3 }.Should().Equal([2, 3, 1], ignoreOrder: true);
		new[] { "abc", "DEF" }.Should().Equal(["ABC", "def"], comparer: StringComparer.OrdinalIgnoreCase);
		new[] { "abc", "DEF" }.Should().Equal(["def", "ABC"], ignoreOrder: true, comparer: StringComparer.OrdinalIgnoreCase);
	}

	[Fact]
	public void ShouldNotEqual_Fail()
	{
		new[] { 1, 2 }.Should().NotEqual([1]);
		new[] { 1, 2 }.Should().NotEqual([1, 2, 3]);
		new[] { 1, 2 }.Should().NotEqual([1, 3]);
		new[] { 1, 2 }.Should().NotEqual([2, 1]);

		new[] { 1, 2 }.Should().NotEqual([1],       ignoreOrder: true);
		new[] { 1, 2 }.Should().NotEqual([1, 2, 3], ignoreOrder: true);
		new[] { 1, 2 }.Should().NotEqual([1, 2, 2], ignoreOrder: true);

		new[] { "1", "2" }.Should().NotEqual(["1", "3"], comparer: StringComparer.InvariantCultureIgnoreCase);
	}

	[Fact]
	public void ShouldNotEqual_NotFail()
	{
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Array.Empty<int>().Should().NotEqual([]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Array.Empty<int>().Should().NotEqual([], ignoreOrder: true));

		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2 }.Should().NotEqual([1, 2]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { 1, 2, 3 }.Should().NotEqual([2, 3, 1], ignoreOrder: true));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { "abc", "DEF" }.Should().NotEqual(["ABC", "def"], comparer: StringComparer.OrdinalIgnoreCase));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new[] { "abc", "DEF" }.Should().NotEqual(["def", "ABC"], ignoreOrder: true, comparer: StringComparer.OrdinalIgnoreCase));
	}
}
