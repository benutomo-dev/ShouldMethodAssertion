namespace ShouldMethodAssertion.GeneratedImages;

#if false
public readonly ref partial struct ShouldObject
{
    #region ShouldExtension属性が付けられている構造体にソースジェネレータで生成
    private object Actual { get; }

    private string? ActualExpression { get; }

    public ShouldObject(object actual, string? actualExpression)
    {
        Actual = actual;
        ActualExpression = actualExpression;
    }
    #endregion

    #region ShouldMethod属性のObjectShouldBeからソースジェネレータで生成
    public void Be<T>(T expected, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            default,
            null);

        var assertMethod = new ObjectShouldBe(context);

        assertMethod.ShouldBe(expected, comparer);
    }

    public void NotBe<T>(T expected, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            default,
            null);

        var assertMethod = new ObjectShouldBe(context);

        assertMethod.ShouldNotBe(expected, comparer);
    }
    #endregion

    #region ShouldMethod属性のObjectShouldSameAsからソースジェネレータで生成
    public void SameReferenceAs<T>(T expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where T : class // ObjectShouldSameAsで定義されるメソッドと同じ型制約
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            default,
            default,
            null);

        var assertMethod = new ObjectShouldSameReferenceAs(context);

        assertMethod.ShouldSameReferenceAs(expected);
    }

    public void NotSameReferenceAs<T>(T expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where T : class // ObjectShouldSameAsで定義されるメソッドと同じ型制約
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            default,
            default,
            null);

        var assertMethod = new ObjectShouldSameReferenceAs(context);

        assertMethod.ShouldNotSameReferenceAs(expected);
    }
    #endregion
}

[ShouldExtension(typeof(IEnumerable<>))]
[ShouldMethod(typeof(EnumerableShouldEquals<>))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))] // ShouldExtensionのTargetTypeがShouldMethodのActualValueTypeに代入可能であれば型引数なしのShouldExtensionが適用可能
public readonly ref partial struct ShouldEnumerable<T>(IEnumerable<T> actual, string? actualExpression) // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    #region ShouldExtension属性が付けられている構造体にソースジェネレータで生成
    private IEnumerable<T> Actual { get; } = actual;

    private string? ActualExpression { get; } = actualExpression;
    #endregion

    #region ShouldExtension属性のCollectionShouldEquals<>からソースジェネレータで生成
    public void Equal(IEnumerable<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(ignoreOrder))] string? ignoreOrderExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<IEnumerable<T>>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("ignoreOrder", comparerExpression ?? $"\"{comparerExpression}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            null);

        var assertMethod = new EnumerableShouldEquals<T>(context);

        assertMethod.ShouldEqual(expected, ignoreOrder, comparer);
    }

    public void NotEqual(IEnumerable<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(ignoreOrder))] string? ignoreOrderExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<IEnumerable<T>>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("ignoreOrder", comparerExpression ?? $"\"{comparerExpression}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            null);

        var assertMethod = new EnumerableShouldEquals<T>(context);

        assertMethod.ShouldNotEqual(expected, ignoreOrder, comparer);
    }
    #endregion

    #region ShouldExtension属性のObjectShouldSameAsからソースジェネレータで生成
    public void SameReferenceAs<TExpected>(TExpected expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where TExpected : class // ObjectShouldSameAsで定義されるメソッドと同じ型制約(名前の重複はT->TExpectedに自動回避が必要)
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            default,
            default,
            null);

        var assertMethod = new ObjectShouldSameReferenceAs(context);

        assertMethod.ShouldNotSameReferenceAs(expected);
    }

    public void NotSameReferenceAs<TExpected>(TExpected expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where TExpected : class // ObjectShouldSameAsで定義されるメソッドと同じ型制約(名前の重複はT->TExpectedに自動回避が必要)
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            default,
            default,
            null);

        var assertMethod = new ObjectShouldSameReferenceAs(context);

        assertMethod.ShouldNotSameReferenceAs(expected);
    }
    #endregion
}

[ShouldExtension(typeof(Span<>))]
[ShouldMethod(typeof(ReadOnlySpanShouldEquals<>))]
public readonly ref partial struct ShouldSpan<T>(Span<T> actual, string? actualExpression) // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    #region ShouldExtension属性が付けられている構造体にソースジェネレータで生成
    private Span<T> Actual { get; } = actual;

    private string? ActualExpression { get; } = actualExpression;
    #endregion

    #region ShouldExtension属性のCollectionShouldEquals<>からソースジェネレータで生成
    public void Equal(Span<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(ignoreOrder))] string? ignoreOrderExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContextSpan<T>(
            Actual,
            ActualExpression ?? $"{{ {string.Join(",", Actual.ToArray())} }}",
            ("expected", expectedExpression ?? $"{{ {string.Join(",", expected.ToArray())} }}"),
            ("ignoreOrder", comparerExpression ?? $"\"{comparerExpression}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            null);

        var assertMethod = new ReadOnlySpanShouldEquals<T>(context);

        assertMethod.ShouldEqual(expected, ignoreOrder, comparer);
    }

    public void NotEqual(Span<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(ignoreOrder))] string? ignoreOrderExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContextSpan<T>(
            Actual,
            ActualExpression ?? $"{{ {string.Join(",", Actual.ToArray())} }}",
            ("expected", expectedExpression ?? $"{{ {string.Join(",", expected.ToArray())} }}"),
            ("ignoreOrder", comparerExpression ?? $"\"{comparerExpression}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            null);

        var assertMethod = new ReadOnlySpanShouldEquals<T>(context);

        assertMethod.ShouldNotEqual(expected, ignoreOrder, comparer);
    }
    #endregion
}

[ShouldExtension(typeof(ReadOnlySpan<>))]
[ShouldMethod(typeof(ReadOnlySpanShouldEquals<>))]
public readonly ref partial struct ShouldReadOnlySpan<T>(ReadOnlySpan<T> actual, string? actualExpression) // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    #region ShouldExtension属性が付けられている構造体にソースジェネレータで生成
    private ReadOnlySpan<T> Actual { get; } = actual;

    private string? ActualExpression { get; } = actualExpression;
    #endregion

    #region ShouldExtension属性のCollectionShouldEquals<>からソースジェネレータで生成
    public void Equal(ReadOnlySpan<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(ignoreOrder))] string? ignoreOrderExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContextSpan<T>(
            Actual,
            ActualExpression ?? $"{{ {string.Join(",", Actual.ToArray())} }}",
            ("expected", expectedExpression ?? $"{{ {string.Join(",", expected.ToArray())} }}"),
            ("ignoreOrder", comparerExpression ?? $"\"{comparerExpression}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            null);

        var assertMethod = new ReadOnlySpanShouldEquals<T>(context);

        assertMethod.ShouldEqual(expected, ignoreOrder, comparer);
    }

    public void NotEqual(ReadOnlySpan<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(ignoreOrder))] string? ignoreOrderExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContextSpan<T>(
            Actual,
            ActualExpression ?? $"{{ {string.Join(",", Actual.ToArray())} }}",
            ("expected", expectedExpression ?? $"{{ {string.Join(",", expected.ToArray())} }}"),
            ("ignoreOrder", comparerExpression ?? $"\"{comparerExpression}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            null);

        var assertMethod = new ReadOnlySpanShouldEquals<T>(context);

        assertMethod.ShouldNotEqual(expected, ignoreOrder, comparer);
    }
    #endregion
}

[ShouldExtension(typeof(IComparable<>))]
[ShouldMethod(typeof(ObjectShouldBe))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))] // ShouldExtensionのTargetTypeがShouldMethodのActualValueTypeに代入可能であれば型引数なしのShouldExtensionが適用可能
[ShouldMethod(typeof(ComparableShouldCompare<>))]
public readonly ref partial struct ShouldComparable<T>(IComparable<T> actual, string? actualExpression)
{
    #region ShouldExtension属性が付けられている構造体にソースジェネレータで生成
    private IComparable<T> Actual { get; } = actual;

    private string? ActualExpression { get; } = actualExpression;
    #endregion

    #region ShouldExtension属性のObjectShouldBeからソースジェネレータで生成
    public void Be<T>(T expected, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            default,
            null);

        var assertMethod = new ObjectShouldBe(context);

        assertMethod.ShouldBe(expected, comparer);
    }

    public void NotBe<T>(T expected, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            default,
            null);

        var assertMethod = new ObjectShouldBe(context);

        assertMethod.ShouldNotBe(expected, comparer);
    }
    #endregion

    #region ShouldExtension属性のObjectShouldSameAsからソースジェネレータで生成
    public void SameReferenceAs<T>(T expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where T : class // ObjectShouldSameAsで定義されるメソッドと同じ型制約
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            default,
            default,
            null);

        var assertMethod = new ObjectShouldSameReferenceAs(context);

        assertMethod.ShouldSameReferenceAs(expected);
    }

    public void NotSameReferenceAs<T>(T expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where T : class // ObjectShouldSameAsで定義されるメソッドと同じ型制約
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            default,
            default,
            null);

        var assertMethod = new ObjectShouldSameReferenceAs(context);

        assertMethod.ShouldNotSameReferenceAs(expected);
    }
    #endregion

    #region ShouldExtension属性のCompararableShouldCompareからソースジェネレータで生成
    public void LessThan(T expected, IComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<IComparable<T>>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            default,
            null);

        var assertMethod = new ComparableShouldCompare<T>(context);

        assertMethod.ShouldLessThan(expected, comparer);
    }

    public void GreaterThan(T expected, IComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<IComparable<T>>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            default,
            null);

        var assertMethod = new ComparableShouldCompare<T>(context);

        assertMethod.ShouldGreaterThan(expected, comparer);
    }
    #endregion

}


[ShouldExtension(typeof(string))]
[ShouldMethod(typeof(ObjectShouldBe))]
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))]
public readonly ref partial struct ShouldString(string actual, string? actualExpression)
{
    #region ShouldExtension属性が付けられている構造体にソースジェネレータで生成
    private object Actual { get; } = actual;

    private string? ActualExpression { get; } = actualExpression;
    #endregion

    #region ShouldExtension属性のObjectShouldBeからソースジェネレータで生成
    public void Be<T>(T expected, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            default,
            null);

        var assertMethod = new ObjectShouldBe(context);

        assertMethod.ShouldBe(expected, comparer);
    }

    public void NotBe<T>(T expected, IEqualityComparer<T>? comparer = null, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null, [CallerArgumentExpression(nameof(comparer))] string? comparerExpression = null)
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            ("comparer", comparerExpression ?? $"\"{comparerExpression}\""),
            default,
            null);

        var assertMethod = new ObjectShouldBe(context);

        assertMethod.ShouldNotBe(expected, comparer);
    }
    #endregion

    #region ShouldExtension属性のObjectShouldSameAsからソースジェネレータで生成
    public void SameReferenceAs<T>(T expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where T : class // ObjectShouldSameAsで定義されるメソッドと同じ型制約
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            default,
            default,
            null);

        var assertMethod = new ObjectShouldSameReferenceAs(context);

        assertMethod.ShouldSameReferenceAs(expected);
    }

    public void NotSameReferenceAs<T>(T expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where T : class // ObjectShouldSameAsで定義されるメソッドと同じ型制約
    {
        var context = new ShouldAssertionContext<object>(
            Actual,
            ActualExpression ?? $"\"{Actual}\"",
            ("expected", expectedExpression ?? $"\"{expected}\""),
            default,
            default,
            null);

        var assertMethod = new ObjectShouldSameReferenceAs(context);

        assertMethod.ShouldNotSameReferenceAs(expected);
    }
    #endregion
}



#region ref構造体のShould()に対しては自動生成
public readonly ref struct ShouldAssertionContextSpan<T>
{
    public ReadOnlySpan<T> Actual { get; }

    public string ActualExpression { get; }

    private readonly (string name, string expression) _param1;
    private readonly (string name, string expression) _param2;
    private readonly (string name, string expression) _param3;

    private readonly Dictionary<string, string>? _extraParamsExpressions;

    public ShouldAssertionContextSpan(ReadOnlySpan<T> actual, string actualExpression, (string name, string expression) param1, (string name, string expression) param2, (string name, string expression) param3, Dictionary<string, string>? extraParamsExpressions)
    {
        Actual = actual;
        ActualExpression = actualExpression;
        _param1 = param1;
        _param2 = param2;
        _param3 = param3;
        _extraParamsExpressions = extraParamsExpressions;
    }

    public string GetExpressionOf(string paramName)
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
#endregion


[ShouldMethodDefinition(typeof(object))]
partial struct ObjectShouldBe
{
    #region ShouldMethod属性に対してソースジェネレータが生成
    private ShouldAssertionContext<object> Context { get; init; }

    public ObjectShouldBe(ShouldAssertionContext<object> context)
    {
        Context = context;
    }
    #endregion

    public void ShouldBe<T>(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Context.Actual is T actual && comparer.Equals(actual, expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldNotBe<T>(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Context.Actual is not T actual || !comparer.Equals(actual, expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}

[ShouldMethodDefinition(typeof(object))]
partial struct ObjectShouldSameReferenceAs
{
    #region ShouldMethod属性に対してソースジェネレータが生成
    private ShouldAssertionContext<object> Context { get; init; }

    public ObjectShouldSameReferenceAs(ShouldAssertionContext<object> context)
    {
        Context = context;
    }
    #endregion

    public void ShouldSameReferenceAs<T>(T expected) where T : class
    {
        if (ReferenceEquals(Context.Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not same as `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldNotSameReferenceAs<T>(T expected) where T : class
    {
        if (!ReferenceEquals(Context.Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is same as `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}


[ShouldMethodDefinition(typeof(IEnumerable<>))]
partial struct EnumerableShouldEquals<T> // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    #region ShouldMethod属性に対してソースジェネレータが生成
    private ShouldAssertionContext<IEnumerable<T>> Context { get; init; }

    public EnumerableShouldEquals(ShouldAssertionContext<IEnumerable<T>> context)
    {
        Context = context;
    }
    #endregion

    public void ShouldEqual(IEnumerable<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Context.Actual.SequenceEqual(expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldNotEqual(IEnumerable<T> expected, bool ignoreOrder, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (!Context.Actual.SequenceEqual(expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}

public class TypeParameter1 { }

[ShouldMethodDefinition(typeof(IComparable<>))]
partial struct ComparableShouldCompare<T> // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    #region ShouldMethod属性に対してソースジェネレータが生成
    private ShouldAssertionContext<IComparable<T>> Context { get; init; }

    public ComparableShouldCompare(ShouldAssertionContext<IComparable<T>> context)
    {
        Context = context;
    }
    #endregion

    public void ShouldLessThan(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Context.Actual.CompareTo(expected) < 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Context.Actual, expected) < 0)
                return;
        }

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not less than `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldGreaterThan(T expected, IComparer<T>? comparer = null)
    {
        if (comparer is null)
        {
            if (Context.Actual.CompareTo(expected) > 0)
                return;
        }
        else
        {
            if (comparer.Compare((T)Context.Actual, expected) > 0)
                return;
        }

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not greater than `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}

[ShouldMethodDefinition(typeof(ReadOnlySpan<>))]
partial struct ReadOnlySpanShouldEquals<T> // ShouldMethod属性で指定した型と同じ数と制約の型引数
{
    #region ShouldMethod属性に対してソースジェネレータが生成
    private ShouldAssertionContextSpan<T> Context { get; init; }

    public ReadOnlySpanShouldEquals(ShouldAssertionContextSpan<T> context)
    {
        Context = context;
    }
    #endregion

    public void ShouldEqual(ReadOnlySpan<T> expected, bool ignoreOrder = false, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Context.Actual.SequenceEqual(expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is not `{Context.GetExpressionOf(nameof(expected))}`.");
    }

    public void ShouldNotEqual(ReadOnlySpan<T> expected, bool ignoreOrder, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (!Context.Actual.SequenceEqual(expected, comparer))
            return;

        throw AssertExceptionUtil.Create($"`{Context.ActualExpression}` is `{Context.GetExpressionOf(nameof(expected))}`.");
    }
}

#region ShouldExtension属性が付けられている構造体に対してソースジェネレータで生成
public static class ShouldExtension
{
    public static T As<T>(this T actual) => actual;

    public static ShouldObject Should(this object actual, [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) => new ShouldObject(actual, actualExpression);

    public static ShouldEnumerable<T> Should<T>(this IEnumerable<T> actual, [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) => new ShouldEnumerable<T>(actual, actualExpression);

    public static ShouldComparable<T> Should<T>(this IComparable<T> actual, [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) => new ShouldComparable<T>(actual, actualExpression);

    public static ShouldSpan<T> Should<T>(this Span<T> actual, [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) => new ShouldSpan<T>(actual, actualExpression);

    public static ShouldReadOnlySpan<T> Should<T>(this ReadOnlySpan<T> actual, [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) => new ShouldReadOnlySpan<T>(actual, actualExpression);

    public static ShouldString Should(this string actual, [CallerArgumentExpression(nameof(actual))] string? actualExpression = null) => new ShouldString(actual, actualExpression);
}
#endregion

#endif