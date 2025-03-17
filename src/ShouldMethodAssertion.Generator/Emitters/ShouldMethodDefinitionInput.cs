using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldMethodDefinitionInput(
    CsTypeReference PartialDefinitionType,
    CsTypeRefWithNullability ActualValueType,
    CsTypeReference ShouldAssertionContextType,
    bool IsGeneratedShouldAssertionContextType
    );
