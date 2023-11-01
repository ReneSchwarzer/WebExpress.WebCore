using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für den Wurzelknoten eines HTML- oder XHTML-Dokuments. Alle weiteren Elemente müssen Nachkommen dieses Elements sein.
    /// </summary>
    public class HtmlElementRootHtml : HtmlElement, IHtmlElementRoot
    {
        /// <summary>
        /// Liefert oder setzt den Kopf
        /// </summary>
        public HtmlElementMetadataHead Head { get; private set; }

        /// <summary>
        /// Liefert oder setzt den Body
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
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE html>");
            ToString(builder, 0);

            return builder.ToString();
        }
    }
}
