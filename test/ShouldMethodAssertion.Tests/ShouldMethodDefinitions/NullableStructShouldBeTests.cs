using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class NullableStructShouldBeTests
{
    [Fact]
    public void ShouldBe_NotFail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        new NullableStructShouldBe<Guid>(empty, "actual", default).ShouldBe((Guid?)empty);
        new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldBe((Guid?)guid);

        new NullableStructShouldBe<Guid>(empty, "actual", default).ShouldBe((Guid?)guid, AlwaysEqual<Guid>.Default);
        new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldBe((Guid?)empty, AlwaysEqual<Guid>.Default);

        new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldBe((Guid?)empty);
        new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe((Guid?)guid);

        new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldBe((Guid?)guid, AlwaysEqual<Guid>.Default);
        new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe((Guid?)empty, AlwaysEqual<Guid>.Default);

        new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldBe(empty);
        new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe(guid);

        new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldBe(guid, AlwaysEqual<Guid>.Default);
        new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe(empty, AlwaysEqual<Guid>.Default);

        new NullableStructShouldBe<Guid>((default(Guid?)), "actual", default).ShouldBe(null);
    }

    [Fact]
    public void ShouldBe_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(empty, "actual", default).ShouldBe((Guid?)guid));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldBe((Guid?)empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(empty, "actual", default).ShouldBe(default(Guid?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldBe(default(Guid?)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldBe((Guid?)guid));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe((Guid?)empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldBe(default(Guid?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe(default(Guid?)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldBe(guid));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe(empty));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe(default(Guid?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldBe(default(Guid?)));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldBe(null));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldBe(null));
    }

    [Fact]
    public void ShouldNotBe_NotFail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        new NullableStructShouldBe<Guid>(empty, "actual", default).ShouldNotBe((Guid?)guid);
        new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldNotBe((Guid?)empty);

        new NullableStructShouldBe<Guid>(empty, "actual", default).ShouldNotBe(default(Guid?));
        new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldNotBe(default(Guid?));

        new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldNotBe((Guid?)guid);
        new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldNotBe((Guid?)empty);

        new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldNotBe(default(Guid?));
        new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldNotBe(default(Guid?));

        new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldNotBe(null);
        new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldNotBe(null);
    }

    [Fact]
    public void ShouldNotBe_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(empty, "actual", default).ShouldNotBe((Guid?)empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldNotBe((Guid?)guid));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(empty, "actual", default).ShouldNotBe((Guid?)guid, AlwaysEqual<Guid>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(guid, "actual", default).ShouldNotBe((Guid?)empty, AlwaysEqual<Guid>.Default));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldNotBe((Guid?)empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldNotBe((Guid?)guid));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldNotBe((Guid?)guid, AlwaysEqual<Guid>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldNotBe((Guid?)empty, AlwaysEqual<Guid>.Default));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldNotBe(empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldNotBe(guid));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)empty), "actual", default).ShouldNotBe(guid, AlwaysEqual<Guid>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>(((Guid?)guid), "actual", default).ShouldNotBe(empty, AlwaysEqual<Guid>.Default));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>((default(Guid?)), "actual", default).ShouldNotBe(default(Guid?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new NullableStructShouldBe<Guid>((default(Guid?)), "actual", default).ShouldNotBe(null));
    }
}
