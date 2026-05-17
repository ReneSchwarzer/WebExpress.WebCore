using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a list of definitions of terms and their respective definitions.
    /// </summary>
    public class HtmlElementTextContentDl : HtmlElement, IHtmlElementTextContent
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextContentDl()
            : base("dl")
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextContentDl(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
