using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IReadOnlyDictionary<,>))]
public partial struct DictionaryShouldContain<TKey, TValue>
{
    public void ShouldContain(TKey key, TValue value, IEqualityComparer<TValue>? valueComparer = null)
    {
        ShouldContain(new KeyValuePair<TKey, TValue>(key, value), valueComparer);
    }

    public void ShouldContain((TKey Key, TValue Value) keyValuePair, IEqualityComparer<TValue>? valueComparer = null)
    {
        ShouldContain(new KeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value), valueComparer);
    }

    public void ShouldContain(KeyValuePair<TKey, TValue> keyValuePair, IEqualityComparer<TValue>? valueComparer = null)
    {
        valueComparer ??= EqualityComparer<TValue>.Default;

        if (!Actual.TryGetValue(keyValuePair.Key, out var actualValue))
        {
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} is NOT contain expected key.
                
                [ExpectedKey]
                {keyValuePair.Key}
                """);
        }

        if (!valueComparer.Equals(actualValue, keyValuePair.Value))
        {
            var actualValueText = actualValue?.ToString() ?? "null";
            var expectdValueText = keyValuePair.Value?.ToString() ?? "null";

            if (actualValueText != expectdValueText)
            {
                throw AssertExceptionUtil.Create($"""
                    {ActualExpression.OneLine} contain expected key. But value is NOT expected.

                    [ActualValue]
                    {actualValueText}
                
                    [ExpectedValue]
                    {expectdValueText}
                    """);
            }
            else
            {
                throw AssertExceptionUtil.Create($"""
                    {ActualExpression.OneLine} contain expected key. But value is NOT expected.
                    """);
            }
        }
    }

    public void ShouldNotContain(TKey key, TValue value, IEqualityComparer<TValue>? valueComparer = null)
    {
        ShouldNotContain(new KeyValuePair<TKey, TValue>(key, value), valueComparer);
    }

    public void ShouldNotContain((TKey Key, TValue Value) keyValuePair, IEqualityComparer<TValue>? valueComparer = null)
    {
        ShouldNotContain(new KeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value), valueComparer);
    }

    public void ShouldNotContain(KeyValuePair<TKey, TValue> keyValuePair, IEqualityComparer<TValue>? valueComparer = null)
    {
        valueComparer ??= EqualityComparer<TValue>.Default;

        if (Actual.TryGetValue(keyValuePair.Key, out var actualValue) && valueComparer.Equals(actualValue, keyValuePair.Value))
        {
            throw AssertExceptionUtil.Create($"""
                {ActualExpression.OneLine} contain {ParamExpressions.keyValuePair.OneLine}.
                """);
        }
    }
}
