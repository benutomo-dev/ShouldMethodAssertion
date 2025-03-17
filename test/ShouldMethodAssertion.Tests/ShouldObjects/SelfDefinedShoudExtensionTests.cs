using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;
using System.Collections;

namespace ShouldMethodAssertion.Tests.ShouldObjects;

public partial class SelfDefinedShoudExtensionTests
{
#pragma warning disable CA1036 // 比較可能な型でメソッドをオーバーライドします
    public sealed record class MixIn(int Value) : IEnumerable<int>, IComparable<MixIn>
#pragma warning restore CA1036 // 比較可能な型でメソッドをオーバーライドします
    {
        public int CompareTo(MixIn? other) => other is null ? -1 : Value.CompareTo(other.Value);

        public IEnumerator<int> GetEnumerator() => new int[] { Value }.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => new int[] { Value }.AsEnumerable().GetEnumerator();
    }

    [ShouldExtension(typeof(MixIn))]
    [ShouldMethod(typeof(ObjectShouldBeOneOf))]
    [ShouldMethod(typeof(EnumerableShouldEquals<>), TypeArgs = [typeof(int)])]
    [ShouldMethod(typeof(ComparableShouldCompare<>), TypeArgs = [typeof(MixIn)])]
    [ShouldMethod(typeof(CustumShouldMethodDefine))]
    public partial struct ShouldMixIn
    {
    }

    [ShouldMethodDefinition(typeof(MixIn))]
    public partial struct CustumShouldMethodDefine
    {
        public void ShouldFailIfZero()
        {
            if (Context.Actual.Value == 0)
                throw AssertExceptionUtil.Create("custum defined assert");
        }
    }

    [Fact]
    public void PreparedShould_Fail()
    {
        var m1 = new MixIn(1);
        var m2 = new MixIn(2);

        Assert.ThrowsAny<Exception>(() => m1.Should().BeOneOf([m2]));
        Assert.ThrowsAny<Exception>(() => new [] { m1, m2 }.Should().Equal([m1, m1], ignoreOrder: true));
        Assert.ThrowsAny<Exception>(() => m2.Should().LessThan(m1));
    }

    [Fact]
    public void PreparedShould_NotFail()
    {
        var m1 = new MixIn(1);
        var m2 = new MixIn(2);

        m1.Should().BeOneOf([m2, m1]);
        new[] { m1, m2 }.Should().Equal([m2, m1], ignoreOrder: true);
        m1.Should().LessThan(m2);
    }

    [Fact]
    public void UserDefinedShould_Fail()
    {
        Assert.ThrowsAny<Exception>(() => new MixIn(0).Should().FailIfZero());
    }

    [Fact]
    public void UserDefinedShould_NotFail()
    {
        new MixIn(1).Should().FailIfZero();
    }
}
