# ShouldContain

## Should().HaveAttribute()

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XAttribute("itemA", "value"),
]);

var attribute = rootElement.Should().HaveAttribute("itemB"); // attribute is XAttribute of first itemB=
```

**Message**

```
`rootElement` do not have attribute of itemB.
```

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XAttribute(XName.Get("itemA", "namespaceA"), "value"),
]);

var attribute = rootElement.Should().HaveAttribute("itemA"); // attribute is XAttribute of first itemA=
```

**Message**

```
`rootElement` do not have attribute of itemA.
```

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XAttribute(XName.Get("itemA", "namespaceA"), "value"),
]);

var attribute = rootElement.Should().HaveAttribute(XName.Get("itemA", "namespaceB")); // attribute is XAttribute of first namespaceB:itemA=
```

**Message**

```
`rootElement` do not have attribute of {namespaceB}itemA.
```

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XAttribute("itemA", "value"),
]);

var attributes = rootElement.Should().HaveAttribute("itemA", 2..3); // attributes is XAttribute[] of all itemA=
```

**Message**

```
`rootElement` contains 1 attribute of itemA, but the expected correct range of attributes is 2～3.
```

## Should().NotHaveAttribute()

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XAttribute("itemA", "value"),
]);

rootElement.Should().NotHaveAttribute("itemA");
```

**Message**

```
`rootElement` contains 1 attribute of itemA, but the expected correct number of attributes is 0.
```

