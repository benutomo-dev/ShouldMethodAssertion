using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Span<>), OverloadResolutionPriority = -1)]
[ShouldMethod(typeof(ReadOnlySpanShouldEqual<>), TypeArgs = [typeof(TypeArg1)])]
public partial struct ShouldSpan<T>
{
}
