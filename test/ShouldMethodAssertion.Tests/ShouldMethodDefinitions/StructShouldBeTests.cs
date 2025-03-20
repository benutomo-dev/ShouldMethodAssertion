using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.StructShouldBe<StructTypeArg>)])]
public class StructShouldBeTests
{
    [Fact]
    public void ShouldBe_NotFail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        empty.Should().Be(empty);
        guid.Should().Be(guid);

        empty.Should().Be(guid, AlwaysEqual<Guid>.Default);
        guid.Should().Be(empty, AlwaysEqual<Guid>.Default);
    }

    [Fact]
    public void ShouldBe_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => empty.Should().Be(guid));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().Be(empty));
    }

    [Fact]
    public void ShouldNotBe_NotFail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        empty.Should().NotBe(guid);
        guid.Should().NotBe(empty);
    }

    [Fact]
    public void ShouldNotBe_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => empty.Should().NotBe(empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().NotBe(guid));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => empty.Should().NotBe(guid, AlwaysEqual<Guid>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().NotBe(empty, AlwaysEqual<Guid>.Default));
    }
}
