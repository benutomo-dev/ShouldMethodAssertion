using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IReadOnlyDictionary<,>))]
public partial struct DictionaryShouldContainKey<TKey, TValue>
{
    public void ShouldContainKey(TKey key, IEqualityComparer<TValue>? valueComparer = null)
    {
        valueComparer ??= EqualityComparer<TValue>.Default;

        if (!Actual.TryGetValue(key, out var actualValue))
        {
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is NOT contain expected key.
                
                [ExpectedKey]
                {key}
                """);
        }
    }

    public void ShouldNotContainKey(TKey key, IEqualityComparer<TValue>? valueComparer = null)
    {
        valueComparer ??= EqualityComparer<TValue>.Default;

        if (Actual.ContainsKey(key))
        {
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} contain {ParamExpressions.key.OneLine}.
                """);
        }
    }
}
