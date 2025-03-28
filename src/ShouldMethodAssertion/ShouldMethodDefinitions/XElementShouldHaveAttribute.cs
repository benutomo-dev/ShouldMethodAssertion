using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;
using System.Xml.Linq;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(XElement))]
#pragma warning disable CA1711 // 識別子は、不適切なサフィックスを含むことはできません
public partial struct XElementShouldHaveAttribute
#pragma warning restore CA1711 // 識別子は、不適切なサフィックスを含むことはできません
{
    public XAttribute ShouldHaveAttribute(string name, string namespaceName)
    {
        return ShouldHaveAttribute(XName.Get(name, namespaceName));
    }

    public XAttribute ShouldHaveAttribute(XName name)
    {
        var attribute = Actual.Attribute(name);

        if (attribute is not null)
            return attribute;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} do not have attribute of {name}.
            """);
    }

    public XAttribute[] ShouldHaveAttribute(string name, string namespaceName, int expectedCount)
    {
        return ShouldHaveAttribute(XName.Get(name, namespaceName), expectedCount);
    }

    public XAttribute[] ShouldHaveAttribute(string name, string namespaceName, Range expectedCountRange)
    {
        return ShouldHaveAttribute(XName.Get(name, namespaceName), expectedCountRange);
    }

    public XAttribute[] ShouldHaveAttribute(XName name, int expectedCount)
    {
        var attributes = Actual.Attributes(name).ToArray();

        if (attributes.Length != expectedCount)
            return attributes;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} contains {attributes.Length} attribute of {name}, but the expected correct number of attributes is {expectedCount}.
            """);
    }

    public XAttribute[] ShouldHaveAttribute(XName name, Range expectedCountRange)
    {
        expectedCountRange.ThrowIfInvalidAsCountRange();

        var attributes = Actual.Attributes(name).ToArray();

        if (expectedCountRange.IsInRange(attributes.Length))
            return attributes;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} contains {attributes.Length} attribute of {name}, but the expected correct range of attributes is {ExpressionUtil.FormatRange(expectedCountRange)}.
            """);
    }

    public void ShouldNotHaveAttribute(string name, string namespaceName)
    {
        ShouldNotHaveAttribute(XName.Get(name, namespaceName));
    }

    public void ShouldNotHaveAttribute(XName name)
    {
        var actualCount = Actual.Attributes(name).Count();

        if (actualCount == 0)
            return;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} contains {actualCount} attribute of {name}, but the expected correct number of attributes is 0.
            """);
    }
}
