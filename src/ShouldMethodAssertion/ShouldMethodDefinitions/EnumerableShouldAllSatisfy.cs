using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IEnumerable<>))]
public partial struct EnumerableShouldAllSatisfy<T>
{
    public void ShouldAllSatisfy(Action<T> action)
    {
        List<(int index, T value, Exception exception)>? fails = null;

        foreach (var current in Actual.Select((value, index) => (value, index)))
        {
            try
            {
                action(current.value);
            }
            catch (Exception ex)
            {
                fails ??= [];
                fails.Add((current.index, current.value, ex));
            }
        }

        if (fails is null)
            return;

        AssertExceptionUtil.CreateBasicShouldAllSatisfyFail(fails, ActualExpression);
    }
}
