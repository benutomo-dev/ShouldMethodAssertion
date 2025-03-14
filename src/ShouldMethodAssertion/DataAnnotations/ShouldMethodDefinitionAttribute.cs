namespace ShouldMethodAssertion.DataAnnotations;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class ShouldMethodDefinitionAttribute : Attribute
{
    public Type ActualValueType { get; }

    public bool AcceptNullReference { get; set; }

    public ShouldMethodDefinitionAttribute(Type actualValueType)
    {
        ActualValueType = actualValueType;
    }
}
