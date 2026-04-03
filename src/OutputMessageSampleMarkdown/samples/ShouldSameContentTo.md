# ShouldSameContentTo

## Should().SameContentTo() / File sizes do not match

**TestCode**

```csharp
var file1 = Path.Combine(tempDir, "file1.txt");
var file2 = Path.Combine(tempDir, "file2.txt");
File.WriteAllText(file1, "hello");
File.WriteAllText(file2, "hello world");

var actualValue = new FileInfo(file1);
var otherFile = new FileInfo(file2);

actualValue.Should().SameContentTo(otherFile);
```

**Message**

```text
File sizes do not match.

`actualValue`("C:\Users\SOME_USER\AppData\Local\Temp\c7397e81-e829-41c3-a755-ee5b39e0fce4\file1.txt"): 5
`otherFile`("C:\Users\SOME_USER\AppData\Local\Temp\c7397e81-e829-41c3-a755-ee5b39e0fce4\file2.txt"): 11
```

## Should().SameContentTo() / File contents do not match

**TestCode**

```csharp
var file1 = Path.Combine(tempDir, "file1.txt");
var file2 = Path.Combine(tempDir, "file2.txt");
File.WriteAllText(file1, "hello");
File.WriteAllText(file2, "world");

var actualValue = new FileInfo(file1);
var otherFile = new FileInfo(file2);

actualValue.Should().SameContentTo(otherFile);
```

**Message**

```text
File contents do not match.
```

## Should().NotSameContentTo()

**TestCode**

```csharp
var file1 = Path.Combine(tempDir, "file1.txt");
var file2 = Path.Combine(tempDir, "file2.txt");
File.WriteAllText(file1, "hello");
File.WriteAllText(file2, "hello");

var actualValue = new FileInfo(file1);
var otherFile = new FileInfo(file2);

actualValue.Should().NotSameContentTo(otherFile);
```

**Message**

```text
File contents matched, but were expected to differ.
```

