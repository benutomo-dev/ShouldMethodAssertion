using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.StringShouldMatch)])]
public class StringShouldMatchTests
{
    [Fact]
    public void ShouldMatch_NotFail()
    {
        "".Should().Match("");
        "".Should().Match("*");
        "".Should().Match("**");
        "".Should().Match("***");

        "\r\n".Should().Match("\r\n");
        "\n".Should().Match("\n");
        "\r\n".Should().Match("\n");
        "\n".Should().Match("\r\n");

        "123abc".Should().Match("123abc");
        "123abc".Should().Match("123?bc");
        "123abc".Should().Match("??????");

        "123abc".Should().Match("123abc", ignoreCase: false);

        "123abc".Should().Match("123abc", ignoreCase: true);
        "123abc".Should().Match("123Abc", ignoreCase: true);

        "123\r\nabc".Should().Match("123\r\nabc");
        "123\nabc".Should().Match("123\nabc");
        "123\r\nabc".Should().Match("123\nabc");
        "123\nabc".Should().Match("123\r\nabc");

        "123abc".Should().Match("*abc");

        "123\r\nabc".Should().Match("**abc");
        "123\r\n\r\nabc".Should().Match("**abc");
        "123\nabc".Should().Match("**abc");
        "123\n\nabc".Should().Match("**abc");
        "\r\nabc".Should().Match("**abc");
        "\r\n\r\nabc".Should().Match("**abc");
        "\nabc".Should().Match("**abc");
        "\n\nabc".Should().Match("**abc");

        "123abc".Should().Match("123*");

        "123\r\nabc".Should().Match("123**");
        "123\r\n\r\nabc".Should().Match("123**");
        "123\nabc".Should().Match("123**");
        "123\n\nabc".Should().Match("123**");
        "123\r\n".Should().Match("123**");
        "123\r\n\r\n".Should().Match("123**");
        "123\n".Should().Match("123**");
        "123\n\n".Should().Match("123**");

        "123".Should().Match("*123*");
        "abc123abc".Should().Match("*123*");

        "123".Should().Match("**123**");
        "\n123\n".Should().Match("**123**");
        "abc\n123\nabc".Should().Match("**123**");

        "123123".Should().Match("123*123");
        "123abc123".Should().Match("123*123");

        "123123".Should().Match("123**123");
        "123abc123".Should().Match("123**123");
        "123\r\n123".Should().Match("123**123");
        "123\r\nabc\r\n123".Should().Match("123**123");

        "123123123".Should().Match("*123");
    }

    [Fact]
    public void ShouldMatch_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "".Should().Match("1"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "".Should().Match("?"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "1".Should().Match(""));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "1".Should().Match("11"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "1".Should().Match("??"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "11".Should().Match("1"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "11".Should().Match("?"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "11".Should().Match("111"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "11".Should().Match("???"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\n".Should().Match("*"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\r\n".Should().Match("*"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "1\n\n1".Should().Match("*\n*"));
    }

    [Fact]
    public void ShouldNotMatch_NotFail()
    {
        "".Should().NotMatch("1");
        "".Should().NotMatch("?");
        "1".Should().NotMatch("");
        "1".Should().NotMatch("11");
        "1".Should().NotMatch("??");
        "11".Should().NotMatch("1");
        "11".Should().NotMatch("?");
        "11".Should().NotMatch("111");
        "11".Should().NotMatch("???");

        "\n".Should().NotMatch("*");
        "\r\n".Should().NotMatch("*");

        "1\n\n1".Should().NotMatch("*\n*");
    }

    [Fact]
    public void ShouldNotMatch_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "".Should().NotMatch(""));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "".Should().NotMatch("*"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "".Should().NotMatch("**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "".Should().NotMatch("***"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\r\n".Should().NotMatch("\r\n"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\n".Should().NotMatch("\n"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\r\n".Should().NotMatch("\n"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\n".Should().NotMatch("\r\n"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc".Should().NotMatch("123abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc".Should().NotMatch("123?bc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc".Should().NotMatch("??????"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc".Should().NotMatch("123abc", ignoreCase: false));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc".Should().NotMatch("123abc", ignoreCase: true));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc".Should().NotMatch("123Abc", ignoreCase: true));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\nabc".Should().NotMatch("123\r\nabc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\nabc".Should().NotMatch("123\nabc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\nabc".Should().NotMatch("123\nabc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\nabc".Should().NotMatch("123\r\nabc"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc".Should().NotMatch("*abc"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\nabc".Should().NotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\n\r\nabc".Should().NotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\nabc".Should().NotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\n\nabc".Should().NotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\r\nabc".Should().NotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\r\n\r\nabc".Should().NotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\nabc".Should().NotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\n\nabc".Should().NotMatch("**abc"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc".Should().NotMatch("123*"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\nabc".Should().NotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\n\r\nabc".Should().NotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\nabc".Should().NotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\n\nabc".Should().NotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\n".Should().NotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\n\r\n".Should().NotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\n".Should().NotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\n\n".Should().NotMatch("123**"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123".Should().NotMatch("*123*"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "abc123abc".Should().NotMatch("*123*"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123".Should().NotMatch("**123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "\n123\n".Should().NotMatch("**123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "abc\n123\nabc".Should().NotMatch("**123**"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123123".Should().NotMatch("123*123"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc123".Should().NotMatch("123*123"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123123".Should().NotMatch("123**123"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123abc123".Should().NotMatch("123**123"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\n123".Should().NotMatch("123**123"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123\r\nabc\r\n123".Should().NotMatch("123**123"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => "123123123".Should().NotMatch("*123"));
    }
}
