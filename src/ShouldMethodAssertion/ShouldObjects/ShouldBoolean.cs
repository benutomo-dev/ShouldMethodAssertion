using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(bool))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(StructShouldBe<>), TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(NullableStructShouldBe<>), TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(BooleanShouldBe))]
public partial struct ShouldBoolean
{
}
