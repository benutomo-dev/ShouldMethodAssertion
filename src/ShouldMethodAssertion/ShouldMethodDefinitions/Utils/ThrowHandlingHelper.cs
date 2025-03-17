using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

internal static class ThrowHandlingHelper
{
    public static TException HandleCatchedAggregateException<TException>(AggregateException aggregateException, bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling, Func<Exception> createFailException)
        where TException : Exception
    {
        Debug.Assert(aggregateExceptionHandling != AggregateExceptionHandling.None);

        if (aggregateExceptionHandling == AggregateExceptionHandling.SingleDirect)
        {
            if (aggregateException.InnerExceptions.Count == 1 && aggregateException.InnerException is TException expectedException)
                return expectedException;
        }
        else
        {
            var flatten = aggregateException.Flatten();

            if (aggregateExceptionHandling == AggregateExceptionHandling.SingleFlattened)
            {
                if (flatten.InnerExceptions.Count == 1 && TryGetExpectedTypeException<TException>(flatten.InnerException, includeDerivedType, out var expectedException))
                    return expectedException;
            }
            else
            {
                Debug.Assert(aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened);

                foreach (var innnerException in flatten.InnerExceptions)
                {
                    if (TryGetExpectedTypeException<TException>(innnerException, includeDerivedType, out var expectedException))
                        return expectedException;
                }
            }
        }

        throw createFailException();
    }

    public static Exception HandleCatchedAggregateException(Type expectedExceptionType, AggregateException aggregateException, bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling, Func<Exception> createFailException)
    {
        Debug.Assert(aggregateExceptionHandling != AggregateExceptionHandling.None);

        if (aggregateExceptionHandling == AggregateExceptionHandling.SingleDirect)
        {
            if (aggregateException.InnerExceptions.Count == 1 && IsExpectedException(expectedExceptionType, includeDerivedType, aggregateException.InnerException))
                return aggregateException.InnerException!;
        }
        else
        {
            var flatten = aggregateException.Flatten();

            if (aggregateExceptionHandling == AggregateExceptionHandling.SingleFlattened)
            {
                if (flatten.InnerExceptions.Count == 1 && IsExpectedException(expectedExceptionType, includeDerivedType, flatten.InnerException))
                    return flatten.InnerException!;
            }
            else
            {
                Debug.Assert(aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened);

                foreach (var innnerException in flatten.InnerExceptions)
                {
                    if (IsExpectedException(expectedExceptionType, includeDerivedType, innnerException))
                        return innnerException;
                }
            }
        }

        throw createFailException();
    }

    public static bool TryGetExpectedTypeException<TException>(Exception? exception, bool includeDerivedType, [MaybeNullWhen(false)] out TException expectedTypeException)
        where TException : Exception
    {
        if (includeDerivedType)
        {
            if (exception is TException _expectedTypeException)
            {
                expectedTypeException = _expectedTypeException;
                return true;
            }
        }
        else
        {
            if (exception?.GetType() == typeof(TException))
            {
                expectedTypeException = (TException)exception;
            }
        }

        expectedTypeException = null;
        return false;
    }

    public static bool IsExpectedException(Type expectedExceptionType, bool includeDerivedType, Exception? exception)
    {
        return includeDerivedType
            ? exception?.GetType().IsAssignableTo(expectedExceptionType) ?? false
            : exception?.GetType() == expectedExceptionType;
    }
}
