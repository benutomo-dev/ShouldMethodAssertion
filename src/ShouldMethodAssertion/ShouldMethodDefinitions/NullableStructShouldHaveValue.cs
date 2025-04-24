using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Nullable<>))]
public partial struct NullableStructShouldHaveValue<T> where T : struct
{
    public void ShouldNotHaveValue()
    {
        if (!Actual.HasValue)
            return;


        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} is not null.

            [Actual]
            {ExpressionUtil.FormartValue(Actual.Value)}
            """);
    }

    public T ShouldHaveValue()
    {
        if (!Actual.HasValue)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is null.");

        return Actual.Value;
    }
}
