using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a single table cell.
    /// </summary>
    public class HtmlElementTableTd : HtmlElement, IHtmlElementTable
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTableTd()
            : base("td")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="node">The content of the html element.</param>
        public HtmlElementTableTd(IHtmlNode node)
            : this()
        {
            Elements.Add(node);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTableTd(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTableTd(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }
    }
}
