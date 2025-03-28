using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(string))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBe<>), TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(StringShouldBeEmpty))]
[ShouldMethod(typeof(StringShouldBeNullOrEmpty))]
[ShouldMethod(typeof(StringShouldContain))]
public partial struct ShouldString
{
}
