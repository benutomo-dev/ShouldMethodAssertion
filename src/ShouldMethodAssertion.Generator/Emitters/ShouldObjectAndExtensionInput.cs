using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldObjectAndExtensionInput(
    CsTypeReference PartialDefinitionType,
    CsTypeRefWithNullability ActualValueType,
    CsTypeRefWithNullability? RawActualValueType,
    EquatableArray<CsGenericTypeParam> ActualValueTypeGenericTypeParams,
    CsTypeReference StringType,
    CsTypeReference CallerArgumentExpressionAttributeType
    );
