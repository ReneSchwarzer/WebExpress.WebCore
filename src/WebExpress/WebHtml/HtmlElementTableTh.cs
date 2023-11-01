using System.Collections.Generic;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Kennzeichnet eine Tabellenzelle mit einer Beschriftung.
    /// </summary>
    public class HtmlElementTableTh : HtmlElement, IHtmlElementTable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTableTh()
            : base("th")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTableTh(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTableTh(List<IHtmlNode> nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }
    }
}
