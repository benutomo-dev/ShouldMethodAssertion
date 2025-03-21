# ShouldThrowAsync

## Should().ThrowAsync()

### Method

**TestCode**

```csharp
await InvokeAsync.That(async () => await someObject.MethodAsync().ConfigureAwait(false)).Should().ThrowAsync<InvalidOperationException>().ConfigureAwait(false);
```

**Message**

```
No exception was thrown by `InvokeAsync.That(async () => await someObject.MethodAsync().ConfigureAwait(false))`.
```

### Func

**TestCode**

```csharp
Func<Task> actionAsync = () => throw new FileNotFoundException();

await InvokeAsync.That(actionAsync).Should().ThrowAsync<IOException>().ConfigureAwait(false); // When default, disallow derive exceptions.
```

**Message**

```
Wrong exception type thrown by `InvokeAsync.That(actionAsync)`.

Expected: `System.IO.IOException`
Actual: `System.IO.FileNotFoundException` with messge "Unable to find the specified file."
```

### Func with args

**TestCode**

```csharp
Func<int, Task> actionAsync = v => throw new FileNotFoundException();

await InvokeAsync.That(actionAsync, 0).Should().ThrowAsync<IOException>().ConfigureAwait(false); // When default, disallow derive exceptions.
```

**Message**

```
Wrong exception type thrown by `InvokeAsync.That(actionAsync, 0)`.

Expected: `System.IO.IOException`
Actual: `System.IO.FileNotFoundException` with messge "Unable to find the specified file."
```

### Include derived exception

**TestCode**

```csharp
Func<Task> actionAsync = () => throw new FileNotFoundException();

await InvokeAsync.That(actionAsync).Should().ThrowAsync<IOException>(includeDerivedType: true).ConfigureAwait(false); // NotFail
```

### With AggregateException Flatten

**TestCode**

```csharp
Func<Task> actionAsync = () => throw new AggregateException(new FileNotFoundException());

await InvokeAsync.That(actionAsync).Should().ThrowAsync<IOException>(
    includeDerivedType: true,
    aggregateExceptionHandling: ShouldMethodAssertion.AggregateExceptionHandling.AnyFlattened
    ).ConfigureAwait(false); // NotFail
```

## Should().NotThrow()

### Method

**TestCode**

```csharp
var returnValueOfMethod = await InvokeAsync.That(someObject.MethodAsync).Should().NotThrowAsync().ConfigureAwait(false);
```

**Message**

```
`InvokeAsync.That(someObject.MethodAsync)` threw an exception of type `System.InvalidOperationException`.

Message: "smple exception"
```

### Func

**TestCode**

```csharp
Func<Task<int>> funcAsync = () => someObject.MethodAsync();

await InvokeAsync.That(funcAsync).Should().NotThrowAsync().ConfigureAwait(false);
```

**Message**

```
`InvokeAsync.That(funcAsync)` threw an exception of type `System.InvalidOperationException`.

Message: "smple exception"
```

### Func with args

**TestCode**

```csharp
Func<int, Task<int>> funcAsync = v => someObject.MethodAsync();

var returnValueOfMethod = await InvokeAsync.That(funcAsync, 0).Should().NotThrowAsync().ConfigureAwait(false);
```

**Message**

```
`InvokeAsync.That(funcAsync, 0)` threw an exception of type `System.InvalidOperationException`.

Message: "smple exception"
```

