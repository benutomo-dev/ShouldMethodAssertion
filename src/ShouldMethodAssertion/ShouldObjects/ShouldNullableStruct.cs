using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Nullable<>))]
[ShouldMethod(typeof(NullableStructShouldBeNull<>), TypeArgs = [typeof(TypeArg1)])]
[ShouldMethod(typeof(ObjectShouldBeAssignableTo))]
// Nullable<T>に対してBe()を実装してしまうとGuidの様な型別の個別実装があるときに
// 型を意識しないでNullable<Guid>型の式に対してShould()を呼び出した時に、Be以上の検証メソッドが
// 存在しないと誤解し易そうなので、Nullable<T>に対して出来ることはあえてNullチェックと型チェックに限定する
// それ以上は`nullable.Value.Should().Xxx`で呼出しが必要に統一する
//[ShouldMethod(typeof(NullableStructShouldBe<>), TypeArgs = [typeof(TypeArg1)])]
public partial struct ShouldNullableStruct<T> where T : struct
{
}
