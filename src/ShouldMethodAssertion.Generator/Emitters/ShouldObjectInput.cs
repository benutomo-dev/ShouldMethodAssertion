using SourceGeneratorCommons.CSharp.Declarations;

namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldObjectInput(
    CsTypeRef ShouldObjectType,
    CsTypeRefWithAnnotation ActualValueType
    );
