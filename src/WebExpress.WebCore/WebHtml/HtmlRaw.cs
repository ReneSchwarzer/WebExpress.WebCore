using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a raw HTML node.
    /// </summary>
    /// <remarks>
    /// This class is used to encapsulate raw HTML content.
    /// </remarks>
    public class HtmlRaw : IHtmlNode
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlRaw()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="html">The text.</param>
        public HtmlRaw(string html)
        {
            Html = html;
        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            return Html;
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public virtual void ToString(StringBuilder builder, int deep)
        {
            builder.Append(Html);
        }
    }
}
