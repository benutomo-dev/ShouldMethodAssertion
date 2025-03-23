using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldObjectAssertionMethodsInput(
    CsTypeRef PartialDefinitionType,
    CsTypeRefWithAnnotation ShouldObjectActualValueType,
    CsTypeRef StringType,
    CsTypeRef CallerArgumentExpressionAttributeType,
    CsTypeRef ShouldMethodDefinitionType,
    string? ActualValueConvertMethodName,

    CsTypeRefWithAnnotation? ShouldMethodDefinitionActualValueType,
    EquatableArray<CsMethod> ShouldMethods,
    string? WarningMessage);
