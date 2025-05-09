using Moq;
using ShouldMethodAssertion.ShouldExtensions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

[TestReference([nameof(ShouldMethodAssertion.ShouldMethodDefinitions.ActionShouldThrow)])]
public class ActionShouldThrowTests
{
    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_NoThrow(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var actionMock = new Mock<Action>();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            actionMock.Object.Should().Throw<Exception>(includeDerivedType, aggregateExceptionHandling);
        });

        actionMock.Verify(v => v.Invoke(), Times.Once());
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowUnexpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            new Action(() => throw new ArgumentException("xxx")).Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling);
        });
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        new Action(() => throw new ArgumentException("xxx")).Should().Throw<ArgumentException>(includeDerivedType, aggregateExceptionHandling);
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            taskFunc.Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                taskFunc.Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowNestedAggregateUnexpectedTypeAndSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new AggregateException("xxx", new AggregateException("yyy", new ArgumentException("zzz"), new FileNotFoundException("expected"))));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = taskFunc.Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                taskFunc.Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowNestedAggregateUnexpectedTypeAndMultipleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new AggregateException("xxx",
            new AggregateException("yyy",
                new ArgumentException("zzz"),
                new FileNotFoundException("expected1")
                ),
            new EndOfStreamException("expected2")
            ));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = taskFunc.Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected2", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                taskFunc.Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowAggregateSingleExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        new Action(() => throw new ArgumentException("xxx")).Should().Throw<ArgumentException>(includeDerivedType, aggregateExceptionHandling);
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowAggregateSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            taskFunc.Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                taskFunc.Should().Throw<IOException>(includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_NoThrow(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var actionMock = new Mock<Action>();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            actionMock.Object.Should().Throw(typeof(Exception), includeDerivedType, aggregateExceptionHandling);
        });

        actionMock.Verify(v => v.Invoke(), Times.Once());
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowUnexpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            new Action(() => throw new ArgumentException("xxx")).Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
        });
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        new Action(() => throw new ArgumentException("xxx")).Should().Throw(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling);
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            taskFunc.Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                taskFunc.Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowNestedAggregateUnexpectedTypeAndSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new AggregateException("xxx", new AggregateException("yyy", new ArgumentException("zzz"), new FileNotFoundException("expected"))));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = taskFunc.Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                taskFunc.Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowNestedAggregateUnexpectedTypeAndMultipleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new AggregateException("xxx",
            new AggregateException("yyy",
                new ArgumentException("zzz"),
                new FileNotFoundException("expected1")
                ),
            new EndOfStreamException("expected2")
            ));

        if (includeDerivedType && aggregateExceptionHandling == AggregateExceptionHandling.AnyFlattened)
        {
            var actualException = taskFunc.Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected2", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                taskFunc.Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowAggregateSingleExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        new Action(() => throw new ArgumentException("xxx")).Should().Throw(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling);
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowAggregateSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            taskFunc.Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                taskFunc.Should().Throw(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
            });
        }
    }


    [Fact]
    public void ShouldNotThrow()
    {
        var exception = Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            new Action(() => throw new Exception()).Should().NotThrow();
        });
        Assert.NotNull(exception.InnerException);
    }
}
