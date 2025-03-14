using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Func<Task>))]
public partial struct TaskFuncShouldThrow
{
    public async Task<TException> ShouldThrowAsync<TException>() where TException : Exception
    {
        try
        {
            await Context.Actual.Invoke();
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not throw.");
        }
        catch (TException ex)
        {
            return ex;
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.");
        }
    }

    public async Task<Exception> ShouldThrowAsync(Type expectedExceptionType)
    {
        try
        {
            await Context.Actual.Invoke();
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not throw.");
        }
        catch (Exception ex) when (ex.GetType().IsAssignableTo(expectedExceptionType))
        {
            return ex;
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.");
        }
    }

    public async Task ShouldNotThrowAsync()
    {
        try
        {
            await Context.Actual.Invoke();
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.");
        }
    }
}
