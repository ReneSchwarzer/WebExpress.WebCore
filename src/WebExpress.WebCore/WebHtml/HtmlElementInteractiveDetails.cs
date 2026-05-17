using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a control that allows the user to obtain additional information or control.
    /// </summary>
    public class HtmlElementInteractiveDetails : HtmlElement, IHtmlElementInteractive
    {
        /// <summary>
        /// Gets the elements.
        /// </summary>
        public new IEnumerable<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementInteractiveDetails()
            : base("details")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementInteractiveDetails(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
        }
    }
}
