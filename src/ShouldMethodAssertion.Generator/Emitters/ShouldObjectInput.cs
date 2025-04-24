namespace ShouldMethodAssertion.Generator.Emitters;

record struct ShouldObjectInput(
    PartialDefinitionTypeWithActualValueType ShouldObjectType,
    PartialDefinitionTypeWithActualValueType? NullableTShouldObjectType
    );
