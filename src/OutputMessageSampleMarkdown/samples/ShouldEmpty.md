# ShouldEmpty

## Should().BeEmpty()

### string[] / Small number of elements

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().BeEmpty();
```

**Message**

```
`actualValue` have 3 elements.

List of all elements:
[0] "apple"
[1] "banana"
[2] "orange"
```

### string[] / Large number of elements

**TestCode**

```csharp
var actualValue = (string[])["apple", "banana", "orange", "A", "B", "C", "D", "E", "F", "G", "H", "I"];

actualValue.Should().BeEmpty();
```

**Message**

```
`actualValue` have 12 elements.

List of first 10 elements:
[0] "apple"
[1] "banana"
[2] "orange"
[3] "A"
[4] "B"
[5] "C"
[6] "D"
[7] "E"
[8] "F"
[9] "G"
```

### Dictionary<TKey, TValue> / Small number of elements

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValue.Should().BeEmpty();
```

**Message**

```
`actualValue` have 3 elements.

List of all elements:
[0] [1, apple]
[1] [2, banana]
[2] [3, orange]
```

### Dictionary<TKey, TValue> / Large number of elements

**TestCode**

```csharp
var actualValue = new Dictionary<string, string> { { "apple", "APPLE" }, { "banana", "BANANA" }, { "orange", "ORANGE" }, { "a", "A" }, { "b", "B" }, { "c", "C" }, { "d", "D" }, { "e", "E" }, { "f", "F" }, { "g", "G" }, { "h", "H" }, { "i", "I" } };

actualValue.Should().BeEmpty();
```

**Message**

```
`actualValue` have 12 elements.

List of first 10 elements:
[0] [apple, APPLE]
[1] [banana, BANANA]
[2] [orange, ORANGE]
[3] [a, A]
[4] [b, B]
[5] [c, C]
[6] [d, D]
[7] [e, E]
[8] [f, F]
[9] [g, G]
```

## Should().NotBeEmpty()

### string[]

**TestCode**

```csharp
var actualValue = (string[])[];

actualValue.Should().NotBeEmpty();
```

**Message**

```
`actualValue` is empty.
```

### Dictionary<TKey, TValue>

**TestCode**

```csharp
var actualValue = new Dictionary<int, string> { };

actualValue.Should().NotBeEmpty();
```

**Message**

```
`actualValue` is empty.
```

