# ShouldThrow

## Should().Throw()

### Method

**TestCode**

```csharp
Invoke.That(() => someObject.Method()).Should().Throw<InvalidOperationException>();
```

**Message**

```
No exception was thrown by `Invoke.That(() => someObject.Method())`.
```

### Action

**TestCode**

```csharp
Action action = () => throw new FileNotFoundException();

action.Should().Throw<IOException>(); // When default, disallow derive exceptions.
```

**Message**

```
Wrong exception type thrown by `action`.

Expected: `System.IO.IOException`
Actual: `System.IO.FileNotFoundException` with messge "Unable to find the specified file."
```

### Action with args

**TestCode**

```csharp
Action<int> action = v => throw new FileNotFoundException();

Invoke.That(() => action(0)).Should().Throw<IOException>(); // When default, disallow derive exceptions.
```

**Message**

```
Wrong exception type thrown by `Invoke.That(() => action(0))`.

Expected: `System.IO.IOException`
Actual: `System.IO.FileNotFoundException` with messge "Unable to find the specified file."
```

### Include derived exception

**TestCode**

```csharp
var action = new Action(() => throw new FileNotFoundException());

action.Should().Throw<IOException>(includeDerivedType: true); // NotFail
```

### With AggregateException Flatten

**TestCode**

```csharp
var action = new Action(() => throw new AggregateException(new FileNotFoundException()));

action.Should().Throw<IOException>(includeDerivedType: true, aggregateExceptionHandling: ShouldMethodAssertion.AggregateExceptionHandling.AnyFlattened); // NotFail
```

## Should().NotThrow()

### Method

**TestCode**

```csharp
var returnValueOfMethod = Invoke.That(() => someObject.Method()).Should().NotThrow();
```

**Message**

```
`Invoke.That(() => someObject.Method())` threw an exception of type `System.InvalidOperationException`.

Message: "smple exception"
```

### Func

**TestCode**

```csharp
Action action = () => someObject.Method();

action.Should().NotThrow();
```

**Message**

```
`action` threw an exception of type `System.InvalidOperationException`.

Message: "smple exception"
```

### Func with args

**TestCode**

```csharp
Func<int, int> action = v => someObject.Method();

var returnValueOfMethod = Invoke.That(() => action(0)).Should().NotThrow();
```

**Message**

```
`Invoke.That(() => action(0))` threw an exception of type `System.InvalidOperationException`.

Message: "smple exception"
```

