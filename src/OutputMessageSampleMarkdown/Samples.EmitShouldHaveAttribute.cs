using ShouldMethodAssertion.ShouldExtensions;
using System.Xml.Linq;

namespace OutputMessageSampleMarkdown;

internal static partial class Samples
{
    public static void EmitShouldHaveAttribute(string outputPath)
    {
        using var writer = new MarkdownWriter(outputPath, "ShouldContain");

        writer.WriteLine($"## Should().HaveAttribute()");

        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XAttribute("itemA", "value"),
            ]);

            var attribute = rootElement.Should().HaveAttribute("itemB"); // attribute is XAttribute of first itemB=
        });
        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XAttribute(XName.Get("itemA", "namespaceA"), "value"),
            ]);

            var attribute = rootElement.Should().HaveAttribute("itemA"); // attribute is XAttribute of first itemA=
        });
        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XAttribute(XName.Get("itemA", "namespaceA"), "value"),
            ]);

            var attribute = rootElement.Should().HaveAttribute(XName.Get("itemA", "namespaceB")); // attribute is XAttribute of first namespaceB:itemA=
        });
        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XAttribute("itemA", "value"),
            ]);

            var attributes = rootElement.Should().HaveAttribute("itemA", 2..3); // attributes is XAttribute[] of all itemA=
        });


        writer.WriteLine($"## Should().NotHaveAttribute()");

        writer.EmitMessageSample(() =>
        {
            var rootElement = new XElement("root", [
                new XAttribute("itemA", "value"),
            ]);

            rootElement.Should().NotHaveAttribute("itemA");
        });
    }
}
