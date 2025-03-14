using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Action))]
public partial struct ActionShouldThrow
{
    public TException ShouldThrow<TException>() where TException : Exception
    {
        try
        {
            Context.Actual.Invoke();
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

    public Exception ShouldThrow(Type expectedExceptionType)
    {
        try
        {
            Context.Actual.Invoke();
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

    public void ShouldNotThrow()
    {
        try
        {
            Context.Actual.Invoke();
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is throw {ex.GetType().FullName}.");
        }
    }
}
