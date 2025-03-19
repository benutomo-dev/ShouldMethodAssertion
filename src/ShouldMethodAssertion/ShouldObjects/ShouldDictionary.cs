using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(IReadOnlyDictionary<,>))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldBeDefault))]
[ShouldMethod(typeof(DictionaryShouldEqual<,>))]
[ShouldMethod(typeof(DictionaryShouldBeEmpty<,>))]
[ShouldMethod(typeof(DictionaryShouldContainKey<,>))]
[ShouldMethod(typeof(DictionaryShouldContain<,>))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
#pragma warning disable CA1711 // 識別子は、不適切なサフィックスを含むことはできません
public partial struct ShouldDictionary<TKey, TValue>
#pragma warning restore CA1711 // 識別子は、不適切なサフィックスを含むことはできません
{
}
