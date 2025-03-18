using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ObjectShouldBeDefaultTests
{
    private struct RawStruct(int value)
    {
        public int Value { get; } = value;
    }

    [Fact]
    public void ShouldBeDefault_NotFail()
    {
        default(int).Should().BeDefault();
        default(RawStruct).Should().BeDefault();

#pragma warning disable SMAssertion0003 // Inappropriate use of BeDefault on reference types (including Nullable<T>)
        default(object).Should().BeDefault();
#pragma warning restore SMAssertion0003 // Inappropriate use of BeDefault on reference types (including Nullable<T>)
    }

    [Fact]
    public void ShouldBeDefault_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new RawStruct(1).Should().BeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 1.Should().BeDefault());


#pragma warning disable SMAssertion0003 // Inappropriate use of BeDefault on reference types (including Nullable<T>)
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new object().Should().BeDefault()); // 否nullの参照型ならば常に否default
#pragma warning restore SMAssertion0003 // Inappropriate use of BeDefault on reference types (including Nullable<T>)
    }

    [Fact]
    public void ShouldNotBeDefault_NotFail()
    {
        new RawStruct(1).Should().NotBeDefault();
        1.Should().NotBeDefault();

#pragma warning disable SMAssertion0004 // Inappropriate use of BeNotDefault on reference types (including Nullable<T>)
        new object().Should().NotBeDefault(); // 否nullの参照型ならば常に否default
#pragma warning restore SMAssertion0004 // Inappropriate use of BeNotDefault on reference types (including Nullable<T>)
    }

    [Fact]
    public void ShouldNotBeDefault_Fail()
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(int).Should().NotBeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => 0.Should().NotBeDefault()); // intの0はdefault(int)と等価
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(RawStruct).Should().NotBeDefault());
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new RawStruct(0).Should().NotBeDefault()); // new RawStruct(0)はdefault(RawStruct)と等価


#pragma warning disable SMAssertion0004 // Inappropriate use of BeNotDefault on reference types (including Nullable<T>)
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => default(object).Should().NotBeDefault());
#pragma warning restore SMAssertion0004 // Inappropriate use of BeNotDefault on reference types (including Nullable<T>)
    }
}
