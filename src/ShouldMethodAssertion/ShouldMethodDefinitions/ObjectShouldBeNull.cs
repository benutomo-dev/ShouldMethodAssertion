﻿using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object), AcceptNullReference = true)]
public partial struct ObjectShouldBeNull
{
    public void ShouldBeNull()
    {
        if (!IsNull(Context.Actual))
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not null.");
    }

    public void ShouldNotBeNull()
    {
        if (IsNull(Context.Actual))
            throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is null.");
    }

    private static bool IsNull<T>(T value)
    {
        if (value is null)
            return true;

        var actualValueType = value.GetType();

        if (!actualValueType.IsValueType)
            return false;

        if (!actualValueType.IsGenericType)
            return false;

        if (actualValueType.GetGenericTypeDefinition() != typeof(Nullable<>))
            return false;

        var hasValue = (bool)actualValueType.GetProperty("HasValue")!.GetValue(actualValueType)!;

        if (hasValue)
            return false;

        return true;
    }
}
