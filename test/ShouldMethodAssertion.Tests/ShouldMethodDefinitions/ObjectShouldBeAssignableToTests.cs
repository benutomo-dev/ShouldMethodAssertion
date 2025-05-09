using ShouldMethodAssertion.ShouldMethodDefinitions;
using System.ComponentModel;
using System.Runtime.Serialization;

#pragma warning disable CA2263 // 型が既知の場合はジェネリック オーバーロードを優先する

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldBeAssignableToTests
{
    [Fact]
    public void ShouldBeAssignableToT_NotFail()
    {
        new ObjectShouldBeAssignableTo(default(object), "actual", default).ShouldBeAssignableTo<object>();
        new ObjectShouldBeAssignableTo(default(Exception), "actual", default).ShouldBeAssignableTo<object>();
        new ObjectShouldBeAssignableTo(default(object), "actual", default).ShouldBeAssignableTo<Exception>();

        new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo<int>();
        new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo<int?>();
        new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo<IComparable<int>>();
        new ObjectShouldBeAssignableTo(((object)1), "actual", default).ShouldBeAssignableTo<int>();
        new ObjectShouldBeAssignableTo(((int?)1), "actual", default).ShouldBeAssignableTo<int>();

        new ObjectShouldBeAssignableTo(new Exception(), "actual", default).ShouldBeAssignableTo<Exception>();
        new ObjectShouldBeAssignableTo(new Exception(), "actual", default).ShouldBeAssignableTo<ISerializable>();
    }

    [Fact]
    public void ShouldBeAssignableToT_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(new object(), "actual", default).ShouldBeAssignableTo<int>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(new object(), "actual", default).ShouldBeAssignableTo<int?>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(new object(), "actual", default).ShouldBeAssignableTo<string>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(new object(), "actual", default).ShouldBeAssignableTo<INotifyPropertyChanged>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo<string>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo<INotifyPropertyChanged>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(((int?)1), "actual", default).ShouldBeAssignableTo<string>());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(((int?)1), "actual", default).ShouldBeAssignableTo<INotifyPropertyChanged>());
    }

    [Fact]
    public void ShouldBeAssignableTo_NotFail()
    {
        new ObjectShouldBeAssignableTo(default(object), "actual", default).ShouldBeAssignableTo(typeof(object));
        new ObjectShouldBeAssignableTo(default(Exception), "actual", default).ShouldBeAssignableTo(typeof(object));
        new ObjectShouldBeAssignableTo(default(object), "actual", default).ShouldBeAssignableTo(typeof(Exception));

        new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo(typeof(int));
        new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo(typeof(int?));
        new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo(typeof(IComparable<int>));
        new ObjectShouldBeAssignableTo(((object)1), "actual", default).ShouldBeAssignableTo(typeof(int));
        new ObjectShouldBeAssignableTo(((int?)1), "actual", default).ShouldBeAssignableTo(typeof(int));

        new ObjectShouldBeAssignableTo(new Exception(), "actual", default).ShouldBeAssignableTo(typeof(Exception));
        new ObjectShouldBeAssignableTo(new Exception(), "actual", default).ShouldBeAssignableTo(typeof(ISerializable));
    }

    [Fact]
    public void ShouldBeAssignableTo_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(new object(), "actual", default).ShouldBeAssignableTo(typeof(int)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(new object(), "actual", default).ShouldBeAssignableTo(typeof(int?)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(new object(), "actual", default).ShouldBeAssignableTo(typeof(string)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(new object(), "actual", default).ShouldBeAssignableTo(typeof(INotifyPropertyChanged)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo(typeof(string)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(1, "actual", default).ShouldBeAssignableTo(typeof(INotifyPropertyChanged)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(((int?)1), "actual", default).ShouldBeAssignableTo(typeof(string)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new ObjectShouldBeAssignableTo(((int?)1), "actual", default).ShouldBeAssignableTo(typeof(INotifyPropertyChanged)));
    }
}
