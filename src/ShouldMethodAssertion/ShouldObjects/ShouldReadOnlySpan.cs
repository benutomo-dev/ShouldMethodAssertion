using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(ReadOnlySpan<>))]
[ShouldMethod(typeof(ReadOnlySpanShouldEqual<>))]
public partial struct ShouldReadOnlySpan<T>
{
}
