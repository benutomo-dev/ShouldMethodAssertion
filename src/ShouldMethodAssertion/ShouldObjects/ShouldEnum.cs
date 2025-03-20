using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(TypeArg1))]
[ShouldMethod(typeof(StructShouldBe<>),         TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(NullableStructShouldBe<>), TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(EnumShouldHaveFlag))]
#pragma warning disable CA1711 // 識別子は、不適切なサフィックスを含むことはできません
public partial struct ShouldEnum<T> where T : struct, Enum
#pragma warning restore CA1711 // 識別子は、不適切なサフィックスを含むことはできません
{
}
