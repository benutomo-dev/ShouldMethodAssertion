using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object), AcceptNullReference = true)]
public partial struct ObjectShouldBeAssignableTo
{
    public T ShouldBeAssignableTo<T>()
    {
        if (Actual is null)
            return default!;

        if (!Actual.GetType().IsAssignableTo(typeof(T)))
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is can not assign to {typeof(T).FullName}.");

        return (T)Actual;
    }

    public void ShouldBeAssignableTo(Type type)
    {
        if (Actual is null)
            return;

        if (!Actual.GetType().IsAssignableTo(type))
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is can not assign to {ParamExpressions.type.OneLine}.");
    }

    public void ShouldNotBeAssignableTo<T>()
    {
        if (Actual is null)
            return;

        if (Actual.GetType().IsAssignableTo(typeof(T)))
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is can assign to {typeof(T).FullName}.");
    }

    public void ShouldNotBeAssignableTo(Type type)
    {
        if (Actual is null)
            return;

        if (Actual.GetType().IsAssignableTo(type))
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is can assign to {ParamExpressions.type.OneLine}.");
    }
}
