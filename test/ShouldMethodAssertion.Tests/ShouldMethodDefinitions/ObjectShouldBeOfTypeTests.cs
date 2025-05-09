using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.ObjectShouldBeOfType)])]
public class ObjectShouldBeOfTypeTests
{
    [Fact]
    public void ShouldBeOfTypeT_NotFail()
    {
        new object().Should().BeOfType<object>();
        Array.Empty<string>().Should().BeOfType<string[]>();
        new FileNotFoundException().Should().BeOfType<FileNotFoundException>();
        default(int).Should().BeOfType<int>();
        default(long).Should().BeOfType<long>();
    }

    [Fact]
    public void ShouldBeOfTypeT_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => (default(object)).Should().BeOfType<object>() );
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => (default(string[])).Should().BeOfType<string[]>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Array.Empty<int>().Should().BeOfType<IEnumerable<int>>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int).Should().BeOfType<long>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(long).Should().BeOfType<int>());
    }

    [Fact]
    public void ShouldBeOfType_NotFail()
    {
        new object().Should().BeOfType(typeof(object));
        Array.Empty<string>().Should().BeOfType(typeof(string[]));
        new FileNotFoundException().Should().BeOfType(typeof(FileNotFoundException));
        default(int).Should().BeOfType(typeof(int));
        default(long).Should().BeOfType(typeof(long));
    }

    [Fact]
    public void ShouldBeOfType_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => (default(object)).Should().BeOfType(typeof(object)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => (default(string[])).Should().BeOfType(typeof(string[])));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Array.Empty<int>().Should().BeOfType(typeof(IEnumerable<int>)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int).Should().BeOfType(typeof(long)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(long).Should().BeOfType(typeof(int)));
    }


    [Fact]
    public void ShouldNotBeOfTypeT_NotFail()
    {
        Array.Empty<string>().Should().NotBeOfType<IEnumerable<string>>();
        new FileNotFoundException().Should().NotBeOfType<IOException>();
    }

    [Fact]
    public void ShouldNotBeOfTypeT_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(object).Should().NotBeOfType<object>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Array.Empty<int>().Should().NotBeOfType<int[]>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int).Should().NotBeOfType<int>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(long).Should().NotBeOfType<long>());
    }

    [Fact]
    public void ShouldNotBeOfType_NotFail()
    {
        Array.Empty<string>().Should().NotBeOfType(typeof(IEnumerable<string>));
        new FileNotFoundException().Should().NotBeOfType(typeof(IOException));
    }

    [Fact]
    public void ShouldNotBeOfType_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(object).Should().NotBeOfType(typeof(object)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => Array.Empty<int>().Should().NotBeOfType(typeof(int[])));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int).Should().NotBeOfType(typeof(int)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(long).Should().NotBeOfType(typeof(long)));
    }
}
