using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a short quote. For longer quotes should blockquote be used.
    /// </summary>
    public class HtmlElementTextSemanticsQ : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextSemanticsQ()
            : base("q")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsQ(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
