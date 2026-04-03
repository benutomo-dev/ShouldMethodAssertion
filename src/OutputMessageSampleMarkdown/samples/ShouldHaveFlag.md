# ShouldHaveFlag

## Should().HaveFlag()

**TestCode**

```csharp
var actualValue = SampleFlags.Read;
var expectedValue = SampleFlags.Write;

actualValue.Should().HaveFlag(expectedValue);
```

**Message**

```text
`actualValue` does not have `expectedValue`.
```

**TestCode**

```csharp
var actualValue = SampleFlags.Read;

actualValue.Should().HaveFlag(SampleFlags.Write);
```

**Message**

```text
`actualValue` does not have `SampleFlags.Write`.
```

## Should().NotHaveFlag()

**TestCode**

```csharp
var actualValue = SampleFlags.Read | SampleFlags.Write;
var expectedValue = SampleFlags.Write;

actualValue.Should().NotHaveFlag(expectedValue);
```

**Message**

```text
`actualValue` has `expectedValue`.
```

**TestCode**

```csharp
var actualValue = SampleFlags.Read | SampleFlags.Write;

actualValue.Should().NotHaveFlag(SampleFlags.Write);
```

**Message**

```text
`actualValue` has `SampleFlags.Write`.
```

