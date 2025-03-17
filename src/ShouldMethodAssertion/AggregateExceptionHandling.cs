namespace ShouldMethodAssertion;

/// <summary>
/// AggregateException内の期待する例外をどのように扱うかを指定するオプション
/// </summary>
public enum AggregateExceptionHandling
{
    /// <summary>
    /// AggregateExceptionは期待する例外と常に異なるものとして扱います。
    /// AggregateExceptionの内部例外に期待する例外が含まれていても、検証は失敗します。
    /// </summary>
    None = 0,

    /// <summary>
    /// AggregateExceptionの直接の内部例外に期待する例外が唯一つだけ含まれる場合に限り、
    /// 期待する例外として扱います。他の例外が含まれる場合や、ネストされたAggregateExceptionは検証が失敗します。
    /// </summary>
    SingleDirect = 1,

    /// <summary>
    /// AggregateException.Flatten()の結果、期待する例外が唯一つだけ含まれる場合に限り、
    /// 期待する例外として扱います。ネストされたAggregateExceptionを含めて考慮します。
    /// </summary>
    SingleFlattened = 2,

    /// <summary>
    /// AggregateException.Flatten()の結果、期待する例外が少なくとも1つ含まれる場合、
    /// 期待する例外として扱います。他の例外が含まれていても検証は成功します。
    /// </summary>
    AnyFlattened = 3
}