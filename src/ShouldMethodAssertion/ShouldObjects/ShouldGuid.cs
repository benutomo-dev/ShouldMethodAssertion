using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Guid))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBe))]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(GuidShouldBeEmpty))]
public partial struct ShouldGuid
{
}