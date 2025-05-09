using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class StringShouldMatchTests
{
    [Fact]
    public void ShouldMatch_NotFail()
    {
        new StringShouldMatch("", "actual", default).ShouldMatch("");
        new StringShouldMatch("", "actual", default).ShouldMatch("*");
        new StringShouldMatch("", "actual", default).ShouldMatch("**");
        new StringShouldMatch("", "actual", default).ShouldMatch("***");

        new StringShouldMatch("\r\n", "actual", default).ShouldMatch("\r\n");
        new StringShouldMatch("\n", "actual", default).ShouldMatch("\n");
        new StringShouldMatch("\r\n", "actual", default).ShouldMatch("\n");
        new StringShouldMatch("\n", "actual", default).ShouldMatch("\r\n");

        new StringShouldMatch("123abc", "actual", default).ShouldMatch("123abc");
        new StringShouldMatch("123abc", "actual", default).ShouldMatch("123?bc");
        new StringShouldMatch("123abc", "actual", default).ShouldMatch("??????");

        new StringShouldMatch("123abc", "actual", default).ShouldMatch("123abc", ignoreCase: false);

        new StringShouldMatch("123abc", "actual", default).ShouldMatch("123abc", ignoreCase: true);
        new StringShouldMatch("123abc", "actual", default).ShouldMatch("123Abc", ignoreCase: true);

        new StringShouldMatch("123\r\nabc", "actual", default).ShouldMatch("123\r\nabc");
        new StringShouldMatch("123\nabc", "actual", default).ShouldMatch("123\nabc");
        new StringShouldMatch("123\r\nabc", "actual", default).ShouldMatch("123\nabc");
        new StringShouldMatch("123\nabc", "actual", default).ShouldMatch("123\r\nabc");

        new StringShouldMatch("123abc", "actual", default).ShouldMatch("*abc");

        new StringShouldMatch("123\r\nabc", "actual", default).ShouldMatch("**abc");
        new StringShouldMatch("123\r\n\r\nabc", "actual", default).ShouldMatch("**abc");
        new StringShouldMatch("123\nabc", "actual", default).ShouldMatch("**abc");
        new StringShouldMatch("123\n\nabc", "actual", default).ShouldMatch("**abc");
        new StringShouldMatch("\r\nabc", "actual", default).ShouldMatch("**abc");
        new StringShouldMatch("\r\n\r\nabc", "actual", default).ShouldMatch("**abc");
        new StringShouldMatch("\nabc", "actual", default).ShouldMatch("**abc");
        new StringShouldMatch("\n\nabc", "actual", default).ShouldMatch("**abc");

        new StringShouldMatch("123abc", "actual", default).ShouldMatch("123*");

        new StringShouldMatch("123\r\nabc", "actual", default).ShouldMatch("123**");
        new StringShouldMatch("123\r\n\r\nabc", "actual", default).ShouldMatch("123**");
        new StringShouldMatch("123\nabc", "actual", default).ShouldMatch("123**");
        new StringShouldMatch("123\n\nabc", "actual", default).ShouldMatch("123**");
        new StringShouldMatch("123\r\n", "actual", default).ShouldMatch("123**");
        new StringShouldMatch("123\r\n\r\n", "actual", default).ShouldMatch("123**");
        new StringShouldMatch("123\n", "actual", default).ShouldMatch("123**");
        new StringShouldMatch("123\n\n", "actual", default).ShouldMatch("123**");

        new StringShouldMatch("123", "actual", default).ShouldMatch("*123*");
        new StringShouldMatch("abc123abc", "actual", default).ShouldMatch("*123*");

        new StringShouldMatch("123", "actual", default).ShouldMatch("**123**");
        new StringShouldMatch("\n123\n", "actual", default).ShouldMatch("**123**");
        new StringShouldMatch("abc\n123\nabc", "actual", default).ShouldMatch("**123**");

        new StringShouldMatch("123123", "actual", default).ShouldMatch("123*123");
        new StringShouldMatch("123abc123", "actual", default).ShouldMatch("123*123");

        new StringShouldMatch("123123", "actual", default).ShouldMatch("123**123");
        new StringShouldMatch("123abc123", "actual", default).ShouldMatch("123**123");
        new StringShouldMatch("123\r\n123", "actual", default).ShouldMatch("123**123");
        new StringShouldMatch("123\r\nabc\r\n123", "actual", default).ShouldMatch("123**123");

        new StringShouldMatch("123123123", "actual", default).ShouldMatch("*123");
    }

    [Fact]
    public void ShouldMatch_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("", "actual", default).ShouldMatch("1"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("", "actual", default).ShouldMatch("?"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("1", "actual", default).ShouldMatch(""));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("1", "actual", default).ShouldMatch("11"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("1", "actual", default).ShouldMatch("??"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("11", "actual", default).ShouldMatch("1"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("11", "actual", default).ShouldMatch("?"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("11", "actual", default).ShouldMatch("111"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("11", "actual", default).ShouldMatch("???"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\n", "actual", default).ShouldMatch("*"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\r\n", "actual", default).ShouldMatch("*"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("1\n\n1", "actual", default).ShouldMatch("*\n*"));
    }

    [Fact]
    public void ShouldNotMatch_NotFail()
    {
        new StringShouldMatch("", "actual", default).ShouldNotMatch("1");
        new StringShouldMatch("", "actual", default).ShouldNotMatch("?");
        new StringShouldMatch("1", "actual", default).ShouldNotMatch("");
        new StringShouldMatch("1", "actual", default).ShouldNotMatch("11");
        new StringShouldMatch("1", "actual", default).ShouldNotMatch("??");
        new StringShouldMatch("11", "actual", default).ShouldNotMatch("1");
        new StringShouldMatch("11", "actual", default).ShouldNotMatch("?");
        new StringShouldMatch("11", "actual", default).ShouldNotMatch("111");
        new StringShouldMatch("11", "actual", default).ShouldNotMatch("???");

        new StringShouldMatch("\n", "actual", default).ShouldNotMatch("*");
        new StringShouldMatch("\r\n", "actual", default).ShouldNotMatch("*");

        new StringShouldMatch("1\n\n1", "actual", default).ShouldNotMatch("*\n*");
    }

    [Fact]
    public void ShouldNotMatch_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("", "actual", default).ShouldNotMatch(""));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("", "actual", default).ShouldNotMatch("*"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("", "actual", default).ShouldNotMatch("**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("", "actual", default).ShouldNotMatch("***"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\r\n", "actual", default).ShouldNotMatch("\r\n"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\n", "actual", default).ShouldNotMatch("\n"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\r\n", "actual", default).ShouldNotMatch("\n"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\n", "actual", default).ShouldNotMatch("\r\n"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc", "actual", default).ShouldNotMatch("123abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc", "actual", default).ShouldNotMatch("123?bc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc", "actual", default).ShouldNotMatch("??????"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc", "actual", default).ShouldNotMatch("123abc", ignoreCase: false));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc", "actual", default).ShouldNotMatch("123abc", ignoreCase: true));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc", "actual", default).ShouldNotMatch("123Abc", ignoreCase: true));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\nabc", "actual", default).ShouldNotMatch("123\r\nabc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\nabc", "actual", default).ShouldNotMatch("123\nabc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\nabc", "actual", default).ShouldNotMatch("123\nabc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\nabc", "actual", default).ShouldNotMatch("123\r\nabc"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc", "actual", default).ShouldNotMatch("*abc"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\nabc", "actual", default).ShouldNotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\n\r\nabc", "actual", default).ShouldNotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\nabc", "actual", default).ShouldNotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\n\nabc", "actual", default).ShouldNotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\r\nabc", "actual", default).ShouldNotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\r\n\r\nabc", "actual", default).ShouldNotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\nabc", "actual", default).ShouldNotMatch("**abc"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\n\nabc", "actual", default).ShouldNotMatch("**abc"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc", "actual", default).ShouldNotMatch("123*"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\nabc", "actual", default).ShouldNotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\n\r\nabc", "actual", default).ShouldNotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\nabc", "actual", default).ShouldNotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\n\nabc", "actual", default).ShouldNotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\n", "actual", default).ShouldNotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\n\r\n", "actual", default).ShouldNotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\n", "actual", default).ShouldNotMatch("123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\n\n", "actual", default).ShouldNotMatch("123**"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123", "actual", default).ShouldNotMatch("*123*"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("abc123abc", "actual", default).ShouldNotMatch("*123*"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123", "actual", default).ShouldNotMatch("**123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("\n123\n", "actual", default).ShouldNotMatch("**123**"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("abc\n123\nabc", "actual", default).ShouldNotMatch("**123**"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123123", "actual", default).ShouldNotMatch("123*123"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc123", "actual", default).ShouldNotMatch("123*123"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123123", "actual", default).ShouldNotMatch("123**123"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123abc123", "actual", default).ShouldNotMatch("123**123"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\n123", "actual", default).ShouldNotMatch("123**123"));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123\r\nabc\r\n123", "actual", default).ShouldNotMatch("123**123"));

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new StringShouldMatch("123123123", "actual", default).ShouldNotMatch("*123"));
    }
}
