# ShouldBeAssignableTo

## Should().BeAssignableTo<T>()

**TestCode**

```csharp
object actualValue = "hello";

actualValue.Should().BeAssignableTo<int>();
```

**Message**

```text
`actualValue` is not assignable to System.Int32.
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
`actualValue` is not assignable to `type`.
```

**TestCode**

```csharp
object actualValue = "hello";

actualValue.Should().BeAssignableTo(typeof(int));
```

**Message**

```text
`actualValue` is not assignable to `typeof(int)`.
```

## Should().NotBeAssignableTo<T>()

**TestCode**

```csharp
object actualValue = "hello";

actualValue.Should().NotBeAssignableTo<string>();
```

**Message**

```text
`actualValue` is assignable to System.String.
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
`actualValue` is assignable to `type`.
```

**TestCode**

```csharp
object actualValue = "hello";

actualValue.Should().NotBeAssignableTo(typeof(string));
```

**Message**

```text
`actualValue` is assignable to `typeof(string)`.
```

