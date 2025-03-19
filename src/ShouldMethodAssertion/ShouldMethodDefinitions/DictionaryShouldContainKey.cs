using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IReadOnlyDictionary<,>))]
public partial struct DictionaryShouldContainKey<TKey, TValue>
{
    public void ShouldContainKey(TKey key)
    {
        if (!Actual.TryGetValue(key, out var actualValue))
        {
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is NOT contain expected key.
                
                [ExpectedKey]
                {key}
                """);
        }
    }

    public void ShouldNotContainKey(TKey key)
    {
        if (Actual.ContainsKey(key))
        {
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} contain {ParamExpressions.key.OneLine}.
                """);
        }
    }
}
