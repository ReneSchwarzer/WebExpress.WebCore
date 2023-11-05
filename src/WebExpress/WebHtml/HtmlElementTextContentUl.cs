using System.Collections.Generic;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Das Element ul beschreibt eine Aufzählungsliste, also eine Liste, bei der die Reihenfolge der Elemente nur eine 
    /// untergeordnete oder keine Rolle spielt. ul steht dabei für unordered list, ungeordnete, unsortierte Liste. 
    /// </summary>
    public class HtmlElementTextContentUl : HtmlElement, IHtmlElementTextContent
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextContentUl()
            : base("ul")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextContentUl(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextContentUl(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
