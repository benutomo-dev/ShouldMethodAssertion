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

### struct (System.Nullable&lt;T&gt;)

**TestCode**

```csharp
var actualValue = (int?)1;

actualValue.Should().BeNull();
```

**Message**

```
`actualValue` is not null.

[Actual]
`1`
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

### struct (System.Nullable&lt;T&gt;)

**TestCode**

```csharp
var actualValue = default(int?);

actualValue.Should().NotBeNull();
```

**Message**

```
`actualValue` is null.
```
