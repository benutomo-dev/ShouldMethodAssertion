using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;
using System.Collections;
using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.Tests;

[ShouldExtension(typeof(UnitTest1.X))]
[ShouldMethod(typeof(EnumerableShouldEquals<int>))]
[ShouldMethod(typeof(ComparableShouldCompare<UnitTest1.X>))]
public partial struct XShould
{
}

public class UnitTest1
{
    public class X : IEnumerable<int>, IComparable<X>
    {
        private int _value;

        public X(int value)
        {
            _value = value;
        }

        public int CompareTo(X? other) => Comparer<int?>.Default.Compare(_value, other?._value);

        public IEnumerator<int> GetEnumerator() => new[] { _value }.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => new[] { _value }.AsEnumerable().GetEnumerator();
    }

    [Fact]
    public async Task Test1()
    {
        new X(0).AsEnumerable().Should().Equal([0], ignoreOrder: true);
        new X(0).AsEnumerable().Should().BeAssignableTo(typeof(IComparable<X>));

        new X(0).Should().Equal([0], ignoreOrder: true);
        new X(0).Should().GreaterThan(new X(-1));

        new[] {
            1,
            2,
            3
        }.Should().Equal([
            2,
            1,
            3
        ], ignoreOrder: true);

        //new int[1].Should().SameReferenceAs(new object());
        //new int[1].Should().Equal([]);

        //new int[1].AsSpan().Should().Equal(new int[2]);

        (2.0).Should().LessThan(3);

        //Guid.NewGuid().Should().Be(Guid.NewGuid());
        
        1.Should().Be(1);

        //"banana".Should().Be("");

        new Dictionary<string, int>
        {
            ["apple"] = 1,
            ["banana"] = 2,
        }.Should().Equal([new("apple", 1), new("banana", 2)]);

        MethodImplOptions.Synchronized.Should().Be(MethodImplOptions.Synchronized);

        (MethodImplOptions.PreserveSig | MethodImplOptions.Synchronized).Should().HaveFlag(MethodImplOptions.Synchronized);

        0.Should().NotBeNull();

        0.Should().BeAssignableTo<object>();

        default(int?).Should().BeNull();
        default(Guid?).Should().BeNull();

        //Guid.NewGuid().Should().Be(Guid.NewGuid());

        var exception1 = new Action(() => throw new FileNotFoundException("hogehoge")).Should().Throw<IOException>(includeDerivedType: true);
        exception1.Message.Should().Be("hogehoge");

        var exception2 = await new Func<Task>(async () =>
        {
            await Task.Delay(1).ConfigureAwait(true);
            throw new FileNotFoundException("fugafuga");
        }).Should().ThrowAsync<IOException>(includeDerivedType: true).ConfigureAwait(true);
        exception2.Message.Should().Be("fugafuga");
    }
}
