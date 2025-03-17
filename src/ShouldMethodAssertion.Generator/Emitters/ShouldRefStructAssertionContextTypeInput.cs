using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldRefStructAssertionContextTypeInput(
    CsTypeReference ShouldAssertionContextType,
    CsTypeRefWithNullability ActualValueType);
