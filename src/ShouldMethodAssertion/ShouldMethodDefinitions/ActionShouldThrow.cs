using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Action))]
public partial struct ActionShouldThrow
{
    public ShouldExceptionContinuation<TException> ShouldThrow<TException>(bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None) where TException : Exception
    {
        var exception = shouldThrowCore(this, includeDerivedType, aggregateExceptionHandling);

        return new ShouldExceptionContinuation<TException>(exception);

        static TException shouldThrowCore(ActionShouldThrow self, bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
        {
            try
            {
                self.Actual.Invoke();
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

    public ShouldExceptionContinuation<Exception> ShouldThrow(Type expectedExceptionType, bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None)
    {
        var exception = shouldThrowCore(this, expectedExceptionType, includeDerivedType, aggregateExceptionHandling);

        return new ShouldExceptionContinuation<Exception>(exception);

        static Exception shouldThrowCore(ActionShouldThrow self, Type expectedExceptionType, bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
        {
            try
            {
                self.Actual.Invoke();
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
