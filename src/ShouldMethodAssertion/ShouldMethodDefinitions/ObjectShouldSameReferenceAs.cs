using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object))]
public partial struct ObjectShouldSameReferenceAs
{
    public void ShouldSameReferenceAs<T>(T expected) where T : class
    {
        if (ReferenceEquals(Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not same as {ParamExpressions.expected.OneLine}.");
    }

    public void ShouldNotSameReferenceAs<T>(T expected) where T : class
    {
        if (!ReferenceEquals(Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is same as {ParamExpressions.expected.OneLine}.");
    }
}
