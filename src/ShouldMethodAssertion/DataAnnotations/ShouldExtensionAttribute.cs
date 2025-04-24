namespace ShouldMethodAssertion.DataAnnotations;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class ShouldExtensionAttribute : Attribute
{
    public Type ActualValueType { get; }

    public Type[]? TypeArgs { get; init; }

    public int OverloadResolutionPriority { get; init; }

    public ShouldExtensionAttribute(Type actualValueType)
    {
        ActualValueType = actualValueType;
    }
}
