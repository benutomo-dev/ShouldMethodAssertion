using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldExtensionInput(
    CsTypeReference PartialDefinitionType,
    CsTypeRefWithNullability ActualValueType,
    CsTypeReference StringType,
    CsTypeReference? NotNullAttributeType,
    CsTypeReference CallerArgumentExpressionAttributeType
    );
