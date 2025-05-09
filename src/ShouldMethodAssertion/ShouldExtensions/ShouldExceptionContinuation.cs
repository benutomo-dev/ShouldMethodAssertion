using ShouldMethodAssertion.ShouldMethodDefinitions;
using System.Diagnostics;

namespace ShouldMethodAssertion.ShouldExtensions;

public struct ShouldExceptionContinuation<T> where T : Exception
{
    public T Exception { get; }

    public ShouldExceptionContinuation(T exception)
    {
        Exception = exception;
    }

    [StackTraceHidden]
    public ShouldExceptionContinuation<T> AndSatisfy(Action<T> action)
    {
        action(Exception);
        return new ShouldExceptionContinuation<T>(Exception);
    }

    [StackTraceHidden]
    public ShouldExceptionContinuation<T> AndMessageMatch(string text, bool ignoreCase = false, char singleMatchChar = StringShouldMatch.DefaultSingleMatchChar, char multipleMatchWildcardChar = StringShouldMatch.DefaultMultipleMatchChar, [global::System.Runtime.CompilerServices.CallerArgumentExpression("text")] string? textCallerArgumentExpression = null, [global::System.Runtime.CompilerServices.CallerArgumentExpression("ignoreCase")] string? ignoreCaseCallerArgumentExpression = null, [global::System.Runtime.CompilerServices.CallerArgumentExpression("singleMatchChar")] string? singleMatchCharCallerArgumentExpression = null, [global::System.Runtime.CompilerServices.CallerArgumentExpression("multipleMatchWildcardChar")] string? multipleMatchWildcardCharCallerArgumentExpression = null)
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
