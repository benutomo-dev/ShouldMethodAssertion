using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;


[ShouldMethodDefinition(typeof(InvokeAsync))]
public partial struct InvokeAsyncShouldThrow
{
    public ShouldContinuationExceptionAwaitable<TException> ShouldThrowAsync<TException>(bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None) where TException : Exception
    {
        var task = shouldThrowAsyncCore(this, includeDerivedType, aggregateExceptionHandling);

        return new ShouldContinuationExceptionAwaitable<TException>(task);

        static async Task<TException> shouldThrowAsyncCore(InvokeAsyncShouldThrow self, bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
        {
            try
            {
                await self.Actual.AsyncAction().ConfigureAwait(false);
            }
            catch (TException ex) when (includeDerivedType ? true : ex.GetType() == typeof(TException))
            {
                return ex;
            }
            catch (AggregateException ex) when (aggregateExceptionHandling != AggregateExceptionHandling.None)
            {
                return ThrowHandlingHelper.HandleCatchedAggregateException<TException>(ex, includeDerivedType, aggregateExceptionHandling,
                    createFailException: () => AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, typeof(TException), self.ActualExpression));
            }
            catch (Exception ex)
            {
                throw AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, typeof(TException), self.ActualExpression);
            }

            throw AssertExceptionUtil.CreateBasicShouldThrowFailByNoThrownMessage(self.ActualExpression);
        }
    }

    public ShouldContinuationExceptionAwaitable<Exception> ShouldThrowAsync(Type expectedExceptionType, bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling  = AggregateExceptionHandling.None)
    {
        var task = shouldThrowAsyncCore(this, expectedExceptionType, includeDerivedType, aggregateExceptionHandling);

        return new ShouldContinuationExceptionAwaitable<Exception>(task);

        static async Task<Exception> shouldThrowAsyncCore(InvokeAsyncShouldThrow self, Type expectedExceptionType, bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
        {
            try
            {
                await self.Actual.AsyncAction().ConfigureAwait(false);
            }
            catch (Exception ex) when (ThrowHandlingHelper.IsExpectedException(expectedExceptionType, includeDerivedType, ex))
            {
                return ex;
            }
            catch (AggregateException ex) when (aggregateExceptionHandling != AggregateExceptionHandling.None)
            {
                return ThrowHandlingHelper.HandleCatchedAggregateException(expectedExceptionType, ex, includeDerivedType, aggregateExceptionHandling,
                    createFailException: () => AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, expectedExceptionType, self.ActualExpression));
            }
            catch (Exception ex)
            {
                throw AssertExceptionUtil.CreateBasicShouldThrowFailByUnexpectedExceptionThrownMessage(ex, expectedExceptionType, self.ActualExpression);
            }

            throw AssertExceptionUtil.CreateBasicShouldThrowFailByNoThrownMessage(self.ActualExpression);

        }
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
