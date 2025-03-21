using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(InvokeAsync))]
public partial struct InvokeAsyncShouldThrow
{
    public async Task<TException> ShouldThrowAsync<TException>(bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None) where TException : Exception
    {
        try
        {
            await Actual.AsyncAction().ConfigureAwait(false);
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

    public async Task<Exception> ShouldThrowAsync(Type expectedExceptionType, bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling  = AggregateExceptionHandling.None)
    {
        try
        {
            await Actual.AsyncAction().ConfigureAwait(false);
        }
        catch (Exception ex) when (ThrowHandlingHelper.IsExpectedException(expectedExceptionType, includeDerivedType, ex))
        {
            return ex;
        }
        catch (AggregateException ex) when (aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            var self = this;
            var actualExpression = ActualExpression;
            return ThrowHandlingHelper.HandleCatchedAggregateException(expectedExceptionType,ex, includeDerivedType, aggregateExceptionHandling,
                createFailException: () => AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, expectedExceptionType, actualExpression));
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, expectedExceptionType, ActualExpression);
        }

        throw AssertExceptionUtil.CreateBasicShouldThrowFailByNoThrownMessage(ActualExpression);
    }

    public async Task ShouldNotThrowAsync()
    {
        try
        {
            await Actual.AsyncAction().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.CreateBasicShouldNotThrowFailByUnexpectedExceptionThrownMessage(ex, ActualExpression);
        }
    }
}
