using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Span<>))]
[ShouldMethod(typeof(ReadOnlySpanShouldEqual<>))]
public partial struct ShouldSpan<T>
{
}
