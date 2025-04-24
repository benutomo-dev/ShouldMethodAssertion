# ShouldHaveValue

## Should().HaveValue()

### struct (System.Nullable&lt;T&gt;)

**TestCode**

```csharp
var actualValue = default(int?);

actualValue.Should().HaveValue();
```

**Message**

```
`actualValue` is null.
```

## Should().NotHaveValue()

### struct (System.Nullable&lt;T&gt;)

**TestCode**

```csharp
var actualValue = (int?)1;

actualValue.Should().NotHaveValue();
```

**Message**

```
`actualValue` is not null.

[Actual]
`1`
```

