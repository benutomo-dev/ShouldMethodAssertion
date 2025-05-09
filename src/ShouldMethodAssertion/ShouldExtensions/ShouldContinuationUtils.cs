using System.Diagnostics;
using System.Runtime.ExceptionServices;

namespace ShouldMethodAssertion.ShouldExtensions;

internal static class ShouldContinuationUtils
{
    [StackTraceHidden]
    public static Task<T> AndSatisfy<T>(Task<T> task, Action<T> action)
    {
        [StackTraceHidden]
        T continuationFunction(Task<T> v)
        {
            try
            {
                var result = v.Result;
                action(result);
                return result;
            }
            catch (AggregateException ex)
            {
                ex = ex.Flatten();

                if (ex.InnerExceptions.Count != 1)
                    throw;

                ExceptionDispatchInfo.Capture(ex.InnerExceptions[0]).Throw();
                throw;
            }
        }

        return task.ContinueWith(continuationFunction);
    }
}
