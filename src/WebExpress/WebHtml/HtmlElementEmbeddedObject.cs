using System.Collections.Generic;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für allgemeinen externen Inhalt, der je nach Kontext als Bild, 
    /// "verschachtelter Browsing-Kontext" (s. iframe), oder externer Inhalt 
    /// (der mit Hilfe eines Plugins darsgestellt wird) betrachtet wird.
    /// </summary>
    public class HtmlElementEmbeddedObject : HtmlElement, IHtmlElementEmbedded
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementEmbeddedObject()
            : base("object")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEmbeddedObject(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEmbeddedObject(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
