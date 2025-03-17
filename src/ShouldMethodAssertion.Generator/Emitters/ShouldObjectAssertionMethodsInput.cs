using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldObjectAssertionMethodsInput(
    CsTypeReference PartialDefinitionType,
    CsTypeRefWithNullability ShouldObjectActualValueType,
    CsTypeReference StringType,
    CsTypeReference CallerArgumentExpressionAttributeType,
    CsTypeReference ShouldMethodDefinitionType,
    string? ActualValueConvertMethodName,

    CsTypeRefWithNullability? ShouldMethodDefinitionActualValueType,
    CsTypeReference? ShouldAssertionContextType,
    EquatableArray<CsMethod> ShouldMethods,
    string? WarningMessage);
