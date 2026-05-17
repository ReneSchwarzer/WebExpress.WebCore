using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// The ul element describes a bulleted list, i.e. a list in which the order of the elements plays 
    /// only a subordinate or no role. ul stands for unordered list. 
    /// </summary>
    public class HtmlElementTextContentUl : HtmlElement, IHtmlElementTextContent
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextContentUl()
            : base("ul")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextContentUl(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextContentUl(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
