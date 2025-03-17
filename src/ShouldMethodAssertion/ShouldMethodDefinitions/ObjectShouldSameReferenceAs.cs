using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object))]
public partial struct ObjectShouldSameReferenceAs
{
    public void ShouldSameReferenceAs<T>(T expected) where T : class
    {
        if (ReferenceEquals(Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is not same as {ParamExpressions.expected}.");
    }

    public void ShouldNotSameReferenceAs<T>(T expected) where T : class
    {
        if (!ReferenceEquals(Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is same as {ParamExpressions.expected}.");
    }
}
