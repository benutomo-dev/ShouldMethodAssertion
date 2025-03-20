using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Guid))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(StructShouldBe<>),         TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(NullableStructShouldBe<>), TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(GuidShouldBeEmpty))]
public partial struct ShouldGuid
{
}