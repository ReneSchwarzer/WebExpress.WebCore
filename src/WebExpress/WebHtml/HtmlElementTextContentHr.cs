using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Bezeichnet einen thematischen Bruch zwischen Absätzen eines Abschnitts, Artikels oder anderem längeren Inhalt.
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
