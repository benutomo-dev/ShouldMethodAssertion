namespace ShouldMethodAssertion.DataAnnotations;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public class ShouldMethodAttribute : Attribute
{
    public Type ShouldMethodDefinitionType { get; }

    public string? ConvertBy { get; init; }

    public ShouldMethodAttribute(Type shouldMethodDefinitionType)
    {
        ShouldMethodDefinitionType = shouldMethodDefinitionType;
    } 
}
