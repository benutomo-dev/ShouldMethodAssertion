# Memo

## 主要な型

### ShouldAssertionContext(仮)

```csharp
// ShouldAssertionContext<T>のTは実際値の型
// Span<T>などジェネリック型にできないref構造体はShouldAssertionContextSpan<T>のようなShouldAssertionContext<T>と等価になる専用の型をソースジェネレータで生成
public readonly ref struct ShouldAssertionContext<T>
{
    public T Actual { get; }

    public string ActualExpression { get; }

    private readonly (string name, string expression) _param1;
    private readonly (string name, string expression) _param2;
    private readonly (string name, string expression) _param3;

    private readonly Dictionary<string, string>? _extraParamsExpressions;

    public ShouldAssertionContext(T actual, string actualExpression, (string name, string expression) param1, (string name, string expression) param2, (string name, string expression) param3, Dictionary<string, string>? extraParamsExpressions)
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
```

### 検証メソッドの定義構造体型であることを宣言する属性
```csharp
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class ShouldMethodDefinitionAttribute : Attribute
{
    public Type ActualValueType { get; }

    public ShouldMethodDefinitionAttribute(Type actualValueType)
    {
        ActualValueType = actualValueType;
    }
}
```

```csharp
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class ShouldExtensionAttribute : Attribute
{
    public Type ActualValueType { get; }

    public ShouldExtensionAttribute(Type actualValueType)
    {
        ActualValueType = actualValueType;
    }
}
```

```csharp
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public class ShouldMethodAttribute : Attribute
{
    public Type ShouldMethodDefinitionType { get; }

    public Type[]? TypeArgs { get; init; }

    public ShouldMethodAttribute(Type shouldMethodDefinitionType)
    {
        ShouldMethodDefinitionType = shouldMethodDefinitionType;
    } 
}
```

## 検証メソッドの定義

```csharp
[ShouldMethodDefinition(typeof(object))] // 属性でobject型の検証メソッドの定義であること宣言
struct ObjectShouldBe
{
    // publicで"Should"から開始するインスタンスメソッドを定義する
    // ジェネリックメソッドでも良い(型制約も可)
    // デフォルト引数も可
    // 自動生成＆設定されるContextで各引数の呼び出し時の式を参照可
    public void ShouldBe<T>(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is T actual && comparer.Equals(actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is not {ParamExpressions.expected}.");
    }

    // 複数定義可
    public void ShouldNotBe<T>(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is not T actual || !comparer.Equals(actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is {ParamExpressions.expected}.");
    }
}

#region ShouldMethodDefinition属性に対してソースジェネレータが生成
public readonly ref partial struct ObjectShouldBe
{
    private ShouldAssertionContext<object> Context { get; init; }

    public ObjectShouldBe(ShouldAssertionContext<object> context)
    {
        Context = context;
    }
}
#endregion
```

```csharp
[ShouldMethodDefinition(typeof(object))]
public readonly ref struct ObjectShouldSameReferenceAs
{
    public void ShouldSameReferenceAs<T>(T expected) where T : class
    {
        if (ReferenceEquals(Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is not same as {ParamExpressions.expected}.");
    }

    public void ShouldNotSameReferenceAs<T>(T expected) where T : class
    {
        if (!ReferenceEquals(Actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is same as {ParamExpressions.expected}.");
    }
}

#region [ShouldMethodDefinition(typeof(object))]に対してソースジェネレータが生成
public readonly ref struct ObjectShouldSameReferenceAs
{
    private ShouldAssertionContext<object> Context { get; init; }

    public ObjectShouldSameReferenceAs(ShouldAssertionContext<object> context)
    {
        Context = context;
    }
}
#endregion
```

## Should拡張メソッドの定義

### object

```csharp
[ShouldExtension(typeof(object))]                   // Should()拡張メソッドの対象とする型はobject
[ShouldMethod(typeof(ObjectShouldBe))]              // ObjectShouldBeの検証メソッドを実装する
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))] // ObjectShouldSameReferenceAsの検証メソッドを実装する
partial struct ShouldObject {}

#region [ShouldExtension(typeof(object))]に対してソースジェネレータが生成
public readonly ref partial struct ShouldObject
{
    private object Actual { get; }

    private string? ActualExpression { get; }

    public ShouldObject(object actual, string? actualExpression)
    {
        Actual = actual;
        ActualExpression = actualExpression;
    }
}
#endregion


#region [ShouldMethod(typeof(ObjectShouldBe))]に対してソースジェネレータが生成
partial struct ShouldObject
{
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
}
#endregion

#region [ShouldMethod(typeof(ObjectShouldSameReferenceAs))]に対してソースジェネレータが生成
partial struct ShouldObject
{
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
}
#endregion
```

### IEnumerable&lt;T&gt;

```csharp
[ShouldExtension(typeof(IEnumerable<>))]            // Should()拡張メソッドの対象とする型はIEnumerable<T>
[ShouldMethod(typeof(EnumerableShouldEquals<>))]    // EnumerableShouldEquals<T>の検証メソッドを実装する
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))] // ObjectShouldSameReferenceAsの検証メソッドを実装する(ObjectShouldSameReferenceAsの実際値の型はobjectだが、IEnuemerable<T>はobjectに継承関係で代入可能なのでOK)
public readonly ref partial struct ShouldEnumerable<T>
{
}

#region [ShouldExtension(typeof(IEnumerable<>))]に対してソースジェネレータが生成
// ShouldExtension属性にオープンジェネリック型が指定する場合は構造体の定義もジェネリック型にする。
// ShouldExtension属性のオープンジェネリック型の型パラメータは構造体の同じ位置の型パラメータに対応する。
public readonly ref partial struct ShouldEnumerable<T>
{
    private IEnumerable<T> Actual { get; }

    private string? ActualExpression { get; }

    public ShouldObject(IEnumerable<T> actual, string? actualExpression)
    {
        Actual = actual;
        ActualExpression = actualExpression;
    }
}
#endregion

#region [ShouldMethod(typeof(EnumerableShouldEquals<>))]に対してソースジェネレータが生成
partial struct ShouldEnumerable<T>
{
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
}
#endregion

#region [ShouldMethod(typeof(ObjectShouldSameReferenceAs))]に対してソースジェネレータが生成
partial struct ShouldEnumerable<T>
{
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
}
#endregion
```
