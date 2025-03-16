using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Enum))]
[ShouldMethod(typeof(ObjectShouldBe))]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(EnumShouldHaveFlag))]
#pragma warning disable CA1711 // 識別子は、不適切なサフィックスを含むことはできません
public partial struct ShouldEnum
#pragma warning restore CA1711 // 識別子は、不適切なサフィックスを含むことはできません
{
}
