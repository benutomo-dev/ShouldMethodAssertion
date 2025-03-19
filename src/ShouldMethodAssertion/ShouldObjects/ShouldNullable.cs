using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Nullable<>))]
[ShouldMethod(typeof(NullableStructShouldBeNull<>), TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
public partial struct ShouldNullable<T> where T : struct
{
}
