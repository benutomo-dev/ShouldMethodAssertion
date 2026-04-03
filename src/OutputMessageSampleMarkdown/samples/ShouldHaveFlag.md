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

[Actual]
`Read`

[Expected Flag]
`Write`
```

**TestCode**

```csharp
var actualValue = SampleFlags.Read;

actualValue.Should().HaveFlag(SampleFlags.Write);
```

**Message**

```text
`actualValue` does not have `SampleFlags.Write`.

[Actual]
`Read`

[Expected Flag]
`Write`
```

**TestCode**

```csharp
var actualValue = SampleFlags.Read | SampleFlags.Write;
var expectedValue = SampleFlags.Read | SampleFlags.Execute;

actualValue.Should().HaveFlag(expectedValue);
```

**Message**

```text
`actualValue` does not have `expectedValue`.

[Actual]
`Read, Write`

[Expected Flag]
`Read, Execute`
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

[Actual]
`Read, Write`

[Unexpected Flag]
`Write`
```

**TestCode**

```csharp
var actualValue = SampleFlags.Read | SampleFlags.Write;

actualValue.Should().NotHaveFlag(SampleFlags.Write);
```

**Message**

```text
`actualValue` has `SampleFlags.Write`.

[Actual]
`Read, Write`

[Unexpected Flag]
`Write`
```

