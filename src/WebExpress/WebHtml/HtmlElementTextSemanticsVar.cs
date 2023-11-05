using System.Collections.Generic;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für eine Variable. Dies kann ein tatsächlicher mathematischer Ausdruck oder Programmierungskontext sein, ein Identifier für eine Konstante, ein Symbol für eine physikalische Größe, ein Funktionsparameter oder einfach ein Platzhalter.
    /// </summary>
    public class HtmlElementTextSemanticsVar : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextSemanticsVar()
            : base("var")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsVar(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsVar(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
