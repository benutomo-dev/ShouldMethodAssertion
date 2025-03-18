using ShouldMethodAssertion.ShouldExtensions;
using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.Tests;

public class UnitTest1
{
    struct StructA
    {
        public int value;
    }

    [Fact]
    public async Task Test1()
    {
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

        (2.0).Should().BeLessThan(3);

        //Guid.NewGuid().Should().Be(Guid.NewGuid());
        
        1.Should().Be(1);

        //"banana".Should().Be("");

        new Dictionary<string, int>
        {
            ["apple"] = 1,
            ["banana"] = 2,
        }.Should().Equal([("apple", 1), ("banana", 2)]);

        MethodImplOptions.Synchronized.Should().Be(MethodImplOptions.Synchronized);

        (MethodImplOptions.PreserveSig | MethodImplOptions.Synchronized).Should().HaveFlag(MethodImplOptions.Synchronized);

        ((int?)0).Should().NotBeNull();

        0.Should().BeAssignableTo<object>();

        default(int?).Should().BeNull();
        default(Guid?).Should().BeNull();

        default(StructA).Should().BeDefault();
        new StructA{ value = 1}.Should().NotBeDefault();

        var guid = Guid.NewGuid();
        guid.Should().NotBeDefault();
        default(Guid).Should().BeDefault();

        var exception1 = new Action(() => throw new FileNotFoundException("hogehoge")).Should().Throw<IOException>(includeDerivedType: true);
        exception1.Message.Should().Be("hogehoge");

        var exception2 = await new Func<Task>(async () =>
        {
            await Task.Delay(1).ConfigureAwait(true);
            throw new FileNotFoundException("fugafuga");
        }).Should().ThrowAsync<IOException>(includeDerivedType: true).ConfigureAwait(true);
        exception2.Message.Should().Be("fugafuga");

        "asdf".ShouldSatisfy(v => v.Length == 4);

        "asdf".Should().BeOneOf(["23456", "457", "asdf", "srt"]);

        int? nullInt = null;
        nullInt.Should().BeNull();

        int? nullableInt = 1;
        nullableInt.Should().NotBeNull();
        nullableInt.Value.Should().BeGreaterThan(0);
        nullableInt.Should().BeAssignableTo<int>();



        var x = nullableInt.Value;
    }
}
