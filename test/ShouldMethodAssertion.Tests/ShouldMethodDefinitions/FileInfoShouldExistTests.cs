using ShouldMethodAssertion.ShouldMethodDefinitions;

namespace ShouldMethodAssertion.Tests.ShouldMethodDefinitions;

public class FileInfoShouldExistTests
{
    [Fact]
    public void ShouldExist_NotFail()
    {
        using var tempFile = new TempFile();

        new FileInfoShouldExist(new FileInfo(tempFile.Path), "actual", default).ShouldExist();
    }

    [Fact]
    public void ShouldExist_Fail()
    {
        var empty = Guid.Empty;
        var guid = Guid.NewGuid();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldExist(new FileInfo(@"c:\dummyfolder\notfoundtest\missingfile"), "actual", default).ShouldExist());
    }

    [Fact]
    public void ShouldNotExist_NotFail()
    {
        new FileInfoShouldExist(new FileInfo(@"c:\dummyfolder\notfoundtest\missingfile"), "actual", default).ShouldNotExist();
    }

    [Fact]
    public void ShouldNotExist_Fail()
    {
        using var tempFile = new TempFile();

        Assert.Throws<Xunit.Sdk.ShouldMethodAssertionException>(() => new FileInfoShouldExist(new FileInfo(tempFile.Path), "actual", default).ShouldNotExist());
    }
}
