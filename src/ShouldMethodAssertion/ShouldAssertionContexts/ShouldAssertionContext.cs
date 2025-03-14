namespace ShouldMethodAssertion.ShouldAssertionContexts;

public readonly struct ShouldAssertionContext<T> // ref構造体以外の共通用
{
    public T Actual { get; }

    public string ActualExpression { get; }

    private readonly (string name, string? expression) _param1;
    private readonly (string name, string? expression) _param2;
    private readonly (string name, string? expression) _param3;

    private readonly Dictionary<string, string?>? _extraParamsExpressions;

    public ShouldAssertionContext(T actual, string actualExpression, (string name, string? expression) param1, (string name, string? expression) param2, (string name, string? expression) param3, Dictionary<string, string?>? extraParamsExpressions)
    {
        Actual = actual;
        ActualExpression = actualExpression;
        _param1 = param1;
        _param2 = param2;
        _param3 = param3;
        _extraParamsExpressions = extraParamsExpressions;
    }

    public string? GetExpressionOf(string paramName)
    {
        if (_param1.name == paramName)
            return _param1.expression;

        if (_param2.name == paramName)
            return _param2.expression;

        if (_param3.name == paramName)
            return _param3.expression;

        if (_extraParamsExpressions is not null && _extraParamsExpressions.TryGetValue(paramName, out var expression))
            return expression;

        throw new ArgumentException(null, nameof(paramName));
    }
}
