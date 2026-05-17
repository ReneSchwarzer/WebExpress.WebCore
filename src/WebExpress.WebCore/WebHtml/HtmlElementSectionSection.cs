using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a section of a document.
    /// </summary>
    public class HtmlElementSectionSection : HtmlElement, IHtmlElementSection
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementSectionSection()
            : base("section")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementSectionSection(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
