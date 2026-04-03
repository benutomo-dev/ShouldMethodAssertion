using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.ShouldExtensions;

public static class ShouldSatisfyExtension
{
    public static void ShouldSatisfy<T>(this T actual, Action<T> action, [CallerArgumentExpression(nameof(actual))] string? actualCallerArgumentExpression = null, [CallerArgumentExpression(nameof(action))] string? actionCallerArgumentExpression = null)
    {
        try
        {
            action(actual);
        }
        catch (Exception ex)
        {
            throw AssertExceptionUtil.CreateBasicShouldSatisfyFail(ex, actual, new ValueExpression(actualCallerArgumentExpression ?? nameof(actual)), actionCallerArgumentExpression, new StackFrame());
        }
    }
}
