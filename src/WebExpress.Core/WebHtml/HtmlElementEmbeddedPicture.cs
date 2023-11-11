using System.Collections.Generic;

namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents a picture.
    /// </summary>
    public class HtmlElementEmbeddedPicture : HtmlElement, IHtmlElementEmbedded
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementEmbeddedPicture()
            : base("picture")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEmbeddedPicture(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEmbeddedPicture(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
