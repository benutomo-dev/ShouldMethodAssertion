using ShouldMethodAssertion.ShouldMethodDefinitions;

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

        new ObjectShouldSameReferenceAs(obj, "actual", default).ShouldSameReferenceAs(obj);
        new ObjectShouldSameReferenceAs(str1, "actual", default).ShouldSameReferenceAs(str1);
        new ObjectShouldSameReferenceAs(((object)str1), "actual", default).ShouldSameReferenceAs(str1);
        new ObjectShouldSameReferenceAs(str1, "actual", default).ShouldSameReferenceAs(((object)str1));
        new ObjectShouldSameReferenceAs(num, "actual", default).ShouldSameReferenceAs(num);
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

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldSameReferenceAs(str1, "actual", default).ShouldSameReferenceAs(str2));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldSameReferenceAs(str1, "actual", default).ShouldSameReferenceAs(obj));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldSameReferenceAs(1, "actual", default).ShouldSameReferenceAs(num));
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

        new ObjectShouldSameReferenceAs(str1, "actual", default).ShouldNotSameReferenceAs(str2);
        new ObjectShouldSameReferenceAs(str1, "actual", default).ShouldNotSameReferenceAs(obj);
        new ObjectShouldSameReferenceAs(1, "actual", default).ShouldNotSameReferenceAs(num);
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

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldSameReferenceAs(obj, "actual", default).ShouldNotSameReferenceAs(obj));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldSameReferenceAs(str1, "actual", default).ShouldNotSameReferenceAs(str1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldSameReferenceAs(((object)str1), "actual", default).ShouldNotSameReferenceAs(str1));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldSameReferenceAs(str1, "actual", default).ShouldNotSameReferenceAs(((object)str1)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldSameReferenceAs(num, "actual", default).ShouldNotSameReferenceAs(num));
    }
}
