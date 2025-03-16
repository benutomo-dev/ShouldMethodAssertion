using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(bool))]
public partial struct BooleanShouldBe
{
    public void ShouldBeTrue()
    {
        if (Context.Actual != true)
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not true.");
    }

    public void ShouldBeFalse()
    {
        if (Context.Actual != false)
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not false.");
    }
}
