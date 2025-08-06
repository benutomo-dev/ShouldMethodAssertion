using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(IEnumerable<>))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(EnumerableShouldEqual<>),     TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(EnumerableShouldBeEmpty<>),   TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(EnumerableShouldBeSingle<>),  TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(EnumerableShouldHaveCount<>), TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(EnumerableShouldContain<>),   TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(ObjectShouldBeOfType))]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
public partial struct ShouldEnumerable<T>
{
}
