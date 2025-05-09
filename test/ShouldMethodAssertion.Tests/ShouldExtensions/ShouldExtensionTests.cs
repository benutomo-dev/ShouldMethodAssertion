using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldObjects;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace ShouldMethodAssertion.Tests.ShouldExtensions;

public class ShouldExtensionTests
{
    [Fact]
    public void ShouldExtension_object() => Assert.IsType<ShouldObject>(new object().Should());

    [Fact]
    public void ShouldExtension_Enum() => Assert.IsType<ShouldEnum<MethodImplOptions>>(MethodImplOptions.NoOptimization.Should());

    [Fact]
    public void ShouldExtension_Nullable_Enum() => Assert.IsType<NullableShouldEnum<MethodImplOptions>>(default(MethodImplOptions?).Should());

    [Fact]
    public void ShouldExtension_bool() => Assert.IsType<ShouldBoolean>(true.Should());

    [Fact]
    public void ShouldExtension_Nullable_bool() => Assert.IsType<NullableShouldBoolean>(default(bool?).Should());

    [Fact]
    public void ShouldExtension_int() => Assert.IsType<ShouldComparable<int>>(1.Should());

    [Fact]
    public void ShouldExtension_Nullable_int() => Assert.IsType<NullableShouldStruct<int>>(default(int?).Should());

    [Fact]
    public void ShouldExtension_double() => Assert.IsType<ShouldComparable<double>>(1.0.Should());

    [Fact]
    public void ShouldExtension_Nullable_double() => Assert.IsType<NullableShouldStruct<double>>(default(double?).Should());

    [Fact]
    public void ShouldExtension_string() => Assert.IsType<ShouldString>("".Should());

    [Fact]
    public void ShouldExtension_Guid() => Assert.IsType<ShouldGuid>(Guid.Empty.Should());

    [Fact]
    public void ShouldExtension_Nullable_Guid() => Assert.IsType<NullableShouldGuid>(default(Guid?).Should());

    [Fact]
    public void ShouldExtension_Action() => Assert.IsType<ShouldAction>(new Action(() => { }).Should());

    [Fact]
    public void ShouldExtension_Func() => Assert.IsType<ShouldFunc<int>>(new Func<int>(() => 1).Should());

    [Fact]
    public void ShouldExtension_AwaitableAction() => Assert.IsType<ShouldInvokeAsync>(InvokeAsync.That(async () => await Task.CompletedTask).Should());

    [Fact]
    public void ShouldExtension_AwaitableFunc() => Assert.IsType<ShouldInvokeAsync<int>>(InvokeAsync.That(async () => await Task.FromResult(1)).Should());

    [Fact]
    public void ShouldExtension_Array() => Assert.IsType<ShouldEnumerable<int>>(Array.Empty<int>().Should());

    [Fact]
    public void ShouldExtension_Span() => ((Span<int>)[1, 2]).Should().Equal(((Span<int>)[1, 2]));

    [Fact]
    public void ShouldExtension_ReadOnlySpan() => ((ReadOnlySpan<int>)[1, 2]).Should().Equal(((ReadOnlySpan<int>)[1, 2]));

    [Fact]
    public void ShouldExtension_Dictionary() => Assert.IsType<ShouldDictionary<int, int>>(new Dictionary<int,int>().Should());

    [Fact]
    public void ShouldExtension_XElement() => Assert.IsType<ShouldXElement>(new XElement("Xxx").Should());
}
