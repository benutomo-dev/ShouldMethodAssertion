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

        ((Guid?)empty).Should().Be((Guid?)empty);
        ((Guid?)guid).Should().Be((Guid?)guid);

        ((Guid?)empty).Should().Be((Guid?)guid, AlwaysEqual<Guid>.Default);
        ((Guid?)guid).Should().Be((Guid?)empty, AlwaysEqual<Guid>.Default);

        ((Guid?)empty).Should().Be(empty);
        ((Guid?)guid).Should().Be(guid);

        ((Guid?)empty).Should().Be(guid, AlwaysEqual<Guid>.Default);
        ((Guid?)guid).Should().Be(empty, AlwaysEqual<Guid>.Default);

        (default(Guid?)).Should().Be(null);
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

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)empty).Should().Be((Guid?)guid));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().Be((Guid?)empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)empty).Should().Be(default(Guid?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().Be(default(Guid?)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)empty).Should().Be(guid));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().Be(empty));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().Be(default(Guid?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().Be(default(Guid?)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().Be(null));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => guid.Should().Be(null));
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

        ((Guid?)empty).Should().NotBe((Guid?)guid);
        ((Guid?)guid).Should().NotBe((Guid?)empty);

        ((Guid?)empty).Should().NotBe(default(Guid?));
        ((Guid?)guid).Should().NotBe(default(Guid?));

        ((Guid?)empty).Should().NotBe(null);
        ((Guid?)guid).Should().NotBe(null);
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

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)empty).Should().NotBe((Guid?)empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().NotBe((Guid?)guid));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)empty).Should().NotBe((Guid?)guid, AlwaysEqual<Guid>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().NotBe((Guid?)empty, AlwaysEqual<Guid>.Default));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)empty).Should().NotBe(empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().NotBe(guid));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)empty).Should().NotBe(guid, AlwaysEqual<Guid>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((Guid?)guid).Should().NotBe(empty, AlwaysEqual<Guid>.Default));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => (default(Guid?)).Should().NotBe(default(Guid?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => (default(Guid?)).Should().NotBe(null));
    }
}
