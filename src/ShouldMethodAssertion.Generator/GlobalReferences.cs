namespace ShouldMethodAssertion.Generator;

internal static class GlobalReferences
{
    internal const string ExceptionCreateCall = $"global::{NameSpaces.ShouldMethodDefinitions}.AssertExceptionUtil.Create";

    internal const string ValueExpression         = $"global::{NameSpaces.ExpressionUtils}.ValueExpression";
    internal const string NullableValueExpression = $"global::{NameSpaces.ExpressionUtils}.NullableValueExpression";
}
