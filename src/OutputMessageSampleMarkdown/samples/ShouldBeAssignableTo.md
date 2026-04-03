# ShouldBeAssignableTo

## Should().BeAssignableTo<T>()

**TestCode**

```csharp
object actualValue = "hello";

actualValue.Should().BeAssignableTo<int>();
```

**Message**

```text
`actualValue` is not assignable to int.
`actualValue` is string.
```

## Should().BeAssignableTo(Type)

**TestCode**

```csharp
object actualValue = "hello";
var type = typeof(int);

actualValue.Should().BeAssignableTo(type);
```

**Message**

```text
`actualValue` is not assignable to `type`(int).
`actualValue` is string.
```

**TestCode**

```csharp
object actualValue = "hello";

actualValue.Should().BeAssignableTo(typeof(int));
```

**Message**

```text
`actualValue` is not assignable to `typeof(int)`.
`actualValue` is string.
```

## Should().NotBeAssignableTo<T>()

**TestCode**

```csharp
object actualValue = "hello";

actualValue.Should().NotBeAssignableTo<string>();
```

**Message**

```text
`actualValue` is assignable to string.
`actualValue` is string.
```

## Should().NotBeAssignableTo(Type)

**TestCode**

```csharp
object actualValue = "hello";
var type = typeof(string);

actualValue.Should().NotBeAssignableTo(type);
```

**Message**

```text
`actualValue` is assignable to `type`(string).
`actualValue` is string.
```

**TestCode**

```csharp
object actualValue = "hello";

actualValue.Should().NotBeAssignableTo(typeof(IEnumerable<char>));
```

**Message**

```text
`actualValue` is assignable to `typeof(IEnumerable<char>)`.
`actualValue` is string.
```

