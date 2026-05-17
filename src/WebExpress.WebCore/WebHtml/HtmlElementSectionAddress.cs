using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a section with contact information.
    /// </summary>
    public class HtmlElementSectionAddress : HtmlElement, IHtmlElementSection
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementSectionAddress()
            : base("address")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementSectionAddress(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
