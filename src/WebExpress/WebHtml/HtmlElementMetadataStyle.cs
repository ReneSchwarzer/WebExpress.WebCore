using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Definition eines internen CSS-Stylesheets.
    /// </summary>
    public class HtmlElementMetadataStyle : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMetadataStyle()
            : base("style")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">The text.</param>
        public HtmlElementMetadataStyle(string code)
            : this()
        {
            Code = code;
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            builder.Append(string.Empty.PadRight(deep));
            builder.Append("<");
            builder.Append(ElementName);
            builder.Append(">");

            if (!string.IsNullOrWhiteSpace(Code))
            {
                builder.Append("\n");
                builder.Append(Code);
                builder.Append("\n");
            }
            builder.Append(string.Empty.PadRight(deep));
            builder.Append("</");
            builder.Append(ElementName);
            builder.Append(">");
            builder.Append("\n");
        }
    }
}
