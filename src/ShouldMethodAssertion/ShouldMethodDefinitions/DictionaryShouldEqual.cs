using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IReadOnlyDictionary<,>))]
public partial struct DictionaryShouldEqual<TKey, TValue>
{
    public void ShouldEqual(IEnumerable<KeyValuePair<TKey, TValue>> expected, IEqualityComparer<TValue>? valueComparer = null)
    {
        valueComparer ??= EqualityComparer<TValue>.Default;

        var expectedKeySet = new HashSet<TKey>();

        foreach (var expectedKeyValuePair in expected)
        {
            expectedKeySet.Add(expectedKeyValuePair.Key);

            if (!Actual.TryGetValue(expectedKeyValuePair.Key, out var actualValue))
                throw AssertExceptionUtil.Create($"{ActualExpression} is not {ParamExpressions.expected}.");

            if (!valueComparer.Equals(expectedKeyValuePair.Value, actualValue))
                throw AssertExceptionUtil.Create($"{ActualExpression} is not {ParamExpressions.expected}.");
        }

        if (Actual.Count != expectedKeySet.Count)
            throw AssertExceptionUtil.Create($"{ActualExpression} is not {ParamExpressions.expected}.");
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

        throw AssertExceptionUtil.Create($"{ActualExpression} is {ParamExpressions.expected}.");
    }
}
