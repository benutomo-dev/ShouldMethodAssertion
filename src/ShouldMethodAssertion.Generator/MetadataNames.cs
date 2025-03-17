namespace ShouldMethodAssertion.Generator;

internal static class MetadataNames
{
    internal const string ShouldAssertionContext          = $"{NameSpaces.ShouldAssertionContexts}.ShouldAssertionContext`1";
    internal const string ShouldExtensionAttribute        = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldExtensionAttribute}";
    internal const string ShouldMethodAttribute           = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldMethodAttribute}";
    internal const string ShouldMethodDefinitionAttribute = $"{NameSpaces.DataAnnotations}.{HintingAttributeSymbolNames.ShouldMethodDefinitionAttribute}";

    internal const string CallerArgumentExpressionAttribute = $"System.Runtime.CompilerServices.CallerArgumentExpressionAttribute";
}
