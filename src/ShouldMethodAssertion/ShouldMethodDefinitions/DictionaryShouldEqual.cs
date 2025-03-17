using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IReadOnlyDictionary<,>))]
public partial struct DictionaryShouldEqual<TKey, TValue>
{
    public void ShouldEqual(IEnumerable<(TKey Key, TValue Value)> expected, IEqualityComparer<TValue>? valueComparer = null)
    {
        ShouldEqual(expected.Select(v => new KeyValuePair<TKey, TValue>(v.Key, v.Value)), valueComparer);
    }

    public void ShouldEqual(IEnumerable<KeyValuePair<TKey, TValue>> expected, IEqualityComparer<TValue>? valueComparer = null)
    {
        valueComparer ??= EqualityComparer<TValue>.Default;

        var expectedKeySet = new HashSet<TKey>();

        foreach (var expectedKeyValuePair in expected)
        {
            expectedKeySet.Add(expectedKeyValuePair.Key);

            if (!Actual.TryGetValue(expectedKeyValuePair.Key, out var actualValue))
                throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not {ParamExpressions.expected.OneLine}.");

            if (!valueComparer.Equals(expectedKeyValuePair.Value, actualValue))
                throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not {ParamExpressions.expected.OneLine}.");
        }

        if (Actual.Count != expectedKeySet.Count)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not {ParamExpressions.expected.OneLine}.");
    }

    public void ShouldNotEqual(IEnumerable<(TKey Key, TValue Value)> expected, IEqualityComparer<TValue>? valueComparer = null)
    {
        ShouldNotEqual(expected.Select(v => new KeyValuePair<TKey, TValue>(v.Key, v.Value)), valueComparer);
    }

    public void ShouldNotEqual(IEnumerable<KeyValuePair<TKey, TValue>> expected, IEqualityComparer<TValue>? valueComparer = null)
    {
        valueComparer ??= EqualityComparer<TValue>.Default;

        var expectedKeySet = new HashSet<TKey>();

        foreach (var expectedKeyValuePair in expected)
        {
            expectedKeySet.Add(expectedKeyValuePair.Key);

            if (!Actual.TryGetValue(expectedKeyValuePair.Key, out var actualValue))
                return;

            if (!valueComparer.Equals(expectedKeyValuePair.Value, actualValue))
                return;
        }

        if (Actual.Count != expectedKeySet.Count)
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is {ParamExpressions.expected.OneLine}.");
    }
}
