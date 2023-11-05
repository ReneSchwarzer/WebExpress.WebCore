using System.Collections.Generic;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Definiert einen Rahmen, mit dem ein HTML-Dokument in seinem eigenen Kontext in das aktuelle Dokument eingebettet werden kann.
    /// </summary>
    public class HtmlElementEmbeddedIframe : HtmlElement, IHtmlElementEmbedded
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementEmbeddedIframe()
            : base("iframe")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEmbeddedIframe(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEmbeddedIframe(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
