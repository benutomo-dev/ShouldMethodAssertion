using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(string))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBe<>), TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
public partial struct ShouldString
{
}
