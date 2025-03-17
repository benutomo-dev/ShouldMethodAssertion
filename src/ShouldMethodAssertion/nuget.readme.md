# ShouldMethodAssertion

ShouldMethodAssertion is a library that provides simple assertion functionality using fluent-style Should method.

```csharp
// using ShouldMethodAssertion.ShouldExtensions;

actualValue.Should().Be(expectedValue);

actualValue.Should().BeOneOf(["apple", "banana", "orange"]);

enumerableOrSpan.Should().Equal(["value1", "value2", "value3"]);

actualSomeEnum.Should().HaveFlag(SomeEnumFlags.Flag1);

actualSomeDictionary.Should().Equal([("key1", 123), ("key2", 0)]);

await taskFunc.Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);

// etc...
```

## Repository

[github](https://github.com/benutomo-dev/ShouldMethodAssertion)
