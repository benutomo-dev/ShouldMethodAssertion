# ShouldBeSingle

## Should().BeSingle()

### string[] / Empty

**TestCode**

```csharp
var actualValues = (string[])[];

actualValues.Should().BeSingle();
```

**Message**

```
`actualValues` is empty.
```

### string[] / Two or more elements

**TestCode**

```csharp
var actualValues = (string[])["apple", "banana" ];

actualValues.Should().BeSingle();
```

**Message**

```
`actualValues` has two or more elements.
```

### Dictionary<TKey, TValue> / Empty

**TestCode**

```csharp
var actualValues = new Dictionary<int, string>();

actualValues.Should().BeSingle();
```

**Message**

```
`actualValues` is empty.
```

### Dictionary<TKey, TValue> / Two or more elements

**TestCode**

```csharp
var actualValues = new Dictionary<string, string> { { "apple", "APPLE" }, { "banana", "BANANA" } };

actualValues.Should().BeSingle();
```

**Message**

```
`actualValues` has two or more elements.
```

### string[] / Use result

**TestCode**

```csharp
var actualValues = (string[])["apple"];

var singleValue = actualValues.Should().BeSingle().Result;

singleValue.Should().Be("apple");
```

## Should().NotBeSingle()

### string[]

**TestCode**

```csharp
var actualValues = (string[])["apple"];

actualValues.Should().NotBeSingle();
```

**Message**

```
`actualValues` is single.
```

### Dictionary<TKey, TValue>

**TestCode**

```csharp
var actualValues = new Dictionary<int, string> { {1, "apple" } };

actualValues.Should().NotBeSingle();
```

**Message**

```
`actualValues` is single.
```

