using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(InvokeAsync<TypeArg1>))]
[ShouldMethod(typeof(InvokeAsyncTShouldThrow<>), TypeArgs = [typeof(TypeArg1)])]
public partial struct ShouldInvokeAsync<TResult>
{
}
