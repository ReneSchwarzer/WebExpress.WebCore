using System.Collections.Generic;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// The ul element describes a bulleted list, i.e. a list in which the order of the elements plays a role. 
    /// ol stands for ordered list.
    /// </summary>
    public class HtmlElementTextContentOl : HtmlElement, IHtmlElementTextContent
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<HtmlElementTextContentLi> Elements { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementTextContentOl()
            : base("ol")
        {
            Elements = new List<HtmlElementTextContentLi>();
        }

        /// <summary>
        /// Initializes a new instance of the class.
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
