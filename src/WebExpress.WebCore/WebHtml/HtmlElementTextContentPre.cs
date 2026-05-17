using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the contents of this element as pre-formatted and that you want to keep this format.
    /// </summary>
    public class HtmlElementTextContentPre : HtmlElement, IHtmlElementTextContent
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextContentPre()
            : base("pre")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextContentPre(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
