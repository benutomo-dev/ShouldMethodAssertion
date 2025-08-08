using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(FileInfo))]
public partial struct FileInfoShouldExist
{
    public void ShouldExist()
    {
        if (!Actual.Exists)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine}({ExpressionUtil.FormartValue(Actual.FullName)}) is not existing.");

    }

    public void ShouldNotExist()
    {
        if (Actual.Exists)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine}({ExpressionUtil.FormartValue(Actual.FullName)}) is existing.");
    }
}
