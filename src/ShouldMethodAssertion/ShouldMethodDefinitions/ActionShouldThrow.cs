using ShouldMethodAssertion.DataAnnotations;
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
            return ThrowHandlingHelper.HandleCatchedAggregateException<TException>(ex, includeDerivedType, aggregateExceptionHandling,
                createFailException: () => AssertExceptionUtil.Create($"{self.ActualExpression.OneLine} is throw {ex.GetType().FullName}.", ex));
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is throw {ex.GetType().FullName}.", ex);
        }
        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not throw.");
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
            return ThrowHandlingHelper.HandleCatchedAggregateException(expectedExceptionType, ex, includeDerivedType, aggregateExceptionHandling,
                createFailException: () => AssertExceptionUtil.Create($"{self.ActualExpression} is throw {ex.GetType().FullName}.", ex));
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is throw {ex.GetType().FullName}.", ex);
        }
        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not throw.");
    }

    public void ShouldNotThrow()
    {
        try
        {
            Actual.Invoke();
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is throw {ex.GetType().FullName}.", ex);
        }
    }
}
