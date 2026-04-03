# ShouldHaveFlag

## Should().HaveFlag()

**TestCode**

```csharp
var actualValue = SampleFlags.Read;
var expectedValue = SampleFlags.Write;

actualValue.Should().HaveFlag(expectedValue);
```

**Message**

```
`actualValue` does not have `expectedValue`.
```

**TestCode**

```csharp
var actualValue = SampleFlags.Read;

actualValue.Should().HaveFlag(SampleFlags.Write);
```

**Message**

```
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

```
`actualValue` has `expectedValue`.
```

**TestCode**

```csharp
var actualValue = SampleFlags.Read | SampleFlags.Write;

actualValue.Should().NotHaveFlag(SampleFlags.Write);
```

**Message**

```
`actualValue` has `SampleFlags.Write`.
```

