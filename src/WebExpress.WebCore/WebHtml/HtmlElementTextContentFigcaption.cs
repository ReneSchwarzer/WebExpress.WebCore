using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the caption of an image.
    /// </summary>
    public class HtmlElementTextContentFigcaption : HtmlElement, IHtmlElementTextContent
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextContentFigcaption()
            : base("figcaption")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextContentFigcaption(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
