using SourceGeneratorCommons.Collections.Generic;
using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldExtensionInput(
    EquatableArray<ShouldExtensionMethodInfo> ShouldExtensionMethodInfos,
    string? ShouldMethodDefinitionNameSpace,
    string ShouldMethodDefinitionClassName,
    CsTypeRef StringType,
    CsTypeRef? NotNullAttributeType,
    CsTypeRef CallerArgumentExpressionAttributeType
    );
