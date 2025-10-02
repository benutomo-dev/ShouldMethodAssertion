# ShouldAllSatisfy

## Should().AllSatisfy()

### string[] / Fail by assertion

**TestCode**

```csharp
var actualValues = (string[])["apple", "banana", "orange", "grape"];

actualValues.Should().AllSatisfy(v =>
{
    v.Should().BeOneOf(["apple", "banana"]);
});
```

**Message**

```
`actualValues` has not satisfied element.

--- [2]: "orange" ---
  `v` is not one of ["apple", "banana"].

  at ShouldMethodAssertion.ShouldMethodDefinitions.ObjectShouldBeOneOf.ShouldBeOneOf[T](ReadOnlySpan`1 expectedList, IEqualityComparer`1 comparer) in K:\Mine\repos\ShouldMethodAssertion\src\ShouldMethodAssertion\ShouldMethodDefinitions\ObjectShouldBeOneOf.cs:line 19
  at ShouldMethodAssertion.ShouldObjects.ShouldString.BeOneOf[T](ReadOnlySpan`1 expectedList, IEqualityComparer`1 comparer, String expectedListCallerArgumentExpression, String comparerCallerArgumentExpression) in K:\Mine\repos\ShouldMethodAssertion\obj\src\ShouldMethodAssertion\Debug\net10.0\ShouldMethodAssertion.Generator\ShouldMethodAssertion.Generator.IncrementalGenerator\ShouldObjects\ShouldString\ObjectShouldBeOneOf\string.cs:line 24
--- [3]: "grape" ---
  `v` is not one of ["apple", "banana"].

  at ShouldMethodAssertion.ShouldMethodDefinitions.ObjectShouldBeOneOf.ShouldBeOneOf[T](ReadOnlySpan`1 expectedList, IEqualityComparer`1 comparer) in K:\Mine\repos\ShouldMethodAssertion\src\ShouldMethodAssertion\ShouldMethodDefinitions\ObjectShouldBeOneOf.cs:line 19
  at ShouldMethodAssertion.ShouldObjects.ShouldString.BeOneOf[T](ReadOnlySpan`1 expectedList, IEqualityComparer`1 comparer, String expectedListCallerArgumentExpression, String comparerCallerArgumentExpression) in K:\Mine\repos\ShouldMethodAssertion\obj\src\ShouldMethodAssertion\Debug\net10.0\ShouldMethodAssertion.Generator\ShouldMethodAssertion.Generator.IncrementalGenerator\ShouldObjects\ShouldString\ObjectShouldBeOneOf\string.cs:line 24
```

### string[] / Fail by exception

**TestCode**

```csharp
var actualValues = (string[])["apple", "1", "banana", "2"];

actualValues.Should().AllSatisfy(v =>
{
    int.Parse(v, CultureInfo.InvariantCulture);
});
```

**Message**

```
`actualValues` has not satisfied element.

--- [0]: "apple" ---
  System.FormatException: The input string 'apple' was not in a correct format.

  at System.Number.ThrowFormatException[TChar](ReadOnlySpan`1 value)
  at System.Int32.Parse(String s, IFormatProvider provider)
--- [2]: "banana" ---
  System.FormatException: The input string 'banana' was not in a correct format.

  at System.Number.ThrowFormatException[TChar](ReadOnlySpan`1 value)
  at System.Int32.Parse(String s, IFormatProvider provider)
```

### Dictionary<TKey, TValue>

**TestCode**

```csharp
var actualValues = new Dictionary<int, string> { { 1, "apple" }, { 2, "banana" }, { 3, "orange" } };

actualValues.Should().AllSatisfy((key, value) =>
{
    value.Should().BeOneOf(["apple", "banana"]);
});
```

**Message**

```
`actualValues` has not satisfied element.

--- [3]: "orange" ---
  `value` is not one of ["apple", "banana"].

  at ShouldMethodAssertion.ShouldMethodDefinitions.ObjectShouldBeOneOf.ShouldBeOneOf[T](ReadOnlySpan`1 expectedList, IEqualityComparer`1 comparer) in K:\Mine\repos\ShouldMethodAssertion\src\ShouldMethodAssertion\ShouldMethodDefinitions\ObjectShouldBeOneOf.cs:line 19
  at ShouldMethodAssertion.ShouldObjects.ShouldString.BeOneOf[T](ReadOnlySpan`1 expectedList, IEqualityComparer`1 comparer, String expectedListCallerArgumentExpression, String comparerCallerArgumentExpression) in K:\Mine\repos\ShouldMethodAssertion\obj\src\ShouldMethodAssertion\Debug\net10.0\ShouldMethodAssertion.Generator\ShouldMethodAssertion.Generator.IncrementalGenerator\ShouldObjects\ShouldString\ObjectShouldBeOneOf\string.cs:line 24
```

