using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(bool))]
public partial struct BooleanShouldBe
{
    public void ShouldBeTrue()
    {
        if (Actual != true)
            throw AssertExceptionUtil.Create($"{ActualExpression} is not true.");
    }

    public void ShouldBeFalse()
    {
        if (Actual != false)
            throw AssertExceptionUtil.Create($"{ActualExpression} is not false.");
    }
}
