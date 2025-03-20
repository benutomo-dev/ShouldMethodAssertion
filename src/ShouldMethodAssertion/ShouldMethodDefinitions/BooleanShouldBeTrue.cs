using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(bool))]
public partial struct BooleanShouldBeTrue
{
    public void ShouldBeTrue()
    {
        if (Actual != true)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not true.");
    }

    public void ShouldBeFalse()
    {
        if (Actual != false)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not false.");
    }
}
