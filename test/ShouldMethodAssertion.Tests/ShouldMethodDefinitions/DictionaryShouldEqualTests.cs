using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.DictionaryShouldEqual<TypeArg, TypeArg>)])]
public class DictionaryShouldEqualTests
{
    [Fact]
    public void ShouldEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldEqual([(1, "1"), (2, "2"), (3, "3")])
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldEqual([(1, "1")])
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldEqual([(1, "1"), (2, "1")])
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldEqual([(1, "1"), (1, "2")])
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldEqual([new KeyValuePair<int, string>(1, "1"), new KeyValuePair<int, string>(1, "2")])
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<string, string>(new Dictionary<string, string> { { "asdf", "asdf" } }, "actual", default).ShouldEqual([("ASDF", "asdf")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<string, string>(new Dictionary<string, string> { { "asdf", "asdf" } }, "actual", default).ShouldEqual([new KeyValuePair<string, string>("ASDF", "asdf")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldEqual([(1, "asdfd")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );
    }

    [Fact]
    public void ShouldEqual_NotFail()
    {
        new DictionaryShouldEqual<int, string>(new Dictionary<int, string>(), "actual", default).ShouldEqual(Array.Empty<(int, string)>());
        new DictionaryShouldEqual<int, string>(new Dictionary<int, string>(), "actual", default).ShouldEqual(Array.Empty<KeyValuePair<int,string>>());

        new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldEqual([(1, "1"), (2, "2")]);
        new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldEqual([new KeyValuePair<int, string>(1, "1"), new KeyValuePair<int, string>(2, "2")]);

        new DictionaryShouldEqual<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldEqual([(1, "ASDF")], valueComparer: StringComparer.InvariantCultureIgnoreCase);
        new DictionaryShouldEqual<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldEqual([new KeyValuePair<int, string>(1, "ASDF")], valueComparer: StringComparer.InvariantCultureIgnoreCase);
    }

    [Fact]
    public void ShouldNotEqual_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new Dictionary<int, string>(), "actual", default).ShouldNotEqual(Array.Empty<(int, string)>())
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new Dictionary<int, string>(), "actual", default).ShouldNotEqual(Array.Empty<KeyValuePair<int, string>>())
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldNotEqual([(1, "1"), (2, "2")])
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldNotEqual([new KeyValuePair<int, string>(1, "1"), new KeyValuePair<int, string>(2, "2")])
            );

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldNotEqual([(1, "ASDF")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(
            () => new DictionaryShouldEqual<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldNotEqual([new KeyValuePair<int, string>(1, "ASDF")], valueComparer: StringComparer.InvariantCultureIgnoreCase)
            );
    }

    [Fact]
    public void ShouldNotEqual_NotFail()
    {
        new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldNotEqual([(1, "1"), (2, "2"), (3, "3")]);
        new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldNotEqual([(1, "1")]);
        new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldNotEqual([(1, "1"), (2, "1")]);
        new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldNotEqual([(1, "1"), (1, "2")]);

        new DictionaryShouldEqual<int, string>(new[] { 1, 2 }.ToDictionary(v => v, v => $"{v}"), "actual", default).ShouldNotEqual([new KeyValuePair<int, string>(1, "1"), new KeyValuePair<int, string>(1, "2")]);

        new DictionaryShouldEqual<string, string>(new Dictionary<string, string> { { "asdf", "asdf" } }, "actual", default).ShouldNotEqual([("ASDF", "asdf")], valueComparer: StringComparer.InvariantCultureIgnoreCase);
        new DictionaryShouldEqual<string, string>(new Dictionary<string, string> { { "asdf", "asdf" } }, "actual", default).ShouldNotEqual([new KeyValuePair<string, string>("ASDF", "asdf")], valueComparer: StringComparer.InvariantCultureIgnoreCase);

        new DictionaryShouldEqual<int, string>(new Dictionary<int, string> { { 1, "asdf" } }, "actual", default).ShouldNotEqual([(1, "asdfd")], valueComparer: StringComparer.InvariantCultureIgnoreCase);
    }
}
