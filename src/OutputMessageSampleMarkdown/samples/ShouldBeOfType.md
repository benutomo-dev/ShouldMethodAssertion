# ShouldBeOfType

## Should().BeOfType()

**TestCode**

```csharp
IEnumerable<string> actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().BeOfType<IEnumerable<string>>();
```

**Message**

```
Type of `actualValue` is string[]. But did not expect it to be.

[Actual]
string[]

[Expected]
System.Collections.Generic.IEnumerable<string>
```

**TestCode**

```csharp
IEnumerable<string> actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().BeOfType(typeof(IEnumerable<string>));
```

**Message**

```
Type of `actualValue` is string[]. But did not expect it to be.

[Actual]
string[]

[Expected]
System.Collections.Generic.IEnumerable<string>
```

## Should().NotBeOfType()

**TestCode**

```csharp
IEnumerable<string> actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotBeOfType<string[]>();
```

**Message**

```
Type of `actualValue` is string[].
```

**TestCode**

```csharp
IEnumerable<string> actualValue = (string[])["apple", "banana", "orange"];

actualValue.Should().NotBeOfType(typeof(string[]));
```

**Message**

```
Type of `actualValue` is string[].
```

