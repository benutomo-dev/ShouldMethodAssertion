using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldObjectInput(
    CsTypeRef PartialDefinitionType,
    CsTypeRefWithAnnotation ActualValueType
    );
