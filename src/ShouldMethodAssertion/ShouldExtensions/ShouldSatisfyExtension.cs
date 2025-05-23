﻿using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;
using System.Runtime.CompilerServices;

namespace ShouldMethodAssertion.ShouldExtensions;

public static class ShouldSatisfyExtension
{
    public static void ShouldSatisfy<T>(this T actual, Func<T, bool> predicate, [CallerArgumentExpression(nameof(actual))] string? actualCallerArgumentExpression = null, [CallerArgumentExpression(nameof(predicate))] string? predicateCallerArgumentExpression = null)
    {
        if (!predicate(actual))
        {
            var _actualCallerArgumentExpression = new ValueExpression(actualCallerArgumentExpression ?? nameof(actual));
            var _predicateCallerArgumentExpression = new ValueExpression(predicateCallerArgumentExpression ?? nameof(actual));

            throw AssertExceptionUtil.Create($"{_actualCallerArgumentExpression.OneLine} is not satisfy {_predicateCallerArgumentExpression.OneLine}.");
        }
    }
}
