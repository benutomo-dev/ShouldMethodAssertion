# ShouldHaveCount

## Should().HaveCount()

### string[]

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().HaveCount(1);
```

**Message**

```
Count of `actualValue` is 3. But it expected to be 1.
```

### Dictionary<TKey, TValue> / Small number of elements

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().HaveCount(1);
```

**Message**

```
Count of `actualValue` is 3. But it expected to be 1.
```

## Should().NotHaveCount()

### string[]

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotHaveCount(3);
```

**Message**

```
Count of `actualValue` is 3. But did not expect it to be.
```

### Dictionary<TKey, TValue>

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().NotHaveCount(3);
```

**Message**

```
Count of `actualValue` is 3. But did not expect it to be.
```

