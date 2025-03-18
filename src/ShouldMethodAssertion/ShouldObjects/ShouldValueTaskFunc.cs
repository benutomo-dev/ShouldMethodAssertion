using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.ShouldObjects;

[ShouldExtension(typeof(Func<ValueTask>))]
[ShouldMethod(typeof(ObjectShouldBeNull))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
[ShouldMethod(typeof(TaskFuncShouldThrow), ConvertBy = nameof(ToTaskFunc))]
public partial struct ShouldValueTaskFunc
{
    private static Func<Task> ToTaskFunc(Func<ValueTask> valueTask) => async () => await valueTask().ConfigureAwait(false);
}
