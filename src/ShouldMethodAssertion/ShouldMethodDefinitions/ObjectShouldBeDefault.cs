using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object), AcceptNullReference = true)]
public partial struct ObjectShouldBeDefault
{
    public void ShouldBeDefault()
    {
        if (!IsDefault(Actual))
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is not default.");
    }

    public void ShouldNotBeDefault()
    {
        if (IsDefault(Actual))
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is default.");
    }

    private static bool IsDefault(object? value)
    {
        if (value is null)
            return true;

        var actualValueType = value.GetType();

        if (!actualValueType.IsValueType)
            return false;

        if (actualValueType.IsGenericType && actualValueType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return (bool)actualValueType.GetProperty("HasValue")!.GetValue(actualValueType)!;
        }
        else
        {
#if NETFRAMEWORK
            var defaultValue = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(actualValueType);
#else
            var defaultValue = System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(actualValueType);
#endif
            return defaultValue.Equals(value);
        }
    }
}
