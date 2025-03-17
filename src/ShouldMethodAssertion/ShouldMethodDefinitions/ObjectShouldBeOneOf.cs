﻿using ShouldMethodAssertion.DataAnnotations;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(object))]
public partial struct ObjectShouldBeOneOf
{
    public void ShouldBeOneOf<T>(ReadOnlySpan<T> expectedList, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        foreach (var expected in expectedList)
        {
            if (Actual is T actual && comparer.Equals(actual, expected))
                return;
        }

        throw AssertExceptionUtil.Create($"{ActualExpression} is not one of {ParamExpressions.expectedList}.");
    }

    public void ShouldNotBeOneOf<T>(ReadOnlySpan<T> expectedList, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        foreach (var expected in expectedList)
        {
            if (Actual is T actual && comparer.Equals(actual, expected))
                throw AssertExceptionUtil.Create($"{ActualExpression} is one of {ParamExpressions.expectedList}.");
        }

        return;
    }
}
