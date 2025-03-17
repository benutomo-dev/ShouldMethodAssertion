# Memo

## ��v�Ȍ^

### ShouldAssertionContext(��)

```csharp
// ShouldAssertionContext<T>��T�͎��ےl�̌^
// Span<T>�ȂǃW�F�l���b�N�^�ɂł��Ȃ�ref�\���̂�ShouldAssertionContextSpan<T>�̂悤��ShouldAssertionContext<T>�Ɠ����ɂȂ��p�̌^���\�[�X�W�F�l���[�^�Ő���
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

### ���؃��\�b�h�̒�`�\���̌^�ł��邱�Ƃ�錾���鑮��
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

## ���؃��\�b�h�̒�`

```csharp
[ShouldMethodDefinition(typeof(object))] // ������object�^�̌��؃��\�b�h�̒�`�ł��邱�Ɛ錾
struct ObjectShouldBe
{
    // public��"Should"����J�n����C���X�^���X���\�b�h���`����
    // �W�F�l���b�N���\�b�h�ł��ǂ�(�^�������)
    // �f�t�H���g��������
    // �����������ݒ肳���Context�Ŋe�����̌Ăяo�����̎����Q�Ɖ�
    public void ShouldBe<T>(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is T actual && comparer.Equals(actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is not {ParamExpressions.expected}.");
    }

    // ������`��
    public void ShouldNotBe<T>(T expected, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        if (Actual is not T actual || !comparer.Equals(actual, expected))
            return;

        throw AssertExceptionUtil.Create($"{ActualExpression} is {ParamExpressions.expected}.");
    }
}

#region ShouldMethodDefinition�����ɑ΂��ă\�[�X�W�F�l���[�^������
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

#region [ShouldMethodDefinition(typeof(object))]�ɑ΂��ă\�[�X�W�F�l���[�^������
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

## Should�g�����\�b�h�̒�`

### object

```csharp
[ShouldExtension(typeof(object))]                   // Should()�g�����\�b�h�̑ΏۂƂ���^��object
[ShouldMethod(typeof(ObjectShouldBe))]              // ObjectShouldBe�̌��؃��\�b�h����������
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))] // ObjectShouldSameReferenceAs�̌��؃��\�b�h����������
partial struct ShouldObject {}

#region [ShouldExtension(typeof(object))]�ɑ΂��ă\�[�X�W�F�l���[�^������
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


#region [ShouldMethod(typeof(ObjectShouldBe))]�ɑ΂��ă\�[�X�W�F�l���[�^������
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

#region [ShouldMethod(typeof(ObjectShouldSameReferenceAs))]�ɑ΂��ă\�[�X�W�F�l���[�^������
partial struct ShouldObject
{
    public void SameReferenceAs<T>(T expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where T : class // ObjectShouldSameAs�Œ�`����郁�\�b�h�Ɠ����^����
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

    public void NotSameReferenceAs<T>(T expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where T : class // ObjectShouldSameAs�Œ�`����郁�\�b�h�Ɠ����^����
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
[ShouldExtension(typeof(IEnumerable<>))]            // Should()�g�����\�b�h�̑ΏۂƂ���^��IEnumerable<T>
[ShouldMethod(typeof(EnumerableShouldEquals<>))]    // EnumerableShouldEquals<T>�̌��؃��\�b�h����������
[ShouldMethod(typeof(ObjectShouldSameReferenceAs))] // ObjectShouldSameReferenceAs�̌��؃��\�b�h����������(ObjectShouldSameReferenceAs�̎��ےl�̌^��object�����AIEnuemerable<T>��object�Ɍp���֌W�ő���\�Ȃ̂�OK)
public readonly ref partial struct ShouldEnumerable<T>
{
}

#region [ShouldExtension(typeof(IEnumerable<>))]�ɑ΂��ă\�[�X�W�F�l���[�^������
// ShouldExtension�����ɃI�[�v���W�F�l���b�N�^���w�肷��ꍇ�͍\���̂̒�`���W�F�l���b�N�^�ɂ���B
// ShouldExtension�����̃I�[�v���W�F�l���b�N�^�̌^�p�����[�^�͍\���̂̓����ʒu�̌^�p�����[�^�ɑΉ�����B
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

#region [ShouldMethod(typeof(EnumerableShouldEquals<>))]�ɑ΂��ă\�[�X�W�F�l���[�^������
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

#region [ShouldMethod(typeof(ObjectShouldSameReferenceAs))]�ɑ΂��ă\�[�X�W�F�l���[�^������
partial struct ShouldEnumerable<T>
{
    public void SameReferenceAs<TExpected>(TExpected expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where TExpected : class // ObjectShouldSameAs�Œ�`����郁�\�b�h�Ɠ����^����(���O�̏d����T->TExpected�Ɏ���������K�v)
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

    public void NotSameReferenceAs<TExpected>(TExpected expected, [CallerArgumentExpression(nameof(expected))] string? expectedExpression = null) where TExpected : class // ObjectShouldSameAs�Œ�`����郁�\�b�h�Ɠ����^����(���O�̏d����T->TExpected�Ɏ���������K�v)
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
