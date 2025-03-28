﻿using Moq;
using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.InvokeAsyncShouldThrow)])]
public class ValueTaskFuncShouldThrowTests
{
    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_NoThrow(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var actionMock = new Mock<Func<ValueTask>>();
        actionMock.Setup(v => v.Invoke()).Returns(default(ValueTask));

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await InvokeAsync.That(actionMock.Object).Should().ThrowAsync<Exception>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        });

        actionMock.Verify(v => v.Invoke(), Times.Once());
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowUnexpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await InvokeAsync.That(async v => await Task.FromException(new ArgumentException("xxx")), 0).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        });
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))).Should().ThrowAsync<ArgumentException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<ValueTask>(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            await InvokeAsync.That(taskFunc).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await InvokeAsync.That(taskFunc).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowNestedAggregateUnexpectedTypeAndSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<ValueTask>(() => throw new AggregateException("xxx", new AggregateException("yyy", new ArgumentException("zzz"), new FileNotFoundException("expected"))));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = await InvokeAsync.That(taskFunc).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await InvokeAsync.That(taskFunc).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowNestedAggregateUnexpectedTypeAndMultipleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<ValueTask>(() => throw new AggregateException("xxx",
            new AggregateException("yyy",
                new ArgumentException("zzz"),
                new FileNotFoundException("expected1")
                ),
            new EndOfStreamException("expected2")
            ));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = await InvokeAsync.That(taskFunc).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
            Assert.Equal("expected2", actualException.Message);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await InvokeAsync.That(taskFunc).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowAggregateSingleExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))).Should().ThrowAsync<ArgumentException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsyncT_ThrowAggregateSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<ValueTask>(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            await InvokeAsync.That(taskFunc).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await InvokeAsync.That(taskFunc).Should().ThrowAsync<IOException>(includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_NoThrow(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var actionMock = new Mock<Func<ValueTask>>();

        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await InvokeAsync.That(actionMock.Object).Should().ThrowAsync(typeof(Exception), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        });

        actionMock.Verify(v => v.Invoke(), Times.Once());
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowUnexpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
        });
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))).Should().ThrowAsync(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<ValueTask>(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            await InvokeAsync.That(taskFunc).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await InvokeAsync.That(taskFunc).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowNestedAggregateUnexpectedTypeAndSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<ValueTask>(() => throw new AggregateException("xxx", new AggregateException("yyy", new ArgumentException("zzz"), new FileNotFoundException("expected"))));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = await InvokeAsync.That(taskFunc).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await InvokeAsync.That(taskFunc).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowNestedAggregateUnexpectedTypeAndMultipleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<ValueTask>(() => throw new AggregateException("xxx",
            new AggregateException("yyy",
                new ArgumentException("zzz"),
                new FileNotFoundException("expected1")
                ),
            new EndOfStreamException("expected2")
            ));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = await InvokeAsync.That(taskFunc).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
            Assert.Equal("expected2", actualException.Message);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await InvokeAsync.That(taskFunc).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowAggregateSingleExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        await InvokeAsync.That(async () => await Task.FromException(new ArgumentException("xxx"))).Should().ThrowAsync(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
    }

    [Theory]
    [CombinatorialData]
    public async Task ShouldThrowAsync_ThrowAggregateSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Func<ValueTask>(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            await InvokeAsync.That(taskFunc).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(true);
        }
        else
        {
            await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
            {
                await InvokeAsync.That(taskFunc).Should().ThrowAsync(typeof(IOException), includeDerivedType, aggregateExceptionHandling).ConfigureAwait(false);
            });
        }
    }

    [Fact]
    public async Task ShouldNotThrowAsync()
    {
        var exception = await Assert.ThrowsAsync<Xunit.Sdk.ShouldMethodAssertionException>(async () =>
        {
            await InvokeAsync.That(async () => await Task.FromException(new Exception("xxx"))).Should().NotThrowAsync().ConfigureAwait(false);
        });
        Assert.NotNull(exception.InnerException);
    }
}
