using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Guid))]
public partial struct GuidShouldBeEmpty
{
    public void ShouldBeEmpty()
    {
        if (Context.Actual == Guid.Empty)
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not Empty.");
    }

    public void ShouldNotBeEmpty()
    {
        if (Context.Actual != Guid.Empty)
            return;
        
        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is Empty.");
    }
}
