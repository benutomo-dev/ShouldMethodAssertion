using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(Enum))]
public partial struct EnumShouldHaveFlag
{
    public void ShouldHaveFlag<T>(T expected) where T : struct, Enum
    {
        if (Actual.HasFlag(expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} not has {ParamExpressions.expected}.");
    }

    public void ShouldNotHaveFlag<T>(T expected) where T : struct, Enum
    {
        if (!Actual.HasFlag(expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} has {ParamExpressions.expected}.");
    }
}
