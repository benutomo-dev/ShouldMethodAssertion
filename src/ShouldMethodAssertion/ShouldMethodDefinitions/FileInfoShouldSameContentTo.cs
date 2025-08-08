using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(FileInfo))]
public partial struct FileInfoShouldSameContentTo
{
    public void ShouldSameContentTo(string otherFile) => ShouldSameContentTo(new FileInfo(otherFile));

    public void ShouldSameContentTo(FileInfo otherFile)
    {
        if (!Actual.Exists)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine}({ExpressionUtil.FormartValue(Actual.FullName)}) is not existing.");

        if (!otherFile.Exists)
            throw AssertExceptionUtil.Create($"{ParamExpressions.otherFile.OneLine}({ExpressionUtil.FormartValue(otherFile.FullName)}) is not existing.");

        if (Actual.Length != otherFile.Length)
            throw AssertExceptionUtil.Create($"""
                File sizes do not match.

                {ActualExpression.OneLine}({ExpressionUtil.FormartValue(Actual.FullName)}): {Actual.Length}
                {ParamExpressions.otherFile.OneLine}({ExpressionUtil.FormartValue(otherFile.FullName)}): {otherFile.Length}
                """);

        using var expectedStream = new FileStream(otherFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var actualStream = new FileStream(Actual.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var expectedQuery = Enumerable.Range(0, (int)otherFile.Length).Select(v => expectedStream.ReadByte());
        var actualQuery = Enumerable.Range(0, (int)Actual.Length).Select(v => actualStream.ReadByte());

        var compareResult = expectedQuery.Zip(actualQuery, (e, a) => new { Expected = e, Actual = a }).All(v => v.Expected == v.Actual);

        if (!compareResult)
            throw AssertExceptionUtil.Create($"File contents do not match.");
    }

    public void ShouldNotSameContentTo(string otherFile) => ShouldNotSameContentTo(new FileInfo(otherFile));

    public void ShouldNotSameContentTo(FileInfo otherFile)
    {
        if (!Actual.Exists)
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine}({ExpressionUtil.FormartValue(Actual.FullName)}) is not existing.");

        if (!otherFile.Exists)
            throw AssertExceptionUtil.Create($"{ParamExpressions.otherFile.OneLine}({ExpressionUtil.FormartValue(otherFile.FullName)}) is not existing.");

        if (Actual.Length != otherFile.Length)
            return;

        using var expectedStream = new FileStream(otherFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var actualStream = new FileStream(Actual.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var expectedQuery = Enumerable.Range(0, (int)otherFile.Length).Select(v => expectedStream.ReadByte());
        var actualQuery = Enumerable.Range(0, (int)Actual.Length).Select(v => actualStream.ReadByte());

        var compareResult = expectedQuery.Zip(actualQuery, (e, a) => new { Expected = e, Actual = a }).All(v => v.Expected == v.Actual);

        if (compareResult)
            throw AssertExceptionUtil.Create($"Expected file contents did not match, but matched.");
    }
}
