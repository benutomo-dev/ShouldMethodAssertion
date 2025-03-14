using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(ReadOnlySpan<>))]
[ShouldMethod(typeof(ReadOnlySpanShouldEquals<>))]
public partial struct ShouldReadOnlySpan<T>
{
}
