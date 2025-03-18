namespace ShouldMethodAssertion.Generator;

internal static class MetadataNames
{
    internal const string ShouldExtensionAttribute        = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldExtensionAttribute}";
    internal const string ShouldMethodAttribute           = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldMethodAttribute}";
    internal const string ShouldMethodDefinitionAttribute = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldMethodDefinitionAttribute}";

    internal const string CallerArgumentExpressionAttribute = $"System.Runtime.CompilerServices.CallerArgumentExpressionAttribute";
    internal const string NotNullAttribute                  = $"System.Diagnostics.CodeAnalysis.NotNullAttribute";
}
