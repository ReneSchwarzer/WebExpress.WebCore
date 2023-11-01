using System.Collections.Generic;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Definiert alternative Inhalte, die angezeigt werden sollen, wenn der Browser kein Skripting unterstützt.
    /// </summary>
    public class HtmlElementScriptingNoscript : HtmlElement, IHtmlElementScripting
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementScriptingNoscript()
            : base("span")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementScriptingNoscript(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementScriptingNoscript(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
