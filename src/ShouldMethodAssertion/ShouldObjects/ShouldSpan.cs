using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Span<>))]
[ShouldMethod(typeof(ReadOnlySpanShouldEquals<>))]
public partial struct ShouldSpan<T>
{
}
