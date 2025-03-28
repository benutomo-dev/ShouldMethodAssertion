using ShouldMethodAssertion.ShouldExtensions;
using System.Xml.Linq;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldHaveElement(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldContain");

        writer.WriteLine($"## Should().HaveElement()");

        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XElement("itemA"),
            ]);

            var element = rootElement.Should().HaveElement("itemB"); // element is XElement of first <itemB>
        });
        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XElement(XName.Get("itemA", "namespaceA")),
            ]);

            var element = rootElement.Should().HaveElement("itemA"); // element is XElement of first <itemA>
        });
        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XElement(XName.Get("itemA", "namespaceA")),
            ]);

            var element = rootElement.Should().HaveElement(XName.Get("itemA", "namespaceB")); // element is XElement of first <namespaceB:itemA>
        });
        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XElement("itemA"),
            ]);

            var elements = rootElement.Should().HaveElement("itemA", 2..3); // elements is XElement[] of all <itemA>
        });


        writer.WriteLine($"## Should().NotHaveElement()");

        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XElement("itemA"),
            ]);

            rootElement.Should().NotHaveElement("itemA");
        });
    }
}
