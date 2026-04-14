using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a variable. This can be an actual mathematical expression or programming 
    /// context, an identifier for a constant, a symbol for a physical quantity, a function 
    /// parameter, or simply a placeholder.
    /// </summary>
    public class HtmlElementTextSemanticsVar : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextSemanticsVar()
            : base("var")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsVar(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
