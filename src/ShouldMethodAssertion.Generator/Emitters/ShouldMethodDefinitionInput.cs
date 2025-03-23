using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldMethodDefinitionInput(
    CsTypeRef PartialDefinitionType,
    EquatableArray<(string Name, bool MayBeNull)> MethodParameters,
    CsTypeRefWithAnnotation ActualValueType
    );
