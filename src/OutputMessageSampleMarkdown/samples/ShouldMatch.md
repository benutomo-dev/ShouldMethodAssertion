# ShouldMatch

## Should().Match()

**TestCode**

```csharp
var actualValue = "hello world";
var expectedPattern = "hello*earth";

actualValue.Should().Match(expectedPattern);
```

**Message**

```text
`actualValue` does not match expected.

[Actual]
hello world

[Expected]
hello*earth
```

**TestCode**

```csharp
var actualValue = "hello world";

actualValue.Should().Match("hello*earth");
```

**Message**

```text
`actualValue` does not match expected.

[Actual]
hello world

[Expected]
hello*earth
```

## Should().NotMatch()

**TestCode**

```csharp
var actualValue = "hello world";
var expectedPattern = "hello*";

actualValue.Should().NotMatch(expectedPattern);
```

**Message**

```text
`actualValue` matches expected.

[Actual]
hello world

[Expected]
hello*
```

**TestCode**

```csharp
var actualValue = "hello world";

actualValue.Should().NotMatch("hello*");
```

**Message**

```text
`actualValue` matches expected.

[Actual]
hello world

[Expected]
hello*
```

