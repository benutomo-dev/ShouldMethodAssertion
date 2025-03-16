using ShouldMethodAssertion.ShouldExtensions;
using Xunit.Sdk;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldBeTests
{
    [Fact]
    public void ShouldBe_NotFail()
    {
        1.Should().Be(1);
        "asdf".Should().Be("asdf");
        "asdf".Should().Be("ASDF", StringComparer.OrdinalIgnoreCase);

        ((object)1).Should().Be(1);
        ((int?)1).Should().Be(1);
        1.Should().Be((object)1);
        ((int?)1).Should().Be((object)1);
        1.Should().Be((int?)1);
        ((object)1).Should().Be((int?)1);

        default(int?).Should().Be((object?)null);
        ((object?)null).Should().Be(default(int?));

        ((object)"asdf").Should().Be("asdf");
        "asdf".Should().Be((object)"asdf");

        default(object).Should().Be(default(object));
    }

    [Fact]
    public void ShouldBe_Fail()
    {
        Assert.ThrowsAny<Exception>(() => 1.Should().Be(2));
        Assert.ThrowsAny<Exception>(() => "asdf".Should().Be("ASDF"));
        Assert.ThrowsAny<Exception>(() => "asdf".Should().Be("ASDFC", StringComparer.OrdinalIgnoreCase));

        Assert.ThrowsAny<Exception>(() => ((object)1).Should().Be(2));
        Assert.ThrowsAny<Exception>(() => 1.Should().Be((object)2));
        Assert.ThrowsAny<Exception>(() => ((int?)1).Should().Be((object)2));
        Assert.ThrowsAny<Exception>(() => 1.Should().Be((int?)2));
        Assert.ThrowsAny<Exception>(() => ((object)1).Should().Be((int?)2));
    }

    [Fact]
    public void ShouldNotBe_NotFail()
    {
        1.Should().NotBe(2);
        "asdf".Should().NotBe("aSdf");
        "asdf".Should().NotBe("ASDFC", StringComparer.OrdinalIgnoreCase);

        ((object)1).Should().NotBe(2);
        ((int?)1).Should().NotBe(2);
        1.Should().NotBe((object)2);
        ((int?)1).Should().NotBe((object)2);
        1.Should().NotBe((int?)2);
        ((object)1).Should().NotBe((int?)2);
    }

    [Fact]
    public void ShouldNotBe_Fail()
    {
        Assert.ThrowsAny<Exception>(() => 1.Should().NotBe(1));
        Assert.ThrowsAny<Exception>(() => "asdf".Should().NotBe("asdf"));
        Assert.ThrowsAny<Exception>(() => "asdf".Should().NotBe("ASDF", StringComparer.OrdinalIgnoreCase));

        Assert.ThrowsAny<Exception>(() => ((object)1).Should().NotBe(1));
        Assert.ThrowsAny<Exception>(() => 1.Should().NotBe((object)1));
        Assert.ThrowsAny<Exception>(() => ((int?)1).Should().NotBe((object)1));
        Assert.ThrowsAny<Exception>(() => 1.Should().NotBe((int?)1));
        Assert.ThrowsAny<Exception>(() => ((object)1).Should().NotBe((int?)1));

        Assert.ThrowsAny<Exception>(() => ((object)"asdf").Should().NotBe("asdf"));
        Assert.ThrowsAny<Exception>(() => "asdf".Should().NotBe((object)"asdf"));

        Assert.ThrowsAny<Exception>(() => default(int?).Should().NotBe((object?)null));
        Assert.ThrowsAny<Exception>(() => ((object?)null).Should().NotBe(default(int?)));
    }
}
