# ShouldMethodAssertion

[Nuget](https://www.nuget.org/packages/ShouldMethodAssertion/)


ShouldMethodAssertion is a library that provides simple assertion functionality using fluent-style Should method.

```csharp
using ShouldMethodAssertion.ShouldExtensions;

actualValue.Should().Be(expectedValue);

actualValue.Should().BeOneOf(["apple", "banana", "orange"]);

enumerableOrSpan.Should().Equal(["value1", "value2", "value3"]);

actualSomeEnum.Should().HaveFlag(SomeEnumFlags.Flag1);

actualSomeDictionary.Should().Equal([("key1", 123), ("key2", 0)]);

// void NoArgsMethod()
await Invoke.That(someObject.NoArgsMethod).Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);

// void TwoArgsMethod(string arg1, string arg2)
await Invoke.That(someObject.TwoArgsMethod, "firstArg", "secondArg").Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);
await Invoke.That(() => someObject.TwoArgsMethod("firstArg", "secondArg")).Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);

// async Task NoArgsMethodAsync()
await InvokeAsync.That(someObject.NoArgsMethodAsync).Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);

// async Task TwoArgsMethodAsync(string arg1, string arg2)
await InvokeAsync.That(someObject.TwoArgsMethodAsync, "firstArg", "secondArg").Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);
await InvokeAsync.That(async () => await someObject.TwoArgsMethodAsync("firstArg", "secondArg").ConfigureAwait(false)).Should().ThrowAsync<ArgumentException>().ConfigureAwait(false);

// etc...
```