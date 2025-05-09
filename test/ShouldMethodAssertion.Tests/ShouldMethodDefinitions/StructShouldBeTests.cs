using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class StructShouldBeTests
{
    [Fact]
    public void ShouldBe_NotFail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        new StructShouldBe<Guid>(empty, "actual", default).ShouldBe(empty);
        new StructShouldBe<Guid>(guid, "actual", default).ShouldBe(guid);

        new StructShouldBe<Guid>(empty, "actual", default).ShouldBe(guid, AlwaysEqual<Guid>.Default);
        new StructShouldBe<Guid>(guid, "actual", default).ShouldBe(empty, AlwaysEqual<Guid>.Default);
    }

    [Fact]
    public void ShouldBe_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StructShouldBe<Guid>(empty, "actual", default).ShouldBe(guid));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StructShouldBe<Guid>(guid, "actual", default).ShouldBe(empty));
    }

    [Fact]
    public void ShouldNotBe_NotFail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        new StructShouldBe<Guid>(empty, "actual", default).ShouldNotBe(guid);
        new StructShouldBe<Guid>(guid, "actual", default).ShouldNotBe(empty);
    }

    [Fact]
    public void ShouldNotBe_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StructShouldBe<Guid>(empty, "actual", default).ShouldNotBe(empty));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StructShouldBe<Guid>(guid, "actual", default).ShouldNotBe(guid));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StructShouldBe<Guid>(empty, "actual", default).ShouldNotBe(guid, AlwaysEqual<Guid>.Default));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StructShouldBe<Guid>(guid, "actual", default).ShouldNotBe(empty, AlwaysEqual<Guid>.Default));
    }
}
