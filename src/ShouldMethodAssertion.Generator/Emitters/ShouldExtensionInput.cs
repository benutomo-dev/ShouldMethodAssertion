using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldExtensionInput(
    CsTypeRef PartialDefinitionType,
    CsTypeRefWithAnnotation ActualValueType,
    CsTypeRef StringType,
    CsTypeRef? NotNullAttributeType,
    CsTypeRef CallerArgumentExpressionAttributeType
    );
