using System.Diagnostics;

namespace ShouldMethodAssertion.ShouldExtensions;

public struct ShouldContinuation<T>
{
    public T Result { get; }

    public ShouldContinuation(T result)
    {
        Result = result;
    }

    [StackTraceHidden]
    public ShouldContinuation<T> AndSatisfy(Action<T> action)
    {
        action(Result);
        return new ShouldContinuation<T>(Result);
    }
}
