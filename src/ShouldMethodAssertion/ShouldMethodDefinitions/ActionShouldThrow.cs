﻿using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Action))]
public partial struct ActionShouldThrow
{
    public TException ShouldThrow<TException>(bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None) where TException : Exception
    {
        try
        {
            Actual.Invoke();
        }
        catch (TException ex) when (includeDerivedType ? true : ex.GetType() == typeof(TException))
        {
            return ex;
        }
        catch (AggregateException ex) when (aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            var self = this;
            var actualExpression = ActualExpression;
            return ThrowHandlingHelper.HandleCatchedAggregateException<TException>(ex, includeDerivedType, aggregateExceptionHandling,
                createFailException: () => AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, typeof(TException), actualExpression));
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, typeof(TException), ActualExpression);
        }

        throw AssertExceptionUtil.CreateBasicShouldThrowFailByNoThrownMessage(ActualExpression);
    }

    public Exception ShouldThrow(Type expectedExceptionType, bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None)
    {
        try
        {
            Actual.Invoke();
        }
        catch (Exception ex) when (ThrowHandlingHelper.IsExpectedException(expectedExceptionType, includeDerivedType, ex))
        {
            return ex;
        }
        catch (AggregateException ex) when (aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            var self = this;
            var actualExpression = ActualExpression;
            return ThrowHandlingHelper.HandleCatchedAggregateException(expectedExceptionType, ex, includeDerivedType, aggregateExceptionHandling,
                createFailException: () => AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, expectedExceptionType, actualExpression));
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, expectedExceptionType, ActualExpression);
        }

        throw AssertExceptionUtil.CreateBasicShouldThrowFailByNoThrownMessage(ActualExpression);
    }

    public void ShouldNotThrow()
    {
        try
        {
            Actual.Invoke();
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.CreateBasicShouldNotThrowFailByUnexpectedExceptionThrownMessage(ex, ActualExpression);
        }
    }
}
