namespace ShouldMethodAssertion.ShouldAssertionContexts;

public struct ValueExpression
{
    public string Default => _indentAdjustmentedValue ??= ExpressionUtil.AdjustExpressionIndent(_rawValue, withComplementBruckets: true);

    public string OneLine => _oneLineValue ??= ExpressionUtil.ToOneLineExpression(_rawValue, withComplementBruckets: true);

    public bool IsMultiLine => _rawValue.Contains('\n');

    public bool HasBrackets => ExpressionUtil.HasBracketsExpression(_rawValue);

    private string _rawValue;

    private string? _indentAdjustmentedValue;
    private string? _oneLineValue;

    public ValueExpression(string rawValue) : this()
    {
        _rawValue = rawValue;
    }

    public static implicit operator ValueExpression(string rawValue) => new ValueExpression(rawValue);

    public static implicit operator string(ValueExpression valueExpression) => valueExpression.Default;


    public override string ToString() => Default;
}
