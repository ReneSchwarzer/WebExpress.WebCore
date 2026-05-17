using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the title of a cite.
    /// </summary>
    public class HtmlElementTextSemanticsCite : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get => string.Join("", Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Clear(); Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextSemanticsCite()
            : base("cite")
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementTextSemanticsCite(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsCite(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
