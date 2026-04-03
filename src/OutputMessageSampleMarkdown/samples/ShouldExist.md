# ShouldExist

## Should().Exist()

**TestCode**

```csharp
var actualValue = new FileInfo("path/to/nonexistent-file.txt");

actualValue.Should().Exist();
```

**Message**

```
`actualValue`("D:\source\ShouldMethodAssertion\bin\src\OutputMessageSampleMarkdown\Debug\net10.0\path\to\nonexistent-file.txt") does not exist.
```

## Should().NotExist()

**TestCode**

```csharp
var tempFile = Path.Combine(tempDir, "testfile.bin");
try
{
    var actualValue = new FileInfo(tempFile);

    actualValue.Should().NotExist();
}
finally
{
    File.Delete(tempFile);
}
```

**Message**

<span style="color:red; font-weight: bold;">No Messsage.</span>

