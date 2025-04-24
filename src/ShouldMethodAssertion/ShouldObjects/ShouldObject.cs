using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(object), OverloadResolutionPriority = OverloadResolutionPriorities.Object)]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBe<>), TypeArgs = [typeof(object)])]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(ObjectShouldBeOfType))]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
public partial struct ShouldObject
{
}
