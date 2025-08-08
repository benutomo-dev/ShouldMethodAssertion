using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class FileInfoShouldSameContentToTests
{
    [Fact]
    public void ShouldSameContentTo_NotFail()
    {
        using var emptyFile1 = new TempFile();
        using var emptyFile2 = new TempFile();

        using var file1 = new TempFile("apple");
        using var file2 = new TempFile("apple");

        new FileInfoShouldSameContentTo(new FileInfo(emptyFile1), "actual", default).ShouldSameContentTo(new FileInfo(emptyFile2));
        new FileInfoShouldSameContentTo(new FileInfo(emptyFile1), "actual", default).ShouldSameContentTo(new FileInfo(emptyFile1));
        new FileInfoShouldSameContentTo(new FileInfo(file1), "actual", default).ShouldSameContentTo(new FileInfo(file2));
        new FileInfoShouldSameContentTo(new FileInfo(file1), "actual", default).ShouldSameContentTo(new FileInfo(file1));
    }

    [Fact]
    public void ShouldSameContentTo_Fail()
    {
        using var file1 = new TempFile();
        using var file2 = new TempFile("apple");
        using var file3 = new TempFile("banana");
        using var file4 = new TempFile("orange");

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldSameContentTo(new FileInfo(file1), "actual", default).ShouldSameContentTo(new FileInfo(file2)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldSameContentTo(new FileInfo(file2), "actual", default).ShouldSameContentTo(new FileInfo(file3)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldSameContentTo(new FileInfo(file3), "actual", default).ShouldSameContentTo(new FileInfo(file4)));
    }

    [Fact]
    public void ShouldNotSameContentTo_NotFail()
    {
        using var file1 = new TempFile();
        using var file2 = new TempFile("apple");
        using var file3 = new TempFile("banana");
        using var file4 = new TempFile("orange");

        new FileInfoShouldSameContentTo(new FileInfo(file1), "actual", default).ShouldNotSameContentTo(new FileInfo(file2));
        new FileInfoShouldSameContentTo(new FileInfo(file2), "actual", default).ShouldNotSameContentTo(new FileInfo(file3));
        new FileInfoShouldSameContentTo(new FileInfo(file3), "actual", default).ShouldNotSameContentTo(new FileInfo(file4));
    }

    [Fact]
    public void ShouldNotBe_Fail()
    {
        using var emptyFile1 = new TempFile();
        using var emptyFile2 = new TempFile();

        using var file1 = new TempFile("apple");
        using var file2 = new TempFile("apple");

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldSameContentTo(new FileInfo(emptyFile1), "actual", default).ShouldNotSameContentTo(new FileInfo(emptyFile2)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldSameContentTo(new FileInfo(emptyFile1), "actual", default).ShouldNotSameContentTo(new FileInfo(emptyFile1)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldSameContentTo(new FileInfo(file1), "actual", default).ShouldNotSameContentTo(new FileInfo(file2)));
        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldSameContentTo(new FileInfo(file1), "actual", default).ShouldNotSameContentTo(new FileInfo(file1)));
    }
}
