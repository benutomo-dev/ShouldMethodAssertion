using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldExtensionInput(
    CsTypeRef ShouldObjectType,
    CsTypeRefWithAnnotation ActualValueType,
    CsTypeRef StringType,
    CsTypeRef? NotNullAttributeType,
    CsTypeRef CallerArgumentExpressionAttributeType
    );
