namespace ShouldMethodAssertion.DataAnnotations;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public class ShouldMethodAttribute : Attribute
{
    public Type ShouldMethodDefinitionType { get; }

    public Type[]? TypeArgs { get; init; }

    public ShouldMethodAttribute(Type shouldMethodDefinitionType)
    {
        ShouldMethodDefinitionType = shouldMethodDefinitionType;
    } 
}
