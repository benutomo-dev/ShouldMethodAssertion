using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object), AcceptNullReference = true)]
public partial struct ObjectShouldBeAssignableTo
{
    public void ShouldBeAssignableTo<T>()
    {
        if (Actual is null)
            return;

        if (!Actual.GetType().IsAssignableTo(typeof(T)))
            throw AssertExceptionUtil.Create($"{ActualExpression} is can not assign to {typeof(T).FullName}.");
    }

    public void ShouldBeAssignableTo(Type type)
    {
        if (Actual is null)
            return;

        if (!Actual.GetType().IsAssignableTo(type))
            throw AssertExceptionUtil.Create($"{ActualExpression} is can not assign to {ParamExpressions.type}.");
    }

    public void ShouldNotBeAssignableTo<T>()
    {
        if (Actual is null)
            return;

        if (Actual.GetType().IsAssignableTo(typeof(T)))
            throw AssertExceptionUtil.Create($"{ActualExpression} is can assign to {typeof(T).FullName}.");
    }

    public void ShouldNotBeAssignableTo(Type type)
    {
        if (Actual is null)
            return;

        if (Actual.GetType().IsAssignableTo(type))
            throw AssertExceptionUtil.Create($"{ActualExpression} is can assign to {ParamExpressions.type}.");
    }
}
