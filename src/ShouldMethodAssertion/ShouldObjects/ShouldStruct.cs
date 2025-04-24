using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(TypeArg1), OverloadResolutionPriority = OverloadResolutionPriorities.GenericStruct)]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(StructShouldBe<>),         TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(NullableStructShouldBe<>), TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
public partial struct ShouldStruct<T> where T : struct
{
}
