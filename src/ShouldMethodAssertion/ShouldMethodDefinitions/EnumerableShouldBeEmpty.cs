using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IEnumerable<>))]
public partial struct EnumerableShouldBeEmpty<T>
{
    public void ShouldBeEmpty()
    {
        if (Actual.Any())
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is empty.");
    }

    public void ShouldNotBeEmpty()
    {
        if (!Actual.Any())
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not empty.");
    }
}
