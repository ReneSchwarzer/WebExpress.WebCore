using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Verbindet seinen Inhalt mit einem maschinenlesbaren Equivalent, angegeben im value-Attribut. (Dieses Element wird nur in der WHATWG-Version des HTML-Standards definiert, nicht aber in der W3C-Version von HTML5).
    /// </summary>
    public class HtmlElementTextSemanticsData : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Returns or sets the value.
        /// </summary>
        public string Value
        {
            get => string.Join("", Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Elements.Clear(); Elements.Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextSemanticsData()
            : base("data")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsData(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsData(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
