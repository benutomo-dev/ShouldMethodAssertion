namespace ShouldMethodAssertion.Generator;

internal static class GlobalReferences
{
    internal const string ExceptionCreateCall = $"global::{NameSpaces.ShouldMethodDefinitions}.AssertExceptionUtil.Create";

    internal const string ValueExpression = $"global::{NameSpaces.ShouldAssertionContexts}.ValueExpression";
    internal const string NullableValueExpression = $"global::{NameSpaces.ShouldAssertionContexts}.NullableValueExpression";
}
