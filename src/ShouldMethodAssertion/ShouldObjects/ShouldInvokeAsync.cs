using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(InvokeAsync))]
[ShouldMethod(typeof(InvokeAsyncShouldThrow))]
public partial struct ShouldInvokeAsync
{
}
