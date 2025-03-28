using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Nullable<>))]
public partial struct NullableStructShouldBeNull<T> where T : struct
{
    public void ShouldBeNull()
    {
        if (!Actual.HasValue)
            return;


        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} is not null.

            [Actual]
            {ExpressionUtil.FormartValue(Actual.Value)}
            """);
    }

    public T ShouldNotBeNull()
    {
        if (!Actual.HasValue)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is null.");

        return Actual.Value;
    }
}
