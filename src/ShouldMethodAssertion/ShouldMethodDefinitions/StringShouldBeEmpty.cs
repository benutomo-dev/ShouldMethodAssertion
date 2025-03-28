using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(string))]
public partial struct StringShouldBeEmpty
{
    public void ShouldBeEmpty()
    {
        if (Actual == "")
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not Empty.");
    }

    public void ShouldNotBeEmpty()
    {
        if (Actual != "")
            return;
        
        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is Empty.");
    }
}
