namespace ShouldMethodAssertion.ShouldMethodDefinitions;

public static class AssertExceptionUtil
{
    public static Exception Create(string message)
    {
        return new InvalidOperationException(message);
    }

    public static Exception Create(string message, Exception exception)
    {
        return new InvalidOperationException(message, exception);
    }
}
