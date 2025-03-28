using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;
using System.Xml.Linq;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(XElement))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBe<>), TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(XElementShouldHaveElement))]
[ShouldMethod(typeof(XElementShouldHaveAttribute))]
public partial struct ShouldXElement
{
}
