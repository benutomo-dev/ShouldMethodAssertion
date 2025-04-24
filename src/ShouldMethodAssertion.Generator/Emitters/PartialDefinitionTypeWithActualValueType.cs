using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct PartialDefinitionTypeWithActualValueType(
    CsTypeRef PartialDefinitionType,
    CsTypeRefWithAnnotation ActualValueType
    );
