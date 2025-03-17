using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldSameReferenceAsTests
{
    [Fact]
    public unsafe void ShouldBe_NotFail()
    {
        string str1;
        fixed (char* ptr = "asdf")
        {
            str1 = new string(ptr);
        }

        var obj = new object();

        var num = (object)1;

        obj.Should().SameReferenceAs(obj);
        str1.Should().SameReferenceAs(str1);
        ((object)str1).Should().SameReferenceAs(str1);
        str1.Should().SameReferenceAs(((object)str1));
        num.Should().SameReferenceAs(num);
    }

    [Fact]
    public unsafe void ShouldBe_Fail()
    {
        string str1, str2;
        fixed (char* ptr = "asdf")
        {
            str1 = new string(ptr);
            str2 = new string(ptr);
        }

        var obj = new object();

        var num = (object)1;

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => str1.Should().SameReferenceAs(str2));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => str1.Should().SameReferenceAs(obj));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().SameReferenceAs(num));
    }

    [Fact]
    public unsafe void ShouldNotBe_NotFail()
    {
        string str1, str2;
        fixed (char* ptr = "asdf")
        {
            str1 = new string(ptr);
            str2 = new string(ptr);
        }

        var obj = new object();

        var num = (object)1;

        str1.Should().NotSameReferenceAs(str2);
        str1.Should().NotSameReferenceAs(obj);
        1.Should().NotSameReferenceAs(num);
    }

    [Fact]
    public unsafe void ShouldNotBe_Fail()
    {
        string str1;
        fixed (char* ptr = "asdf")
        {
            str1 = new string(ptr);
        }

        var obj = new object();

        var num = (object)1;

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => obj.Should().NotSameReferenceAs(obj));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => str1.Should().NotSameReferenceAs(str1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((object)str1).Should().NotSameReferenceAs(str1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => str1.Should().NotSameReferenceAs(((object)str1)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => num.Should().NotSameReferenceAs(num));
    }
}
