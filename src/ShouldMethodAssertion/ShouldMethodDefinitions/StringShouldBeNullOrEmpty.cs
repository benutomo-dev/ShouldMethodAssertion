using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(string), AcceptNullReference = true)]
public partial struct StringShouldBeNullOrEmpty
{
    public void ShouldBeNullOrEmpty()
    {
        if (string.IsNullOrEmpty(Actual))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not null or Empty.");
    }

    public void ShouldNotBeNullOrEmpty()
    {
        if (Actual is null)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is null.");

        if (Actual == "")
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is Empty.");
    }
}
