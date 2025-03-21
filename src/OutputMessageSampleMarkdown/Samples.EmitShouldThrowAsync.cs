using ShouldMethodAssertion.ShouldExtensions;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static async Task EmitShouldThrowAsync(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldThrowAsync");

        var someObject = new { MethodAsync = new Func<Task<int>>(async () => await Task.FromResult(0)) };

        writer.WriteLine($"## Should().ThrowAsync()");

        writer.WriteLine($"### Method");
        await writer.EmitMessageSampleAsync(async () =>
        {
            await InvokeAsync.That(async () => await someObject.MethodAsync().ConfigureAwait(false)).Should().ThrowAsync<InvalidOperationException>().ConfigureAwait(false);
        }).ConfigureAwait(false);

        writer.WriteLine($"### Func");
        await writer.EmitMessageSampleAsync(async () =>
        {
            Func<Task> actionAsync = () => throw new FileNotFoundException();

            await InvokeAsync.That(actionAsync).Should().ThrowAsync<IOException>().ConfigureAwait(false); // When default, disallow derive exceptions.
        }).ConfigureAwait(false);

        writer.WriteLine($"### Func with args");
        await writer.EmitMessageSampleAsync(async () =>
        {
            Func<int, Task> actionAsync = v => throw new FileNotFoundException();

            await InvokeAsync.That(actionAsync, 0).Should().ThrowAsync<IOException>().ConfigureAwait(false); // When default, disallow derive exceptions.
        }).ConfigureAwait(false);

        writer.WriteLine($"### Include derived exception");
        await writer.EmitSuccessSampleAsync(async () =>
        {
            Func<Task> actionAsync = () => throw new FileNotFoundException();

            await InvokeAsync.That(actionAsync).Should().ThrowAsync<IOException>(includeDerivedType: true).ConfigureAwait(false); // NotFail
        }).ConfigureAwait(false);

        writer.WriteLine($"### With AggregateException Flatten");
        await writer.EmitSuccessSampleAsync(async () =>
        {
            Func<Task> actionAsync = () => throw new AggregateException(new FileNotFoundException());

            await InvokeAsync.That(actionAsync).Should().ThrowAsync<IOException>(
                includeDerivedType: true,
                aggregateExceptionHandling: ShouldMethodAssertion.AggregateExceptionHandling.AnyFlattened
                ).ConfigureAwait(false); // NotFail
        }).ConfigureAwait(false);


        someObject = new { MethodAsync = new Func<Task<int>>(async () => { await Task.CompletedTask.ConfigureAwait(false); throw new InvalidOperationException("smple exception"); }) };

        writer.WriteLine($"## Should().NotThrow()");

        writer.WriteLine($"### Method");
        await writer.EmitMessageSampleAsync(async () =>
        {
            var returnValueOfMethod = await InvokeAsync.That(someObject.MethodAsync).Should().NotThrowAsync().ConfigureAwait(false);
        }).ConfigureAwait(false);

        writer.WriteLine($"### Func");
        await writer.EmitMessageSampleAsync(async () =>
        {
            Func<Task<int>> funcAsync = () => someObject.MethodAsync();

            await InvokeAsync.That(funcAsync).Should().NotThrowAsync().ConfigureAwait(false);
        }).ConfigureAwait(false);

        writer.WriteLine($"### Func with args");
        await writer.EmitMessageSampleAsync(async () =>
        {
            Func<int, Task<int>> funcAsync = v => someObject.MethodAsync();

            var returnValueOfMethod = await InvokeAsync.That(funcAsync, 0).Should().NotThrowAsync().ConfigureAwait(false);
        }).ConfigureAwait(false);
    }
}
