using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object), AcceptNullReference = true)]
public partial struct ObjectShouldBeAssignableTo
{
    public ShouldContinuation<T> ShouldBeAssignableTo<T>()
    {
        if (Actual is null)
            return default!;

        if (!Actual.GetType().IsAssignableTo(typeof(T)))
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is not assignable to {ExpressionUtil.FormatValue(typeof(T))}.
                {ActualExpression.OneLine} is {ExpressionUtil.FormatValue(Actual.GetType())}.
                """);

        return new ShouldContinuation<T>((T)Actual);
    }

    public void ShouldBeAssignableTo(Type type)
    {
        if (Actual is null)
            return;

        if (!Actual.GetType().IsAssignableTo(type))
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is not assignable to {(ParamExpressions.type.IsTypeOfExpression ? ParamExpressions.type.OneLine : $"{ParamExpressions.type.OneLine}({ExpressionUtil.FormatValue(type)})")}.
                {ActualExpression.OneLine} is {ExpressionUtil.FormatValue(Actual.GetType())}.
                """);
    }

    public void ShouldNotBeAssignableTo<T>()
    {
        if (Actual is null)
            return;

        if (Actual.GetType().IsAssignableTo(typeof(T)))
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is assignable to {ExpressionUtil.FormatValue(typeof(T))}.
                {ActualExpression.OneLine} is {ExpressionUtil.FormatValue(Actual.GetType())}.
                """);
    }

    public void ShouldNotBeAssignableTo(Type type)
    {
        if (Actual is null)
            return;

        if (Actual.GetType().IsAssignableTo(type))
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is assignable to {(ParamExpressions.type.IsTypeOfExpression ? ParamExpressions.type.OneLine : $"{ParamExpressions.type.OneLine}({ExpressionUtil.FormatValue(type)})")}.
                {ActualExpression.OneLine} is {ExpressionUtil.FormatValue(Actual.GetType())}.
                """);
    }
}
