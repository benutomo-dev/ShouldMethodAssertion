using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object))]
public partial struct ObjectShouldBeOfType
{
    public void ShouldBeOfType<T>()
    {
        if (Actual.GetType() != typeof(T))
            throw AssertExceptionUtil.Create($"Type of {ActualExpression.OneLine} is not {ExpressionUtil.FormatValueAsOneline(typeof(T))}.");
    }

    public void ShouldBeOfType(Type type)
    {
        if (Actual.GetType() != type)
            throw AssertExceptionUtil.Create($"Type of {ActualExpression.OneLine} is not {ExpressionUtil.FormatValueAsOneline(type)}.");
    }

    public void ShouldNotBeOfType<T>()
    {
        if (Actual.GetType() == typeof(T))
            throw AssertExceptionUtil.Create($"Type of {ActualExpression.OneLine} is {ExpressionUtil.FormatValueAsOneline(typeof(T))}.");
    }

    public void ShouldNotBeOfType(Type type)
    {
        if (Actual.GetType() == type)
            throw AssertExceptionUtil.Create($"Type of {ActualExpression.OneLine} is {ExpressionUtil.FormatValueAsOneline(type)}.");
    }

}
