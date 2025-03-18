using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Guid))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(ObjectShouldBe))]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(GuidShouldBeEmpty))]
public partial struct ShouldGuid
{
}