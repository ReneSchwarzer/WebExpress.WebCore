using System.Collections.Generic;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Markiert die Gruppe der Tabellenzeilen, die die Beschriftungen der Tabellenspalten enthalten.
    /// </summary>
    public class HtmlElementTableThead : HtmlElement, IHtmlElementTable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTableThead()
            : base("thead")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTableThead(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTableThead(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }
    }
}
