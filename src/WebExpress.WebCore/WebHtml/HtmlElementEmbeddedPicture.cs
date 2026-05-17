using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a picture.
    /// </summary>
    public class HtmlElementEmbeddedPicture : HtmlElement, IHtmlElementEmbedded
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementEmbeddedPicture()
            : base("picture")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEmbeddedPicture(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
