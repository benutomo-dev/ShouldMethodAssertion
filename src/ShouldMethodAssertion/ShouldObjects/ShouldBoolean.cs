using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(bool))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(ObjectShouldBe))]
[ShouldMethod(typeof(BooleanShouldBe))]
public partial struct ShouldBoolean
{
}
