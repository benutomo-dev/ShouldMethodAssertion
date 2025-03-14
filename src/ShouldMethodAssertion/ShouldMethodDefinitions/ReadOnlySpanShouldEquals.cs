using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(ReadOnlySpan<>))]
public partial struct ReadOnlySpanShouldEquals<T> // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    public void ShouldEqual(ReadOnlySpan<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Context.Actual.SequenceEqual(expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldNotEqual(ReadOnlySpan<T> expected, bool ignoreOrder, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (!Context.Actual.SequenceEqual(expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}
