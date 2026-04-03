# ShouldSatisfy

## ShouldSatisfy() / Fail by assertion

**TestCode**

```csharp
var actual = "hello";

actual.ShouldSatisfy(v =>
{
    v.Should().Be("world");
});
```

**Message**

```text
`actual`(⇒ `v`) did not satisfy the specified condition.
[Actual Value] "hello"

  `v` is "world". But did not expect it to be.
  
  [Actual]
  "hello"
  
  [Expected]
  "world"
  
```

## ShouldSatisfy() / Fail by exception

**TestCode**

```csharp
var actual = "not a number";

actual.ShouldSatisfy(v =>
{
    int.Parse(v, CultureInfo.InvariantCulture);
});
```

**Message**

```text
An exception occurred while verifying `actual`(⇒ `v`).
[Actual Value] "not a number"

  System.FormatException: The input string 'not a number' was not in a correct format.
```

## ShouldSatisfy() / Nested ShouldSatisfy

**TestCode**

```csharp
var actual = (Name: "Alice", Age: 15);

actual.ShouldSatisfy(v =>
{
    v.Name.ShouldSatisfy(name =>
    {
        name.Should().Be("Bob");
    });
});
```

**Message**

```text
`actual`(⇒ `v`) did not satisfy the specified condition.
[Actual Value] (Alice, 15)

  `v.Name`(⇒ `name`) did not satisfy the specified condition.
  [Actual Value] "Alice"
  
    `name` is "Bob". But did not expect it to be.
    
    [Actual]
    "Alice"
    
    [Expected]
    "Bob"
    
  
```

