# ShouldEqual

## Should().Equal()

### string[] / Not equal by value

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = (string[])["apple", "banana", "grape"];

actualValue.Should().Equal(expectedValue);
```

**Message**

```
`actualValue` must match `expectedValue`, but it did not match.

The content of `actualValue`[2] is different.

[Actual]
"orange"

[Expected]
"grape"
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().Equal(["apple", "banana", "grape"]);
```

**Message**

```
`actualValue` must match ["apple", "banana", "grape"], but it did not match.

The content of `actualValue`[2] is different.

[Actual]
"orange"

[Expected]
"grape"
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().Equal([
    "apple",
    "banana",
    "grape",
    ]);
```

**Message**

```
`actualValue` must match the following, but it did not match.

[
"apple",
"banana",
"grape",
]

The content of `actualValue`[2] is different.

[Actual]
"orange"

[Expected]
"grape"
```

### string[] / Not equal by count

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = (string[])["apple", "banana"];

actualValue.Should().Equal(expectedValue);
```

**Message**

```
`actualValue` must match `expectedValue`, including the order, but it did not match.

`actualValue` contains more elements than expected.
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().Equal(["apple", "banana"]);
```

**Message**

```
`actualValue` must match ["apple", "banana"], including the order, but it did not match.

`actualValue` contains more elements than expected.
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().Equal([
    "apple",
    "banana",
    ]);
```

**Message**

```
`actualValue` must match the following, including the order, but it did not match.

[
"apple",
"banana",
]

`actualValue` contains more elements than expected.
```

### string[] / Not equal without the order

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = (string[])["banana", "apple", "banana", "grape"];

actualValue.Should().Equal(expectedValue, ignoreOrder: true);
```

**Message**

```
`actualValue` must match the following when compared with `ignoreOrder`, but it did not match.

`expectedValue`

Below shows the differences between each collection in terms of the number of items for the same element.
Target item : {Number contained in actual collection, Number contained in expected collection}

banana : {ActualCount:1, ExpectedCount:2}
grape : {ActualCount:0, ExpectedCount:1}
orange : {ActualCount:1, ExpectedCount:0}

```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().Equal([
    "A",
    "C",
    "D",
    "E",
    "F",
    "G",
    "H",
    "I",
    "J",
    "K",
    "L",
    "M",
], ignoreOrder: true);
```

**Message**

```
`actualValue` must match the following when compared with `ignoreOrder`, but it did not match.

["A","C","D","E","F","G","H","I","J","K","L","M",]

Below shows the differences between each collection in terms of the number of items for the same element.
Target item : {Number contained in actual collection, Number contained in expected collection}

A : {ActualCount:0, ExpectedCount:1}
C : {ActualCount:0, ExpectedCount:1}
D : {ActualCount:0, ExpectedCount:1}
E : {ActualCount:0, ExpectedCount:1}
F : {ActualCount:0, ExpectedCount:1}
G : {ActualCount:0, ExpectedCount:1}
H : {ActualCount:0, ExpectedCount:1}
I : {ActualCount:0, ExpectedCount:1}
J : {ActualCount:0, ExpectedCount:1}
K : {ActualCount:0, ExpectedCount:1}

Additionally, there were differences in the count of 5 other elements.
```

### string[] / Not equal with comparer

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = (string[])["Apple", "Banana", "Grape"];

actualValue.Should().Equal(expectedValue, comparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` must match `expectedValue` when compared using `StringComparer.OrdinalIgnoreCase`, but it did not match.

The content of `actualValue`[2] is different.

[Actual]
"orange"

[Expected]
"Grape"
```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().Equal(["Apple", "Banana", "Grape"], comparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` must match ["Apple", "Banana", "Grape"] when compared using `StringComparer.OrdinalIgnoreCase`, but it did not match.

The content of `actualValue`[2] is different.

[Actual]
"orange"

[Expected]
"Grape"
```

### Dictionary<TKey, TValue> / Not equal by value

**TestCode**

```csharp
var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };
var expectedValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 2 } };

actualValue.Should().Equal(expectedValue);
```

**Message**

```
`actualValue` is not `expectedValue`.
```

**TestCode**

```csharp
var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };

actualValue.Should().Equal([("apple", 1), ("banana", 1), ("orange", 2)]);
```

**Message**

```
`actualValue` is not [("apple", 1), ("banana", 1), ("orange", 2)].
```

### Dictionary<TKey, TValue> / Not equal by key

**TestCode**

```csharp
var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };
var expectedValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "grape", 1 } };

actualValue.Should().Equal(expectedValue);
```

**Message**

```
`actualValue` is not `expectedValue`.
```

**TestCode**

```csharp
var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };

actualValue.Should().Equal([("apple", 1), ("banana", 1), ("grape", 1)]);
```

**Message**

```
`actualValue` is not [("apple", 1), ("banana", 1), ("grape", 1)].
```

### Dictionary<TKey, TValue> / Not equal by count

**TestCode**

```csharp
var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };
var expectedValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 } };

actualValue.Should().Equal(expectedValue);
```

**Message**

```
`actualValue` is not `expectedValue`.
```

**TestCode**

```csharp
var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 1 } };

actualValue.Should().Equal([("apple", 1), ("banana", 1)]);
```

**Message**

```
`actualValue` is not [("apple", 1), ("banana", 1)].
```

## Should().NotEqual()

### string[] / Equal by defualt

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotEqual(expectedValue);
```

**Message**

```
`actualValue` matches `expectedValue`, including the order,

```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotEqual(["apple", "banana", "orange"]);
```

**Message**

```
`actualValue` matches ["apple", "banana", "orange"], including the order,

```

### string[] / Equal ignore order

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotEqual(expectedValue, ignoreOrder: true);
```

**Message**

```
`actualValue` matches `expectedValue`, without the order,

```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotEqual(["apple", "banana", "orange"], ignoreOrder: true);
```

**Message**

```
`actualValue` matches ["apple", "banana", "orange"], without the order,

```

### string[] / Equal with comparer

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];
var expectedValue = (string[])["Apple", "Banana", "Orange"];

actualValue.Should().NotEqual(expectedValue, comparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` matches `expectedValue` when compared using `StringComparer.OrdinalIgnoreCase`, including the order,

```

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotEqual(["Apple", "Banana", "Orange"], comparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` matches ["Apple", "Banana", "Orange"] when compared using `StringComparer.OrdinalIgnoreCase`, including the order,

```

### Dictionary<TKey, TValue> / Equal by defualt

**TestCode**

```csharp
var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 2 } };
var expectedValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 2 } };

actualValue.Should().NotEqual(expectedValue);
```

**Message**

```
`actualValue` is `expectedValue`.
```

**TestCode**

```csharp
var actualValue = new Dictionary<string, int> { { "apple", 1 }, { "banana", 1 }, { "orange", 2 } };

actualValue.Should().NotEqual([("apple", 1), ("banana", 1), ("orange", 2)]);
```

**Message**

```
`actualValue` is [("apple", 1), ("banana", 1), ("orange", 2)].
```

### Dictionary<TKey, TValue> / Equal with value comparer

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };
var expectedValue = new Dictionary<int, string> { { 1, "Apple" }, { 2, "Banana" }, { 3, "Orange" } };

actualValue.Should().NotEqual(expectedValue, valueComparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` is `expectedValue`.
```

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().NotEqual([(1, "Apple"), (2, "Banana"), (3, "Orange")], valueComparer: StringComparer.OrdinalIgnoreCase);
```

**Message**

```
`actualValue` is [(1, "Apple"), (2, "Banana"), (3, "Orange")].
```

