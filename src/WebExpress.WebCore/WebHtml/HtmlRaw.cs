using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    public class HtmlRaw : IHtmlNode
    {
        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlRaw()
        {

        }

        /// <summary>
        /// Constructor
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
