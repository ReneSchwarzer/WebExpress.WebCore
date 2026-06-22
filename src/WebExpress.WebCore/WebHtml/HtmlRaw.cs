using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// A node that holds ready-made HTML markup and writes it to the output verbatim, without any
    /// escaping. Only use it with trusted markup; for untrusted text use <see cref="HtmlText"/> so
    /// special characters cannot be interpreted as markup.
    /// </summary>
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
