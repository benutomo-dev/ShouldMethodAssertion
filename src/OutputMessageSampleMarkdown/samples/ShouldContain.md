# ShouldContain

## Should().Contain()

### string

**TestCode**

```csharp
var actualValue = "apple banana orange";
var expectedValue = "Apple";

actualValue.Should().Contain(expectedValue);
```

**Message**

```
`actualValue` is not contain "Apple".

[Actual]
"apple banana orange"
```

**TestCode**

```csharp
var actualValue = "apple banana orange";

actualValue.Should().Contain("Apple", /*containedCounts*/ 2, ignoreCase: true);
```

**Message**

```
`actualValue` is contain 1 "Apple", with case ignored. But expected count is 2.

[Actual]
"apple banana orange"
```

**TestCode**

```csharp
var actualValue = "apple banana orange";

actualValue.Should().Contain("banana", /*containedCountsRange*/ 2..3, ignoreCase: true);
```

**Message**

```
`actualValue` is contain 1 "banana", with case ignored. But expected count is in range of 2～3.

[Actual]
"apple banana orange"
```

### Array / Default

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = "Apple";

actualValue.Should().Contain(expectedValue);
```

**Message**

```
`actualValue` is NOT contain `expectedValue`.
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().Contain("Apple");
```

**Message**

```
`actualValue` is NOT contain "Apple".
```

### Array / Not contain with comparer

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = "Grape";

actualValue.Should().Contain(expectedValue, StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` is NOT contain `expectedValue`.
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().Contain("Grape", StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` is NOT contain "Grape".
```

### Dictionary<TKey, TValue> / Not contain by KeyValuePair<,>

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
var expectedValue = new KeyValuePair<int, string>(1, "Apple");

actualValue.Should().Contain(expectedValue);
```

**Message**

```
`actualValue` contain expected key. But value is NOT expected.

[ActualValue]
apple

[ExpectedValue]
Apple
```

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().Contain(new KeyValuePair<int, string>(1, "Apple"));
```

**Message**

```
`actualValue` contain expected key. But value is NOT expected.

[ActualValue]
apple

[ExpectedValue]
Apple
```

### Dictionary<TKey, TValue> / Not contain by ValueTuple

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
var expectedValue = (1, "Apple");

actualValue.Should().Contain(expectedValue);
```

**Message**

```
`actualValue` contain expected key. But value is NOT expected.

[ActualValue]
apple

[ExpectedValue]
Apple
```

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().Contain((1, "Apple"));
```

**Message**

```
`actualValue` contain expected key. But value is NOT expected.

[ActualValue]
apple

[ExpectedValue]
Apple
```

### Dictionary<TKey, TValue> / Not contain with value comparer

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple"}, { 2, "banana" }, { 3, "orange" } };
var expectedValue = (3, "grape");

actualValue.Should().Contain(expectedValue, valueComparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` contain expected key. But value is NOT expected.

[ActualValue]
orange

[ExpectedValue]
grape
```

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().Contain((3, "grape"), valueComparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` contain expected key. But value is NOT expected.

[ActualValue]
orange

[ExpectedValue]
grape
```

## Should().NotContain()

### string

**TestCode**

```csharp
var actualValue = "apple banana orange";
var expectedValue = "apple";

actualValue.Should().NotContain(expectedValue);
```

**Message**

```
`actualValue` is contain 1 "apple". But expected count is 0.

[Actual]
"apple banana orange"
```

**TestCode**

```csharp
var actualValue = "apple banana orange";

actualValue.Should().NotContain("Apple", ignoreCase: true);
```

**Message**

```
`actualValue` is contain 1 "Apple", with case ignored. But expected count is 0.

[Actual]
"apple banana orange"
```

### Array / Defualt

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = "banana";

actualValue.Should().NotContain(expectedValue);
```

**Message**

```
`actualValue` contain `expectedValue`.
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotContain("banana");
```

**Message**

```
`actualValue` contain "banana".
```

### Array / Contain with comparer

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = "Banana";

actualValue.Should().NotContain(expectedValue, comparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` contain `expectedValue`.
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotContain("Banana", comparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` contain "Banana".
```

### Dictionary<TKey, TValue> / Contain by KeyValuePair<,>

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
var expectedValue = new KeyValuePair<int, string>(1, "apple");

actualValue.Should().NotContain(expectedValue);
```

**Message**

```
`actualValue` contain `expectedValue`.
```

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().NotContain(new KeyValuePair<int, string>(1, "apple"));
```

**Message**

```
`actualValue` contain `new KeyValuePair<int, string>(1, "apple")`.
```

### Dictionary<TKey, TValue> / Contain by ValueTuple

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
var expectedValue = (1, "apple");

actualValue.Should().NotContain(expectedValue);
```

**Message**

```
`actualValue` contain `expectedValue`.
```

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().NotContain((1, "apple"));
```

**Message**

```
`actualValue` contain (1, "apple").
```

### Dictionary<TKey, TValue> / Equal with value comparer

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
var expectedValue = (1, "Apple");

actualValue.Should().NotContain(expectedValue, valueComparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` contain `expectedValue`.
```

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().NotContain((1, "Apple"), valueComparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` contain (1, "Apple").
```

