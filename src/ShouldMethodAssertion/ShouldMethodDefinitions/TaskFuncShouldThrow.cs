using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Func<Task>))]
public partial struct TaskFuncShouldThrow
{
    public async Task<TException> ShouldThrowAsync<TException>(bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling = AggregateExceptionHandling.None) where TException : Exception
    {
        try
        {
            await Context.Actual.Invoke();
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

    public async Task<Exception> ShouldThrowAsync(Type expectedExceptionType, bool includeDerivedType = false, AggregateExceptionHandling aggregateExceptionHandling  = AggregateExceptionHandling.None)
    {
        try
        {
            await Context.Actual.Invoke();
        }
        catch (Exception ex) when (ThrowHandlingHelper.IsExpectedException(expectedExceptionType, includeDerivedType, ex))
        {
            return ex;
        }
        catch (AggregateException ex) when (aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            var self = this;
            return ThrowHandlingHelper.HandleCatchedAggregateException(expectedExceptionType,ex, includeDerivedType, aggregateExceptionHandling,
                createFailException: () => AssertExceptionUtil.Create($"`{self.Context.ActualExpression}` is throw {ex.GetType().FullName}.", ex));
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.", ex);
        }
        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not throw.");
    }

    public async Task ShouldNotThrowAsync()
    {
        try
        {
            await Context.Actual.Invoke();
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.", ex);
        }
    }
}
