using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(IReadOnlyDictionary<,>), TypeArgs = [typeof(TypeArg1), typeof(TypeArg2)])]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(DictionaryShouldEqual<,>),      TypeArgs = [typeof(TypeArg1), typeof(TypeArg2)])]
[ShouldMethod(typeof(DictionaryShouldBeEmpty<,>),    TypeArgs = [typeof(TypeArg1), typeof(TypeArg2)])]
[ShouldMethod(typeof(DictionaryShouldContainKey<,>), TypeArgs = [typeof(TypeArg1), typeof(TypeArg2)])]
[ShouldMethod(typeof(DictionaryShouldContain<,>),    TypeArgs = [typeof(TypeArg1), typeof(TypeArg2)])]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
#pragma warning disable CA1711 // 識別子は、不適切なサフィックスを含むことはできません
public partial struct ShouldDictionary<TKey, TValue>
#pragma warning restore CA1711 // 識別子は、不適切なサフィックスを含むことはできません
{
}
