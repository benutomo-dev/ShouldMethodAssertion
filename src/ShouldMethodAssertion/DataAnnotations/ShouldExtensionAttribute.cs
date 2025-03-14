namespace ShouldMethodAssertion.DataAnnotations;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class ShouldExtensionAttribute : Attribute
{
    public Type ActualValueType { get; }

    public ShouldExtensionAttribute(Type actualValueType)
    {
        ActualValueType = actualValueType;
    }
}
