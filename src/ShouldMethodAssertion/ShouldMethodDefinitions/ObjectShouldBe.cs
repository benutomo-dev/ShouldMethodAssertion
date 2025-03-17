using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object), AcceptNullReference = true)]
public partial struct ObjectShouldBe
{
    public void ShouldBe<T>(T? expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is null && expected is null)
            return;

        if (Actual is T actual && comparer.Equals(actual, expected!))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is not {ParamExpressions.expected}.");
    }

    public void ShouldNotBe<T>(T? expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is null && expected is null)
            throw AssertExceptionUtil.Create($"Both {ActualExpression} and {ParamExpressions.expected} are `null`.");

        if (Actual is not T actual || !comparer.Equals(actual, expected!))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is {ParamExpressions.expected}.");
    }
}
