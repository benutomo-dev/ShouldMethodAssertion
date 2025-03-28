using ShouldMethodAssertion.DataAnnotations;
using ShouldMethodAssertion.ShouldMethodDefinitions.Utils;
using System.Xml.Linq;

namespace ShouldMethodAssertion.ShouldMethodDefinitions;

[ShouldMethodDefinition(typeof(XElement))]
public partial struct XElementShouldHaveElement
{
    public XElement ShouldHaveElement(string name, string namespaceName)
    {
        return ShouldHaveElement(XName.Get(name, namespaceName));
    }

    public XElement ShouldHaveElement(XName name)
    {
        var element = Actual.Element(name);

        if (element is not null)
            return element;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} do not have {name}.
            """);
    }

    public XElement[] ShouldHaveElement(string name, string namespaceName, int expectedCount)
    {
        return ShouldHaveElement(XName.Get(name, namespaceName), expectedCount);
    }

    public XElement[] ShouldHaveElement(string name, string namespaceName, Range expectedCountRange)
    {
        return ShouldHaveElement(XName.Get(name, namespaceName), expectedCountRange);
    }

    public XElement[] ShouldHaveElement(XName name, int expectedCount)
    {
        var elements = Actual.Elements(name).ToArray();

        if (elements.Length != expectedCount)
            return elements;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} contains {elements.Length} {name}, but the expected correct number of elements is {expectedCount}.
            """);
    }

    public XElement[] ShouldHaveElement(XName name, Range expectedCountRange)
    {
        expectedCountRange.ThrowIfInvalidAsCountRange();

        var elements = Actual.Elements(name).ToArray();

        if (expectedCountRange.IsInRange(elements.Length))
            return elements;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} contains {elements.Length} {name}, but the expected correct range of elements is {ExpressionUtil.FormatRange(expectedCountRange)}.
            """);
    }

    public void ShouldNotHaveElement(string name, string namespaceName)
    {
        ShouldNotHaveElement(XName.Get(name, namespaceName));
    }

    public void ShouldNotHaveElement(XName name)
    {
        var actualCount = Actual.Elements(name).Count();

        if (actualCount == 0)
            return;

        throw AssertExceptionUtil.Create($"""
            {ActualExpression.OneLine} contains {actualCount} {name}, but the expected correct number of elements is 0.
            """);
    }
}
