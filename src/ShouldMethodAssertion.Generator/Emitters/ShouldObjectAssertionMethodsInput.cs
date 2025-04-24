using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldObjectAssertionMethodsInput(
    PartialDefinitionTypeWithActualValueType ShouldObjectType,
    PartialDefinitionTypeWithActualValueType ShouldMethodDefinitionType,
    EquatableArray<CsMethod> ShouldMethods,
    CsTypeRef StringType,
    CsTypeRef CallerArgumentExpressionAttributeType,
    string? ActualValueConvertMethodName,
    string? WarningMessage);
