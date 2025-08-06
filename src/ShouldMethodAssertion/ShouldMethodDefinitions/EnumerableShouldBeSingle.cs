using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IEnumerable<>))]
public partial struct EnumerableShouldBeSingle<T>
{
    public ShouldContinuation<T> ShouldBeSingle()
    {
        using var enumerator = Actual.GetEnumerator();

        if (!enumerator.MoveNext())
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is empty.");

        var value = enumerator.Current;

        if (enumerator.MoveNext())
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} has two or more elements.");

        return new ShouldContinuation<T>(value);
    }

    public void ShouldNotBeSingle()
    {
        using var enumerator = Actual.GetEnumerator();

        if (!enumerator.MoveNext())
            return;

        if (!enumerator.MoveNext())
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is single.");
    }
}
