using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldExtensionInput(
    CsTypeReference PartialDefinitionType,
    CsTypeRefWithNullability ActualValueType,
    CsTypeRefWithNullability? RawActualValueType,
    EquatableArray<CsGenericTypeParam> ActualValueTypeGenericTypeParams,
    CsTypeReference StringType,
    CsTypeReference CallerArgumentExpressionAttributeType
    );
