using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Connects its content to a machine-readable equivalent, specified in the value attribute (this element 
    /// is defined only in the WHATWG version of the html standard, but not in the W3C version of HTML5).
    /// </summary>
    public class HtmlElementTextSemanticsData : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get => string.Join("", Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Clear(); Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextSemanticsData()
            : base("data")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsData(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
