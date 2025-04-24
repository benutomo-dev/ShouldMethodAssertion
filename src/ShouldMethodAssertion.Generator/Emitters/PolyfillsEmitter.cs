using Microsoft.CodeAnalysis;

namespace ShouldMethodAssertion.Generator.Emitters;

internal static class PolyfillsEmitter
{
    private const string OverloadResolutionPriorityAttributeSource = """
        namespace System.Runtime.CompilerServices;

        using Diagnostics;
        using Diagnostics.CodeAnalysis;

        [ExcludeFromCodeCoverage]
        [AttributeUsage(
            AttributeTargets.Method |
            AttributeTargets.Constructor |
            AttributeTargets.Property,
            Inherited = false)]
        internal sealed class OverloadResolutionPriorityAttribute : Attribute
        {
            public OverloadResolutionPriorityAttribute(int priority) => Priority = priority;

            public int Priority { get; }
        }
        """;

    public static void Emit(SourceProductionContext context, string args)
    {
        if (args == MetadataNames.OverloadResolutionPriorityAttribute)
        {
            context.AddSource("Polyfills/OverloadResolutionPriorityAttribute.cs", OverloadResolutionPriorityAttributeSource);
        }
    }
}
