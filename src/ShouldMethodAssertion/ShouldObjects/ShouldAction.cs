using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Action))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBe))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(ActionShouldThrow))]
public partial struct ShouldAction
{
}
