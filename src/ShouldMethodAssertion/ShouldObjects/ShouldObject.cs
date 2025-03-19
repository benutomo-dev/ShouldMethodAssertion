using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(object))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(ObjectShouldBe<>), TypeArgs = [typeof(object)])]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
public partial struct ShouldObject
{
}
