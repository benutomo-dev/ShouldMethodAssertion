namespace ShouldMethodAssertion.Tests;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
internal sealed class TestReferenceAttribute : Attribute
{
    public TestReferenceAttribute(string[] mainTestees) { }
}

internal sealed class TypeArg { }

internal struct StructTypeArg { }
