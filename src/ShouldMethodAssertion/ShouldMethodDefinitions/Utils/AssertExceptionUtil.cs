using System.Globalization;
using System.Text;

namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

public static class AssertExceptionUtil
{
    private interface IExceptionFactory
    {
        Exception Create(string message, Exception? exception);
    }

    private sealed class DefaultExceptionFactory : IExceptionFactory
    {
        public Exception Create(string message, Exception? exception)
        {
            if (exception is null)
                return new Exceptions.ShouldMethodAssertionException(message);
            else
                return new Exceptions.ShouldMethodAssertionException(message, exception);
        }
    }

    private sealed class XunitExceptionFactory : IExceptionFactory
    {
        public Exception Create(string message, Exception? exception)
        {
            if (exception is null)
                return new Xunit.Sdk.ShouldMethodAssertionException(message);
            else
                return new Xunit.Sdk.ShouldMethodAssertionException(message, exception);
        }
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

    internal static Exception CreateSimpleIsStyleMessage<TActual, TExpected>(TActual actual, ValueExpression actualExpression, TExpected expectedValue, ValueExpression expectedExpression, Exception? exception = null)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} IS {expectedExpression.OneLine}. But did not expect it to be.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[Actual]");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actual}");

        return _exceptionFactory.Value.Create(stringBuilder.ToString(), exception);
    }

    internal static Exception CreateSimpleIsNotStyleMessage<TActual, TExpected>(TActual actual, ValueExpression actualExpression, TExpected expectedValue, ValueExpression expectedExpression, Exception? exception = null)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actualExpression.OneLine} is NOT {expectedExpression.OneLine}. But did not expect it to be.");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[Actual]");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{actual}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"[Expected]");
        stringBuilder.AppendLine(CultureInfo.InvariantCulture, $"{expectedValue}");

        return _exceptionFactory.Value.Create(stringBuilder.ToString(), exception);
    }

    public static Exception Create(string message, Exception? exception = null) =>  _exceptionFactory.Value.Create(message, exception);
}
