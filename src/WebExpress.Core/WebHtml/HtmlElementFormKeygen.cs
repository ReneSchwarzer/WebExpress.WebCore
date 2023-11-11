using System.Text;

namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents a control element for generating a pair of public and private keys and sending the public key.
    /// </summary>
    public class HtmlElementFormKeygen : HtmlElement, IHtmlFormularItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementFormKeygen()
            : base("keygen")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFormKeygen(params IHtmlNode[] nodes)
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
