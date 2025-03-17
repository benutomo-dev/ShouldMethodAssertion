using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldObjectInput(
    CsTypeReference PartialDefinitionType,
    CsTypeRefWithNullability ActualValueType
    );
