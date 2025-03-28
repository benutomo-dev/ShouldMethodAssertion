# ShouldContain

## Should().HaveElement()

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XElement("itemA"),
]);

var element = rootElement.Should().HaveElement("itemB"); // element is XElement of first <itemB>
```

**Message**

```
`rootElement` do not have itemB.
```

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XElement(XName.Get("itemA", "namespaceA")),
]);

var element = rootElement.Should().HaveElement("itemA"); // element is XElement of first <itemA>
```

**Message**

```
`rootElement` do not have itemA.
```

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XElement(XName.Get("itemA", "namespaceA")),
]);

var element = rootElement.Should().HaveElement(XName.Get("itemA", "namespaceB")); // element is XElement of first <namespaceB:itemA>
```

**Message**

```
`rootElement` do not have {namespaceB}itemA.
```

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XElement("itemA"),
]);

var elements = rootElement.Should().HaveElement("itemA", 2..3); // elements is XElement[] of all <itemA>
```

**Message**

```
`rootElement` contains 1 itemA, but the expected correct range of elements is 2～3.
```

## Should().NotHaveElement()

**TestCode**

```csharp
var rootElement = new XElement("root", [
    new XElement("itemA"),
]);

rootElement.Should().NotHaveElement("itemA");
```

**Message**

```
`rootElement` contains 1 itemA, but the expected correct number of elements is 0.
```

