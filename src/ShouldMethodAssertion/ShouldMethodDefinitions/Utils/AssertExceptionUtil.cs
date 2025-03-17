namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

public static class AssertExceptionUtil
{
    private interface IExceptionFactory
    {
        Exception Create(string message);
        Exception Create(string message, Exception exception);
    }

    private sealed class DefaultExceptionFactory : IExceptionFactory
    {
        public Exception Create(string message) => new ShouldMethodAssertion.ShouldMethodDefinitions.Exceptions.ShouldMethodAssertionException(message);

        public Exception Create(string message, Exception exception) => new ShouldMethodAssertion.ShouldMethodDefinitions.Exceptions.ShouldMethodAssertionException(message, exception);
    }

    private sealed class XunitExceptionFactory : IExceptionFactory
    {
        public Exception Create(string message) => new Xunit.Sdk.ShouldMethodAssertionException(message);

        public Exception Create(string message, Exception exception) => new Xunit.Sdk.ShouldMethodAssertionException(message, exception);
    }

    private static Lazy<IExceptionFactory> _exceptionFactory = new Lazy<IExceptionFactory>(CreateFactory);

    private static IExceptionFactory CreateFactory()
    {
        var x = AppDomain.CurrentDomain.GetAssemblies();

        if (AppDomain.CurrentDomain.GetAssemblies().Any(v => v.GetName().Name is "xunit.core" or "xunit.v3.core"))
        {
            return new XunitExceptionFactory();
        }
        else
        {
            return new DefaultExceptionFactory();
        }
    }

    public static Exception Create(string message) => _exceptionFactory.Value.Create(message);

    public static Exception Create(string message, Exception exception) => _exceptionFactory.Value.Create(message, exception);
}
