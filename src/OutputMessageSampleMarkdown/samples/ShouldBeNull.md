# ShouldBeNull

## Should().BeNull()

### class

**TestCode**

```csharp
var actualValue = "apple";

actualValue.Should().BeNull();
```

**Message**

```
`actualValue` is not null.

[Actual]
"apple"
```

## Should().NotBeNull()

### class

**TestCode**

```csharp
var actualValue = default(string);

actualValue.Should().NotBeNull();
```

**Message**

```
`actualValue` is null.
```

