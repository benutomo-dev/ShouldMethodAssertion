using ShouldMethodAssertion.ShouldMethodDefinitions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.ShouldExtensions;

public struct ShouldContinuationAwaitable<T>
{
    private Task<T> _task;

    public ShouldContinuationAwaitable(Task<T> task)
    {
        _task = task;
    }

    public TaskAwaiter<T> GetAwaiter() => _task.GetAwaiter();

    public ConfiguredTaskAwaitable<T> ConfigureAwait(bool continueOnCapturedContext) => _task.ConfigureAwait(continueOnCapturedContext);

    [StackTraceHidden]
    public ShouldContinuationAwaitable<T> AndSatisfy(Action<T> action) => new ShouldContinuationAwaitable<T>(ShouldContinuationUtils.AndSatisfy(_task, action));
}
