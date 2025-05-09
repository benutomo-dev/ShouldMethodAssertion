using ShouldMethodAssertion.ShouldMethodDefinitions;

#pragma warning disable CA2263 // 型が既知の場合はジェネリック オーバーロードを優先する

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldBeOfTypeTests
{
    [Fact]
    public void ShouldBeOfTypeT_NotFail()
    {
        new ObjectShouldBeOfType(new object(), "actual", default).ShouldBeOfType<object>();
        new ObjectShouldBeOfType(Array.Empty<string>(), "actual", default).ShouldBeOfType<string[]>();
        new ObjectShouldBeOfType(new FileNotFoundException(), "actual", default).ShouldBeOfType<FileNotFoundException>();
        new ObjectShouldBeOfType(default(int), "actual", default).ShouldBeOfType<int>();
        new ObjectShouldBeOfType(default(long), "actual", default).ShouldBeOfType<long>();
    }

    [Fact]
    public void ShouldBeOfTypeT_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(Array.Empty<int>(), "actual", default).ShouldBeOfType<IEnumerable<int>>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(default(int), "actual", default).ShouldBeOfType<long>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(default(long), "actual", default).ShouldBeOfType<int>());
    }

    [Fact]
    public void ShouldBeOfType_NotFail()
    {
        new ObjectShouldBeOfType(new object(), "actual", default).ShouldBeOfType(typeof(object));
        new ObjectShouldBeOfType(Array.Empty<string>(), "actual", default).ShouldBeOfType(typeof(string[]));
        new ObjectShouldBeOfType(new FileNotFoundException(), "actual", default).ShouldBeOfType(typeof(FileNotFoundException));
        new ObjectShouldBeOfType(default(int), "actual", default).ShouldBeOfType(typeof(int));
        new ObjectShouldBeOfType(default(long), "actual", default).ShouldBeOfType(typeof(long));
    }

    [Fact]
    public void ShouldBeOfType_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(Array.Empty<int>(), "actual", default).ShouldBeOfType(typeof(IEnumerable<int>)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(default(int), "actual", default).ShouldBeOfType(typeof(long)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(default(long), "actual", default).ShouldBeOfType(typeof(int)));
    }


    [Fact]
    public void ShouldNotBeOfTypeT_NotFail()
    {
        new ObjectShouldBeOfType(Array.Empty<string>(), "actual", default).ShouldNotBeOfType<IEnumerable<string>>();
        new ObjectShouldBeOfType(new FileNotFoundException(), "actual", default).ShouldNotBeOfType<IOException>();
    }

    [Fact]
    public void ShouldNotBeOfTypeT_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(Array.Empty<int>(), "actual", default).ShouldNotBeOfType<int[]>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(default(int), "actual", default).ShouldNotBeOfType<int>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(default(long), "actual", default).ShouldNotBeOfType<long>());
    }

    [Fact]
    public void ShouldNotBeOfType_NotFail()
    {
        new ObjectShouldBeOfType(Array.Empty<string>(), "actual", default).ShouldNotBeOfType(typeof(IEnumerable<string>));
        new ObjectShouldBeOfType(new FileNotFoundException(), "actual", default).ShouldNotBeOfType(typeof(IOException));
    }

    [Fact]
    public void ShouldNotBeOfType_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(Array.Empty<int>(), "actual", default).ShouldNotBeOfType(typeof(int[])));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(default(int), "actual", default).ShouldNotBeOfType(typeof(int)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeOfType(default(long), "actual", default).ShouldNotBeOfType(typeof(long)));
    }
}
