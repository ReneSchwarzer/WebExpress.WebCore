using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the title of a document that appears in the browser's title bar in the 
    /// tab of that page. May contain text only. Any tags contained are not interpreted.
    /// </summary>
    public class HtmlElementMetadataTitle : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// The title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMetadataTitle()
            : base("title")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">The title.</param>
        public HtmlElementMetadataTitle(string title)
            : this()
        {
            Title = title;
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            builder.AppendLine();
            builder.Append(string.Empty.PadRight(deep));
            builder.Append("<");
            builder.Append(ElementName);
            builder.Append(">");

            builder.Append(Title);

            builder.Append("</");
            builder.Append(ElementName);
            builder.Append(">");
        }
    }
}
