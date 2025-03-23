using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(IEnumerable<>))]
public partial struct EnumerableShouldBeEmpty<T>
{
    public void ShouldBeEmpty()
    {
        var nonEnumeratedCount = Actual.TryGetNonEnumeratedCount(out var _nonEnumeratedCount) ? _nonEnumeratedCount : default(int?);

        var (headValues, hasMoreValues, enumeratedCount) = SequenceHelper.GetHeadValues(new CommonEnumerator<T>(Actual.GetEnumerator()), nonEnumeratedCount);

        if (headValues?.Count > 0)
            throw AssertExceptionUtil.CreateBasicShouldEmptyFail(headValues, hasMoreValues, enumeratedCount, ActualExpression);
    }

    public void ShouldNotBeEmpty()
    {
        if (!Actual.Any())
            throw AssertExceptionUtil.Create($"{ActualExpression.OneLine} is empty.");
    }
}
