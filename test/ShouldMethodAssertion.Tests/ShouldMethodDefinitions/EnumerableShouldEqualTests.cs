using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class EnumerableShouldEqualTests
{
	EnumerableShouldEqual<int>.__ParameterExpressions paramExpDefaultInt = new()
    {
		expected = "expected",
	};

    EnumerableShouldEqual<string>.__ParameterExpressions paramExpDefaultString = new()
    {
        expected = "expected",
    };

    [Fact]
    public void ShouldEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([1]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([1, 2, 3]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([1, 3]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([2, 1]));

		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([1],       ignoreOrder: true));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([1, 2, 3], ignoreOrder: true));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([1, 3],    ignoreOrder: true));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([1, 2, 2], ignoreOrder: true));

		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<string>(["1", "2"], "actual", paramExpDefaultString).ShouldEqual(["1", "3"], comparer: StringComparer.InvariantCultureIgnoreCase));
	}

	[Fact]
	public void ShouldEqual_NotFail()
	{
		new EnumerableShouldEqual<int>([], "actual", paramExpDefaultInt).ShouldEqual([]);
        new EnumerableShouldEqual<int>([], "actual", paramExpDefaultInt).ShouldEqual([], ignoreOrder: true);

		new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldEqual([1, 2]);
		new EnumerableShouldEqual<int>([1, 2, 3], "actual", paramExpDefaultInt).ShouldEqual([2, 3, 1], ignoreOrder: true);
		new EnumerableShouldEqual<string>(["abc", "DEF"], "actual", paramExpDefaultString).ShouldEqual(["ABC", "def"], comparer: StringComparer.OrdinalIgnoreCase);
		new EnumerableShouldEqual<string>(["abc", "DEF"], "actual", paramExpDefaultString).ShouldEqual(["def", "ABC"], ignoreOrder: true, comparer: StringComparer.OrdinalIgnoreCase);
	}

	[Fact]
	public void ShouldNotEqual_Fail()
	{
		new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldNotEqual([1]);
		new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldNotEqual([1, 2, 3]);
		new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldNotEqual([1, 3]);
		new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldNotEqual([2, 1]);

		new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldNotEqual([1],       ignoreOrder: true);
		new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldNotEqual([1, 2, 3], ignoreOrder: true);
		new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldNotEqual([1, 2, 2], ignoreOrder: true);

		new EnumerableShouldEqual<string>(["1", "2"], "actual", paramExpDefaultString).ShouldNotEqual(["1", "3"], comparer: StringComparer.InvariantCultureIgnoreCase);
	}

	[Fact]
	public void ShouldNotEqual_NotFail()
	{
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([], "actual", paramExpDefaultInt).ShouldNotEqual([]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([], "actual", paramExpDefaultInt).ShouldNotEqual([], ignoreOrder: true));

		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2], "actual", paramExpDefaultInt).ShouldNotEqual([1, 2]));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<int>([1, 2, 3], "actual", paramExpDefaultInt).ShouldNotEqual([2, 3, 1], ignoreOrder: true));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<string>(["abc", "DEF"], "actual", paramExpDefaultString).ShouldNotEqual(["ABC", "def"], comparer: StringComparer.OrdinalIgnoreCase));
		Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new EnumerableShouldEqual<string>(["abc", "DEF"], "actual", paramExpDefaultString).ShouldNotEqual(["def", "ABC"], ignoreOrder: true, comparer: StringComparer.OrdinalIgnoreCase));
	}
}
