# ShouldMatch

## Should().Match()

**TestCode**

```csharp
var actualValue = "hello world";
var expectedPattern = "hello*earth";

actualValue.Should().Match(expectedPattern);
```

**Message**

```
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

```
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

```
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

```
`actualValue` matches expected.

[Actual]
hello world

[Expected]
hello*
```

