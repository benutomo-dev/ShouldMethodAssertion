using Moq;
using ShouldMethodAssertion.ShouldMethodDefinitions;

#pragma warning disable CA2263 // 型が既知の場合はジェネリック オーバーロードを優先する

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class ActionShouldThrowTests
{
    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_NoThrow(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var actionMock = new Mock<Action>();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            new ActionShouldThrow(actionMock.Object, "actual", default).ShouldThrow<Exception>(includeDerivedType, aggregateExceptionHandling);
        });

        actionMock.Verify(v => v.Invoke(), Times.Once());
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowUnexpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            new ActionShouldThrow(new Action(() => throw new ArgumentException("xxx")), "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling);
        });
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        new ActionShouldThrow(new Action(() => throw new ArgumentException("xxx")), "actual", default).ShouldThrow<ArgumentException>(includeDerivedType, aggregateExceptionHandling);
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling);
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
            var actualException = new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling);
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
            var actualException = new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected2", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowAggregateSingleExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new AggregateException("xxx", new ArgumentException("expected")));

        if (aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            var actualException = new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<ArgumentException>(includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<ArgumentException>(includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrowT_ThrowAggregateSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new AggregateException("xxx", new FileNotFoundException()));

        if (includeDerivedType && aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow<IOException>(includeDerivedType, aggregateExceptionHandling);
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
            new ActionShouldThrow(actionMock.Object, "actual", default).ShouldThrow(typeof(Exception), includeDerivedType, aggregateExceptionHandling);
        });

        actionMock.Verify(v => v.Invoke(), Times.Once());
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowUnexpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            new ActionShouldThrow(new Action(() => throw new ArgumentException("xxx")), "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
        });
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        new ActionShouldThrow(new Action(() => throw new ArgumentException("xxx")), "actual", default).ShouldThrow(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling);
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new FileNotFoundException());

        if (includeDerivedType)
        {
            new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
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
            var actualException = new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
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
            var actualException = new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected2", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowAggregateSingleExactExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new AggregateException("xxx", new ArgumentException("expected")));

        if (aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            var actualException = new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling).Exception;
            Assert.Equal("expected", actualException.Message);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(ArgumentException), includeDerivedType, aggregateExceptionHandling);
            });
        }
    }

    [Theory]
    [CombinatorialData]
    public void ShouldThrow_ThrowAggregateSingleDerivedExpectedType(bool includeDerivedType, AggregateExceptionHandling aggregateExceptionHandling)
    {
        var taskFunc = new Action(() => throw new AggregateException("xxx", new FileNotFoundException()));

        if (includeDerivedType && aggregateExceptionHandling != AggregateExceptionHandling.None)
        {
            new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
        }
        else
        {
            Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
            {
                new ActionShouldThrow(taskFunc, "actual", default).ShouldThrow(typeof(IOException), includeDerivedType, aggregateExceptionHandling);
            });
        }
    }


    [Fact]
    public void ShouldNotThrow()
    {
        var exception = Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() =>
        {
            new ActionShouldThrow(new Action(() => throw new Exception()), "actual", default).ShouldNotThrow();
        });
        Assert.NotNull(exception.InnerException);
    }
}
