using ShouldMethodAssertion.ShouldMethodDefinitions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.ShouldExtensions;

public struct ShouldContinuationExceptionAwaitable<T> where T : Exception
{
    private Task<T> _task;

    public ShouldContinuationExceptionAwaitable(Task<T> task)
    {
        _task = task;
    }

    public TaskAwaiter<T> GetAwaiter() => _task.GetAwaiter();

    public ConfiguredTaskAwaitable<T> ConfigureAwait(bool continueOnCapturedContext) => _task.ConfigureAwait(continueOnCapturedContext);

    [StackTraceHidden]
    public ShouldContinuationExceptionAwaitable<T> AndSatisfy(Action<T> action) => new ShouldContinuationExceptionAwaitable<T>(ShouldContinuationUtils.AndSatisfy(_task, action));

    [StackTraceHidden]
    public ShouldContinuationExceptionAwaitable<T> AndMessageMatch(string text, bool ignoreCase = false, char singleMatchChar = StringShouldMatch.DefaultSingleMatchChar, char multipleMatchWildcardChar = StringShouldMatch.DefaultMultipleMatchChar, [global::System.Runtime.CompilerServices.CallerArgumentExpression("text")] string? textCallerArgumentExpression = null, [global::System.Runtime.CompilerServices.CallerArgumentExpression("ignoreCase")] string? ignoreCaseCallerArgumentExpression = null, [global::System.Runtime.CompilerServices.CallerArgumentExpression("singleMatchChar")] string? singleMatchCharCallerArgumentExpression = null, [global::System.Runtime.CompilerServices.CallerArgumentExpression("multipleMatchWildcardChar")] string? multipleMatchWildcardCharCallerArgumentExpression = null)
    {
        [StackTraceHidden]
        void continuationFunction(T exception)
        {
            var stringShouldMatch = new StringShouldMatch(exception.Message, $"{typeof(T).Name}.{nameof(exception.Message)}", new StringShouldMatch.__ParameterExpressions
            {
                text = textCallerArgumentExpression!,
                ignoreCase = ignoreCaseCallerArgumentExpression,
                singleMatchChar = singleMatchCharCallerArgumentExpression,
                multipleMatchWildcardChar = multipleMatchWildcardCharCallerArgumentExpression,
            });

            stringShouldMatch.ShouldMatch(text, ignoreCase, singleMatchChar, multipleMatchWildcardChar);
        }

        return AndSatisfy(continuationFunction);
    }
}
