using System.Diagnostics.CodeAnalysis;

namespace ShouldMethodAssertion.ShouldAssertionContexts;

public struct ValueExpression
{
    public string Default => _indentAdjustmentedValue ??= ExpressionUtil.AdjustExpressionIndent(_rawValue);

    public string OneLine => _oneLineValue ??= ExpressionUtil.ToOneLineExpression(_rawValue);

    public bool IsMultiLine => _rawValue.Contains('\n');

    public bool HasBrackets => ExpressionUtil.HasBracketsExpression(_rawValue);

    private string _rawValue;

    private string? _indentAdjustmentedValue;
    private string? _oneLineValue;
    public ValueExpression(string rawValue) : this()
    {
        _rawValue = rawValue;
    }
}


public struct NullableValueExpression
{
    [MemberNotNullWhen(true, nameof(Default), nameof(OneLine))]
    public bool HasValue => _rawValue is not null;

    public string? Default => _indentAdjustmentedValue ??= ExpressionUtil.AdjustExpressionIndent(_rawValue);

    public string? OneLine => _oneLineValue ??= ExpressionUtil.ToOneLineExpression(_rawValue);

    [MemberNotNullWhen(true, nameof(Default), nameof(OneLine))]
    public bool IsMultiLine => _rawValue?.Contains('\n') ?? false;

    [MemberNotNullWhen(true, nameof(Default), nameof(OneLine))]
    public bool HasBrackets => ExpressionUtil.HasBracketsExpression(_rawValue);

    private string? _rawValue;

    private string? _indentAdjustmentedValue;
    private string? _oneLineValue;

    public NullableValueExpression(string? rawValue) : this()
    {
        _rawValue = rawValue;
    }
}
