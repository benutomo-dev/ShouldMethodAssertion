using ShouldMethodAssertion.ShouldExtensions;
using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.Tests;

public class UnitTest1
{
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

        var guid = Guid.NewGuid();
        guid.Should().Be(guid);

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
    }
}
