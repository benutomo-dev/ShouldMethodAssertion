namespace ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

internal ref struct CommonEnumerator<T>
{
    private IEnumerator<T>? _enumerator;
    private ReadOnlySpan<T>.Enumerator _spanEnumerator;

    public T Current => _enumerator is null ? _spanEnumerator.Current : _enumerator.Current;

    public CommonEnumerator(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator;
    }

    public CommonEnumerator(ReadOnlySpan<T>.Enumerator spanEnumerator)
    {
        _spanEnumerator = spanEnumerator;
    }

    public bool MoveNext() => _enumerator is null ? _spanEnumerator.MoveNext() : _enumerator.MoveNext();
}
