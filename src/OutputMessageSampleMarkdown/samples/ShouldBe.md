# ShouldBe

## Should().Be()

### int

**TestCode**

```csharp
var actualValue = 1;
var expectedValue = 2;

actualValue.Should().Be(expectedValue);
```

**Message**

```
`actualValue` is `2`. But did not expect it to be.

[Actual]
`1`

[Expected]
`2`
```

**TestCode**

```csharp
var actualValue = 1;

actualValue.Should().Be(2);
```

**Message**

```
`actualValue` is `2`. But did not expect it to be.

[Actual]
`1`

[Expected]
`2`
```

### string

**TestCode**

```csharp
var actualValue = "apple";
var expectedValue = "banana";

actualValue.Should().Be(expectedValue);
```

**Message**

```
`actualValue` is "banana". But did not expect it to be.

[Actual]
"apple"

[Expected]
"banana"
```

**TestCode**

```csharp
var actualValue = "apple";

actualValue.Should().Be("banana");
```

**Message**

```
`actualValue` is "banana". But did not expect it to be.

[Actual]
"apple"

[Expected]
"banana"
```

### Guid

**TestCode**

```csharp
var actualValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");
var expectedValue = Guid.Parse("2968a94a-febf-4939-b90e-c763a9fefba4");

actualValue.Should().Be(expectedValue);
```

**Message**

```
`actualValue` is not expected.

[Actual]
`73b75ba1-8a48-4cb9-9eb3-c138743e9b0b`

[Expected]
`2968a94a-febf-4939-b90e-c763a9fefba4`
```

**TestCode**

```csharp
var actualValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");

actualValue.Should().Be(Guid.Parse("2968a94a-febf-4939-b90e-c763a9fefba4"));
```

**Message**

```
`actualValue` is not expected.

[Actual]
`73b75ba1-8a48-4cb9-9eb3-c138743e9b0b`

[Expected]
`2968a94a-febf-4939-b90e-c763a9fefba4`
```

## Should().NotBe()

### int

**TestCode**

```csharp
var actualValue = 1;
var expectedValue = 1;

actualValue.Should().NotBe(expectedValue);
```

**Message**

```
`actualValue` is `1`. But did not expect it to be.

```

**TestCode**

```csharp
var actualValue = 1;

actualValue.Should().NotBe(1);
```

**Message**

```
`actualValue` is `1`. But did not expect it to be.

```

### string

**TestCode**

```csharp
var actualValue = "apple";
var expectedValue = "apple";

actualValue.Should().NotBe(expectedValue);
```

**Message**

```
`actualValue` is "apple". But did not expect it to be.

```

**TestCode**

```csharp
var actualValue = "apple";

actualValue.Should().NotBe("apple");
```

**Message**

```
`actualValue` is "apple". But did not expect it to be.

```

### Guid

**TestCode**

```csharp
var actualValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");
var expectedValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");

actualValue.Should().NotBe(expectedValue);
```

**Message**

```
The value of `actualValue` is not expected.

[`actualValue`]
`73b75ba1-8a48-4cb9-9eb3-c138743e9b0b`
```

**TestCode**

```csharp
var actualValue = Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b");

actualValue.Should().NotBe(Guid.Parse("73b75ba1-8a48-4cb9-9eb3-c138743e9b0b"));
```

**Message**

```
The value of `actualValue` is not expected.

[`actualValue`]
`73b75ba1-8a48-4cb9-9eb3-c138743e9b0b`
```

