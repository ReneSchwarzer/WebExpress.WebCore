using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the result of a calculation.
    /// </summary>
    public class HtmlElementFormOutput : HtmlElement, IHtmlElementFormItem
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementFormOutput()
            : base("output")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFormOutput(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
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
