using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Nullable<>), AcceptNullReference = true)]
public partial struct NullableShouldBeNull<T> where T : struct
{
    public void ShouldBeNull()
    {
        if (Actual.HasValue)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not null.");
    }

    public void ShouldNotBeNull()
    {
        if (!Actual.HasValue)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is null.");
    }
}
