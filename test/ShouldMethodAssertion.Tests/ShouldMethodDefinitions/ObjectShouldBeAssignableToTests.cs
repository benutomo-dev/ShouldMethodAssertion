using ShouldMethodAssertion.ShouldExtensions;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;
public class ObjectShouldBeAssignableToTests
{
    [Fact]
    public void ShouldBeAssignableToT_NotFail()
    {
        default(object).Should().BeAssignableTo<object>();
        default(Exception).Should().BeAssignableTo<object>();
        default(object).Should().BeAssignableTo<Exception>();

        1.Should().BeAssignableTo<int>();
        1.Should().BeAssignableTo<int?>();
        1.Should().BeAssignableTo<IComparable<int>>();
        ((object)1).Should().BeAssignableTo<int>();
        ((int?)1).Should().BeAssignableTo<int>();

        new Exception().Should().BeAssignableTo<Exception>();
        new Exception().Should().BeAssignableTo<ISerializable>();
        ((object)new Exception()).Should().BeAssignableTo<Exception>();
    }

    [Fact]
    public void ShouldBeAssignableToT_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeAssignableTo<int>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeAssignableTo<int?>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeAssignableTo<string>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeAssignableTo<INotifyPropertyChanged>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeAssignableTo<string>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeAssignableTo<INotifyPropertyChanged>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((int?)1).Should().BeAssignableTo<string>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((int?)1).Should().BeAssignableTo<INotifyPropertyChanged>());
    }
    [Fact]
    public void ShouldBeAssignableTo_NotFail()
    {
        default(object).Should().BeAssignableTo(typeof(object));
        default(Exception).Should().BeAssignableTo(typeof(object));
        default(object).Should().BeAssignableTo(typeof(Exception));

        1.Should().BeAssignableTo(typeof(int));
        1.Should().BeAssignableTo(typeof(int?));
        1.Should().BeAssignableTo(typeof(IComparable<int>));
        ((object)1).Should().BeAssignableTo(typeof(int));
        ((int?)1).Should().BeAssignableTo(typeof(int));

        new Exception().Should().BeAssignableTo(typeof(Exception));
        new Exception().Should().BeAssignableTo(typeof(ISerializable));
        ((object)new Exception()).Should().BeAssignableTo(typeof(Exception));
    }

    [Fact]
    public void ShouldBeAssignableTo_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeAssignableTo(typeof(int)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeAssignableTo(typeof(int?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeAssignableTo(typeof(string)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeAssignableTo(typeof(INotifyPropertyChanged)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeAssignableTo(typeof(string)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeAssignableTo(typeof(INotifyPropertyChanged)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((int?)1).Should().BeAssignableTo(typeof(string)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => ((int?)1).Should().BeAssignableTo(typeof(INotifyPropertyChanged)));
    }
}
