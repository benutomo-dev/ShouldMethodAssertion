using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldThrow(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldThrow");

        var someObject = new { Method = new Func<int>(() => 0) };

        writer.WriteLine($"## Should().Throw()");

        writer.WriteLine($"### Method");
        writer.EmitMessageSample(() =>
        {
            Invoke.That(() => someObject.Method()).Should().Throw<InvalidOperationException>();
        });

        writer.WriteLine($"### Action");
        writer.EmitMessageSample(() =>
        {
            Action action = () => throw new FileNotFoundException();

            action.Should().Throw<IOException>(); // When default, disallow derive exceptions.
        });

        writer.WriteLine($"### Action with args");
        writer.EmitMessageSample(() =>
        {
            Action<int> action = v => throw new FileNotFoundException();

            Invoke.That(() => action(0)).Should().Throw<IOException>(); // When default, disallow derive exceptions.
        });

        writer.WriteLine($"### Include derived exception");
        writer.EmitSuccessSample(() =>
        {
            var action = new Action(() => throw new FileNotFoundException());

            action.Should().Throw<IOException>(includeDerivedType: true); // NotFail
        });

        writer.WriteLine($"### With AggregateException Flatten");
        writer.EmitSuccessSample(() =>
        {
            var action = new Action(() => throw new AggregateException(new FileNotFoundException()));

            action.Should().Throw<IOException>(includeDerivedType: true, aggregateExceptionHandling: ShouldMethodAssertion.AggregateExceptionHandling.AnyFlattened); // NotFail
        });


        someObject = new { Method = new Func<int>(() => throw new InvalidOperationException("smple exception")) };

        writer.WriteLine($"## Should().NotThrow()");

        writer.WriteLine($"### Method");
        writer.EmitMessageSample(() =>
        {
            var returnValueOfMethod = Invoke.That(() => someObject.Method()).Should().NotThrow();
        });

        writer.WriteLine($"### Func");
        writer.EmitMessageSample(() =>
        {
            Action action = () => someObject.Method();

            action.Should().NotThrow();
        });

        writer.WriteLine($"### Func with args");
        writer.EmitMessageSample(() =>
        {
            Func<int, int> action = v => someObject.Method();

            var returnValueOfMethod = Invoke.That(() => action(0)).Should().NotThrow();
        });
    }
}
