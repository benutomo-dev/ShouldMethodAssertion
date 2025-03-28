using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IEnumerable<>))]
public partial struct EnumerableShouldHaveCount<T>
{
    public void ShouldHaveCount(int count)
    {
        var actualCount = Actual.Count();

        if (actualCount != count)
            throw AssertExceptionUtil.Create($"Count of {ActualExpression.OneLine} is {actualCount}. But it expected to be {count}.");
    }

    public void ShouldNotHaveCount(int count)
    {
        var actualCount = Actual.Count();

        if (actualCount == count)
            throw AssertExceptionUtil.Create($"Count of {ActualExpression.OneLine} is {actualCount}. But did not expect it to be.");
    }
}
