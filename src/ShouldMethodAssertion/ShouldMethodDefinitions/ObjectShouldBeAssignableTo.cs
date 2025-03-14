using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object))]
public partial struct ObjectShouldBeAssignableTo
{
    public void ShouldBeAssignableTo<T>()
    {
        if (!Context.Actual.GetType().IsAssignableTo(typeof(T)))
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is can not assign to {typeof(T).FullName}.");
    }

    public void ShouldBeAssignableTo(Type type)
    {
        if (!Context.Actual.GetType().IsAssignableTo(type))
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is can not assign to {Context.GetExpressionOf(nameof(type))}.");
    }

    public void ShouldNotBeAssignableTo<T>()
    {
        if (Context.Actual.GetType().IsAssignableTo(typeof(T)))
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is can assign to {typeof(T).FullName}.");
    }

    public void ShouldNotBeAssignableTo(Type type)
    {
        if (Context.Actual.GetType().IsAssignableTo(type))
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is can assign to {Context.GetExpressionOf(nameof(type))}.");
    }
}
