using Moq;
using ShouldMethodAssertion.ShouldExtensions;
using ShouldMethodAssertion.ShouldMethodDefinitions;

#pragma warning disable CA2263 // 型が既知の場合はジェネリック オーバーロードを優先する

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class InvokeAsyncShouldThrowTests
{
    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_NoThrow(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var actionMock = new Mock<Func<Task>>();
        actionMock.Setup(v => v.Invoke()).Returns(Task.CompletedTask);

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(actionMock.Object), "actual", default).ShouldThrowAsync<Exception>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        });

        actionMock.Verify(v => v.Invoke(), Times.Once());
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowUnexpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        });
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await new InvokeAsyncShouldThrow(InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))), "actual", default).ShouldThrowAsync<ArgumentException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<Task>(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowNestedAggregateUnexpectedTypeAndSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<Task>(() => throw new AggregateException("xxx", new AggregateException("yyy", new ArgumentException("zzz"), new FileNotFoundException("expected"))));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowNestedAggregateUnexpectedTypeAndMultipleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<Task>(() => throw new AggregateException("xxx",
            new AggregateException("yyy",
                new ArgumentException("zzz"),
                new FileNotFoundException("expected1")
                ),
            new EndOfStreamException("expected2")
            ));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            Assert.Equal("expected2", actualException.Message);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowAggregateSingleExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await new InvokeAsyncShouldThrow(InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))), "actual", default).ShouldThrowAsync<ArgumentException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowAggregateSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<Task>(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_NoThrow(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var actionMock = new Mock<Func<Task>>();

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(actionMock.Object), "actual", default).ShouldThrowAsync(typeof(Exception), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        });

        actionMock.Verify(v => v.Invoke(), Times.Once());
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowUnexpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        });
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await new InvokeAsyncShouldThrow(InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))), "actual", default).ShouldThrowAsync(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<Task>(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowNestedAggregateUnexpectedTypeAndSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<Task>(() => throw new AggregateException("xxx", new AggregateException("yyy", new ArgumentException("zzz"), new FileNotFoundException("expected"))));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowNestedAggregateUnexpectedTypeAndMultipleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<Task>(() => throw new AggregateException("xxx",
            new AggregateException("yyy",
                new ArgumentException("zzz"),
                new FileNotFoundException("expected1")
                ),
            new EndOfStreamException("expected2")
            ));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            Assert.Equal("expected2", actualException.Message);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowAggregateSingleExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await new InvokeAsyncShouldThrow(InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))), "actual", default).ShouldThrowAsync(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowAggregateSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<Task>(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await new InvokeAsyncShouldThrow(InvokeAsync.That(taskFunc), "actual", default).ShouldThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Fact]
    public async Task ShouldNotThrowAsync()
    {
        var exception = await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await new InvokeAsyncShouldThrow(InvokeAsync.That(async () => await Task.FromException(new Exception("xxx"))), "actual", default).ShouldNotThrowAsync().ConfigureAwait(false);
        });
        Assert.NotNull(exception.InnerException);
    }
}
