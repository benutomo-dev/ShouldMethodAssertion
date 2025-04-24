using ShouldMethodAssertion.ShouldExtensions;
using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.Tests;

public class UnitTest1
{
    struct StructA
    {
        public int value;
    }

    private static void X()
    {
    }

    private static async ValueTask<int> XAsync()
    {
        await Task.CompletedTask.ConfigureAwait(true);
        throw new Exception();
    }

    [Fact]
    public async Task Test1()
    {
        Invoke.That(() => { }).Should().NotThrow();


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

        

        1.Should().Be(1, EqualityComparer<int>.Default);

        //"banana".Should().Be("");

        new Dictionary<string, int>
        {
            ["apple"] = 1,
            ["banana"] = 2,
        }.Should().Equal([("apple", 1), ("banana", 2)]);

        MethodImplOptions.Synchronized.Should().Be(MethodImplOptions.Synchronized);

        (MethodImplOptions.PreserveSig | MethodImplOptions.Synchronized).Should().HaveFlag(MethodImplOptions.Synchronized);

        ((int?)0).Should().HaveValue();

        0.Should().BeAssignableTo<object>();

        default(int?).Should().NotHaveValue();
        default(Guid?).Should().NotHaveValue();

        default(StructA).Should().BeDefault();
        new StructA{ value = 1}.Should().NotBeDefault();

        var guid = Guid.NewGuid();
        guid.Should().NotBeDefault();
        default(Guid).Should().BeDefault();

        var exception1 = new Action(() => throw new FileNotFoundException("hogehoge")).Should().Throw<IOException>(includeDerivedType: true);
        exception1.Message.Should().Be("hogehoge");

        var exception2 = await InvokeAsync.That(async () =>
        {
            await Task.Delay(1).ConfigureAwait(true);
            throw new FileNotFoundException("fugafuga");
        }).Should().ThrowAsync<IOException>(includeDerivedType: true).ConfigureAwait(true);
        exception2.Message.Should().Be("fugafuga");

        "asdf".ShouldSatisfy(v => v.Length == 4);

        "asdf".Should().BeOneOf(["23456", "457", "asdf", "srt"]);

        int? nullInt = null;
        nullInt.Should().NotHaveValue();

        int? nullableInt = 1;
        nullableInt.Should().HaveValue();
        nullableInt.Value.Should().BeGreaterThan(0);
        nullableInt.Should().BeAssignableTo<int>();

        new StructA().Should().Be(new StructA());

        var x = nullableInt.Value;
    }
}
