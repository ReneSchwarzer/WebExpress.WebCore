using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für ein Kontrollelement zur Erzeugung einesPaares aus öffentlichem und privaten Schlüssel und zum Versenden des öffentlichen Schlüssels.
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
