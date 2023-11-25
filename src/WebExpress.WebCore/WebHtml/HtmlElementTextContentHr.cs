using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a thematic break between paragraphs of a section, article, or other longer piece of content.
    /// </summary>
    public class HtmlElementTextContentHr : HtmlElement, IHtmlElementTextContent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextContentHr()
            : base("hr", false)
        {

        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            base.ToString(builder, deep);
        }
    }
}
