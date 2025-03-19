namespace ShouldMethodAssertion.Generator;

internal static class MetadataNames
{
    internal const string ActualValueType = $"{NameSpaces.DataAnnotations}.ActualValueType";

    internal const string TypeArg1 = $"{NameSpaces.DataAnnotations}.TypeArg1";
    internal const string TypeArg2 = $"{NameSpaces.DataAnnotations}.TypeArg2";
    internal const string TypeArg3 = $"{NameSpaces.DataAnnotations}.TypeArg3";
    internal const string TypeArg4 = $"{NameSpaces.DataAnnotations}.TypeArg4";

    internal const string ShouldExtensionAttribute        = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldExtensionAttribute}";
    internal const string ShouldMethodAttribute           = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldMethodAttribute}";
    internal const string ShouldMethodDefinitionAttribute = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldMethodDefinitionAttribute}";

    internal const string CallerArgumentExpressionAttribute = $"System.Runtime.CompilerServices.CallerArgumentExpressionAttribute";
    internal const string NotNullAttribute                  = $"System.Diagnostics.CodeAnalysis.NotNullAttribute";
}
