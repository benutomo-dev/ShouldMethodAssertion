using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class DictionaryShouldEqualTests
{
    [Fact]
    public void ShouldEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().Equal([(1, "1"), (2, "2"), (3, "3")])
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().Equal([(1, "1")])
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().Equal([(1, "1"), (2, "1")])
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().Equal([(1, "1"), (1, "2")])
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().Equal([new KeyValuePair<int, string>(1, "1"), new KeyValuePair<int, string>(1, "2")])
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<string, string> { { "asdf", "asdf" } }.Should().Equal([("ASDF", "asdf")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<string, string> { { "asdf", "asdf" } }.Should().Equal([new KeyValuePair<string, string>("ASDF", "asdf")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "asdf" } }.Should().Equal([(1, "asdfd")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );
    }

    [Fact]
    public void ShouldEqual_NotFail()
    {
        new Dictionary<int, string>().Should().Equal(Array.Empty<(int, string)>());
        new Dictionary<int, string>().Should().Equal(Array.Empty<KeyValuePair<int,string>>());

        new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().Equal([(1, "1"), (2, "2")]);
        new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().Equal([new KeyValuePair<int, string>(1, "1"), new KeyValuePair<int, string>(2, "2")]);

        new Dictionary<int, string> { { 1, "asdf" } }.Should().Equal([(1, "ASDF")], valueComparer: StringComparer.InvariantCultureIgnoreCase);
        new Dictionary<int, string> { { 1, "asdf" } }.Should().Equal([new KeyValuePair<int, string>(1, "ASDF")], valueComparer: StringComparer.InvariantCultureIgnoreCase);
    }

    [Fact]
    public void ShouldNotEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string>().Should().NotEqual(Array.Empty<(int, string)>())
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string>().Should().NotEqual(Array.Empty<KeyValuePair<int, string>>())
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().NotEqual([(1, "1"), (2, "2")])
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().NotEqual([new KeyValuePair<int, string>(1, "1"), new KeyValuePair<int, string>(2, "2")])
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "asdf" } }.Should().NotEqual([(1, "ASDF")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new Dictionary<int, string> { { 1, "asdf" } }.Should().NotEqual([new KeyValuePair<int, string>(1, "ASDF")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );
    }

    [Fact]
    public void ShouldNotEqual_NotFail()
    {
        new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().NotEqual([(1, "1"), (2, "2"), (3, "3")]);
        new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().NotEqual([(1, "1")]);
        new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().NotEqual([(1, "1"), (2, "1")]);
        new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().NotEqual([(1, "1"), (1, "2")]);

        new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}").Should().NotEqual([new KeyValuePair<int, string>(1, "1"), new KeyValuePair<int, string>(1, "2")]);

        new Dictionary<string, string> { { "asdf", "asdf" } }.Should().NotEqual([("ASDF", "asdf")], valueComparer: StringComparer.InvariantCultureIgnoreCase);
        new Dictionary<string, string> { { "asdf", "asdf" } }.Should().NotEqual([new KeyValuePair<string, string>("ASDF", "asdf")], valueComparer: StringComparer.InvariantCultureIgnoreCase);

        new Dictionary<int, string> { { 1, "asdf" } }.Should().NotEqual([(1, "asdfd")], valueComparer: StringComparer.InvariantCultureIgnoreCase);
    }
}
