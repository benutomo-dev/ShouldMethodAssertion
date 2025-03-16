using ShouldMethodAssertion.DataAnnotations;
using System.Diagnostics;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Action))]
public partial struct ActionShouldThrow
{
    public TException ShouldThrow<TException>(bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None) where TException : Exception
    {
        try
        {
            Context.Actual.Invoke();
        }
        catch (TException ex) when (includeDerivedType ? true : ex.GetType() == typeof(TException))
        {
            return ex;
        }
        catch (AggregateException ex) when (aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            var self = this;
            return ThrowHandlingHelper.HandleCatchedAggregateException<TException>(ex, includeDerivedType, aggregateExceptionHandling,
                createFailException: () => AssertExceptionUtil.Create($"`{self.Context.ActualExpression}` is throw {ex.GetType().FullName}.", ex));
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.", ex);
        }
        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not throw.");
    }

    public Exception ShouldThrow(Type expectedExceptionType, bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None)
    {
        try
        {
            Context.Actual.Invoke();
        }
        catch (Exception ex) when (ThrowHandlingHelper.IsExpectedException(expectedExceptionType, includeDerivedType, ex))
        {
            return ex;
        }
        catch (AggregateException ex) when (aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            var self = this;
            return ThrowHandlingHelper.HandleCatchedAggregateException(expectedExceptionType, ex, includeDerivedType, aggregateExceptionHandling,
                createFailException: () => AssertExceptionUtil.Create($"`{self.Context.ActualExpression}` is throw {ex.GetType().FullName}.", ex));
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.", ex);
        }
        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not throw.");
    }

    public void ShouldNotThrow()
    {
        try
        {
            Context.Actual.Invoke();
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.", ex);
        }
    }
}
