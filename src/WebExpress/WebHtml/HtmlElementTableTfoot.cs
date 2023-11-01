using System.Collections.Generic;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Markiert die Gruppe der Tabellenzeilen, die die Zusammenfassungen der Tabellenspalten enthalten.
    /// </summary>
    public class HtmlElementTableTfoot : HtmlElement, IHtmlElementTable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTableTfoot()
            : base("tfoot")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTableTfoot(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTableTfoot(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }
    }
}
