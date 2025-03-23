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

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} is not same reference as {ParamExpressions.expected.OneLine}. But did not expect it to be.

            [Actual]
            {ExpressionUtil.FormartValue(Actual)}

            [Expected]
            {ExpressionUtil.FormartValue(expected)}
            """);
    }

    public void ShouldNotSameReferenceAs<T>(T expected) where T : class
    {
        if (!ReferenceEquals(Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is same reference as {ParamExpressions.expected.OneLine}. But did not expect it to be.");
    }
}
