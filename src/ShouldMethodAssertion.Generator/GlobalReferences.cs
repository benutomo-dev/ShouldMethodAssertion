namespace ShouldMethodAssertion.Generator;

internal static class GlobalReferences
{
    internal const string ExceptionCreateMethod   = $"global::{NameSpaces.Utils}.AssertExceptionUtil.Create";

    internal const string ValueExpression         = $"global::{NameSpaces.Utils}.ValueExpression";
    internal const string NullableValueExpression = $"global::{NameSpaces.Utils}.NullableValueExpression";
}
