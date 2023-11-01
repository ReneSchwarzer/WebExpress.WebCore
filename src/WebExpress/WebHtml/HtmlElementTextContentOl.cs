using System.Collections.Generic;
using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Das Element ul beschreibt eine Aufzählungsliste, also eine Liste, bei der die Reihenfolge der Elemente eine Rolle spielt. 
    /// ol steht dabei für ordered list, geordnete, sortierte Liste. 
    /// </summary>
    public class HtmlElementTextContentOl : HtmlElement, IHtmlElementTextContent
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<HtmlElementTextContentLi> Elements { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextContentOl()
            : base("ol")
        {
            Elements = new List<HtmlElementTextContentLi>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextContentOl(params HtmlElementTextContentLi[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            base.Elements.Clear();
            base.Elements.AddRange(Elements);

            base.ToString(builder, deep);
        }
    }
}
