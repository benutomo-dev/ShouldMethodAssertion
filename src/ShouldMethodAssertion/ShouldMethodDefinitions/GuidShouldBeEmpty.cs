using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Guid))]
public partial struct GuidShouldBeEmpty
{
    public void ShouldBeEmpty()
    {
        if (Actual == Guid.Empty)
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is not Empty.");
    }

    public void ShouldNotBeEmpty()
    {
        if (Actual != Guid.Empty)
            return;
        
        throw AssertExceptionUtil.Create($"{ActualExpression} is Empty.");
    }
}
