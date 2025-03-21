using OutputMessageSampleMarkdown;

var outputDir = Environment.CurrentDirectory;

if (args.Length != 0)
{
    if (args is ["--output", { Length: > 0 } outputArg])
    {
        outputDir = Path.GetFullPath(outputArg);

        if (!Directory.Exists(outputDir))
        {
            Console.Error.WriteLine($"directory not found.");
            Environment.Exit(-1);
        }
    }
    else
    {
        Console.Error.WriteLine($"invalid args.");
        Console.Error.WriteLine($"> {Path.GetFileName(Environment.ProcessPath)} --output {{OutputDir}}");
        Environment.Exit(-1);
    }
}

var emitMethods = typeof(Samples).GetMethods()
    .Where(v =>
    {
        if (!v.Name.StartsWith("Emit", StringComparison.Ordinal))
            return false;

        var parameter = v.GetParameters();
        if (parameter.Length != 1)
            return false;

        if (parameter[0].ParameterType != typeof(string))
            return false;

        return true;
    });

foreach (var method in emitMethods)
{
    var outputPath = Path.Combine(outputDir, $"{method.Name.Substring("Emit".Length)}.md");

    if (method.ReturnType.IsAssignableTo(typeof(Task)))
    {
        await (Task)method.Invoke(null, [outputPath])!;
    }
    else
    {
        method.Invoke(null, [outputPath]);
    }
}

//Samples.EmitShouldBe(@$"{outputDir}\ShouldBe.md");
//Samples.EmitShouldEqual(@$"{outputDir}\ShouldEqual.md");
