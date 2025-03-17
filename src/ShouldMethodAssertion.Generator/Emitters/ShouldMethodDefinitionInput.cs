using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldMethodDefinitionInput(
    CsTypeReference PartialDefinitionType,
    EquatableArray<(string Name, bool MayBeNull)> MethodParameters,
    CsTypeRefWithNullability ActualValueType,
    [property: Obsolete]
    CsTypeReference ShouldAssertionContextType,
    bool IsGeneratedShouldAssertionContextType
    );
