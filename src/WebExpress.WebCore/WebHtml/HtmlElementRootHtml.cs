using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the root node of an html or xhtml document. All other elements must 
    /// be descendants of this element.
    /// </summary>
    public class HtmlElementRootHtml : HtmlElement, IHtmlElementRoot
    {
        /// <summary>
        /// Returns or sets the head.
        /// </summary>
        public HtmlElementMetadataHead Head { get; private set; }

        /// <summary>
        /// Returns or sets the body.
        /// </summary>
        public HtmlElementSectionBody Body { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementRootHtml()
            : base("html")
        {
            Head = new HtmlElementMetadataHead();
            Body = new HtmlElementSectionBody();
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            builder.Append("<");
            builder.Append(ElementName);
            builder.Append(">");

            Head.ToString(builder, deep + 1);
            Body.ToString(builder, deep + 1);

            builder.AppendLine();
            builder.Append("</");
            builder.Append(ElementName);
            builder.Append(">");
            builder.Append("\n");
        }

        /// <summary>
        /// Convert to string.
        /// </summary>
        /// <returns>The object as a string.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE html>");
            ToString(builder, 0);

            return builder.ToString();
        }
    }
}
