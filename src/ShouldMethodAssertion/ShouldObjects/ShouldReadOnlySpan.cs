using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(ReadOnlySpan<>), OverloadResolutionPriority = -1)]
[ShouldMethod(typeof(ReadOnlySpanShouldEqual<>), TypeArgs = [typeof(TypeArg1)])]
public partial struct ShouldReadOnlySpan<T>
{
}
