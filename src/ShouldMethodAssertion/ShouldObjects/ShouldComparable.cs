using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(IComparable<>))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(ObjectShouldBe<>), TypeArgs = [typeof(ActualValueType)])]
[ShouldMethod(typeof(ComparableShouldBe<>), TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(ObjectShouldBeOneOf))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
[ShouldMethod(typeof(ComparableShouldCompare<>), TypeArgs = [typeof(TypeArg1)])]
public partial struct ShouldComparable<T>
{
}
