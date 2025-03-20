using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.NullableStructShouldBe<StructTypeArg>)])]
public class NullableStructShouldBeTests
{
    [Fact]
    public void ShouldBe_NotFail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        empty.Should().Be((Guid?)empty);
        guid.Should().Be((Guid?)guid);

        empty.Should().Be((Guid?)guid, AlwaysEqual<Guid>.Default);
        guid.Should().Be((Guid?)empty, AlwaysEqual<Guid>.Default);
    }

    [Fact]
    public void ShouldBe_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => empty.Should().Be((Guid?)guid));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().Be((Guid?)empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => empty.Should().Be(default(Guid?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().Be(default(Guid?)));
    }

    [Fact]
    public void ShouldNotBe_NotFail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        empty.Should().NotBe((Guid?)guid);
        guid.Should().NotBe((Guid?)empty);

        empty.Should().NotBe(default(Guid?));
        guid.Should().NotBe(default(Guid?));
    }

    [Fact]
    public void ShouldNotBe_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => empty.Should().NotBe((Guid?)empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().NotBe((Guid?)guid));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => empty.Should().NotBe((Guid?)guid, AlwaysEqual<Guid>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().NotBe((Guid?)empty, AlwaysEqual<Guid>.Default));
    }
}
