namespace ShouldMethodAssertion;

public static class AssertExceptionUtil
{
    public static Exception Create(string message)
    {
        return new InvalidOperationException(message);
    }
}
