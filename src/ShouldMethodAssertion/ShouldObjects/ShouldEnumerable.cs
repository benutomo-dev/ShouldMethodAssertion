using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(IEnumerable<>))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(EnumerableShouldEqual<>))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
public partial struct ShouldEnumerable<T>
{
}
