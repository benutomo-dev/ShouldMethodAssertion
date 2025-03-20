namespace ShouldMethodAssertion.Tests;

internal sealed class AlwaysEqual<T> : IEqualityComparer<T>
{
    public static AlwaysEqual<T> Default { get; } = new AlwaysEqual<T>();

    public bool Equals(T? x, T? y) => true;

    public int GetHashCode(T obj) => 0;
}