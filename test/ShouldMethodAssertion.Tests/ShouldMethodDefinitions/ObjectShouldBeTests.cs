using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldBeTests
{
    [Fact]
    public void ShouldBe_NotFail()
    {
        new ObjectShouldBe<int>(1, "actual", default).ShouldBe(1);
        new ObjectShouldBe<string>("asdf", "actual", default).ShouldBe("asdf");
        new ObjectShouldBe<string>("asdf", "actual", default).ShouldBe("ASDF", StringComparer.OrdinalIgnoreCase);

        new ObjectShouldBe<int>(((object)1), "actual", default).ShouldBe(1);
        new ObjectShouldBe<int?>(1, "actual", default).ShouldBe((int?)1);
        new ObjectShouldBe<int?>(((object)1), "actual", default).ShouldBe((int?)1);

        new ObjectShouldBe<int?>(((object?)null), "actual", default).ShouldBe(default(int?));

        new ObjectShouldBe<string>(((object)"asdf"), "actual", default).ShouldBe("asdf");

        new ObjectShouldBe<object>(default(object), "actual", default).ShouldBe(default(object));
    }

    [Fact]
    public void ShouldBe_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int>(1, "actual", default).ShouldBe(2));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<string>("asdf", "actual", default).ShouldBe("ASDF"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<string>("asdf", "actual", default).ShouldBe("ASDFC", StringComparer.OrdinalIgnoreCase));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int>(((object)1), "actual", default).ShouldBe(2));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int?>(1, "actual", default).ShouldBe((int?)2));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int?>(((object)1), "actual", default).ShouldBe((int?)2));
    }

    [Fact]
    public void ShouldNotBe_NotFail()
    {
        new ObjectShouldBe<int>(1, "actual", default).ShouldNotBe(2);
        new ObjectShouldBe<string>("asdf", "actual", default).ShouldNotBe("aSdf");
        new ObjectShouldBe<string>("asdf", "actual", default).ShouldNotBe("ASDFC", StringComparer.OrdinalIgnoreCase);

        new ObjectShouldBe<int>(((object)1), "actual", default).ShouldNotBe(2);
        new ObjectShouldBe<int?>(1, "actual", default).ShouldNotBe((int?)2);
        new ObjectShouldBe<int?>(((object)1), "actual", default).ShouldNotBe((int?)2);
    }

    [Fact]
    public void ShouldNotBe_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int>(1, "actual", default).ShouldNotBe(1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<string>("asdf", "actual", default).ShouldNotBe("asdf"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<string>("asdf", "actual", default).ShouldNotBe("ASDF", StringComparer.OrdinalIgnoreCase));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int>(((object)1), "actual", default).ShouldNotBe(1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int?>(1, "actual", default).ShouldNotBe((int?)1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int?>(((object)1), "actual", default).ShouldNotBe((int?)1));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<string>(((object)"asdf"), "actual", default).ShouldNotBe("asdf"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBe<int?>(((object?)null), "actual", default).ShouldNotBe(default(int?)));
    }
}
