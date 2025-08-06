using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IReadOnlyDictionary<,>))]
public partial struct DictionaryShouldAllSatisfy<TKey, TValue>
{
    public void ShouldAllSatisfy(Action<TKey, TValue> action)
    {
        List<(TKey key, TValue value, Exception exception)>? fails = null;

        foreach (var current in Actual)
        {
            try
            {
                action(current.Key, current.Value);
            }
            catch (Exception ex)
            {
                fails ??= [];
                fails.Add((current.Key, current.Value, ex));
            }
        }

        if (fails is null)
            return;

        AssertExceptionUtil.CreateBasicShouldAllSatisfyFail(fails, ActualExpression);
    }
}
