using System.Linq;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Can be used to control the text direction of the included child elements. This can be used to explicitly override the unicode BiDi algorithm.
    /// </summary>
    public class HtmlElementTextSemanticsBdo : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Text
        {
            get => string.Join("", Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Elements.Clear(); Elements.Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Returns or sets the writing direction. 
        /// </summary>
        public string Dir
        {
            get => GetAttribute("dir");
            set => SetAttribute("dir", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextSemanticsBdo()
            : base("bdo")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementTextSemanticsBdo(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsBdo(params IHtmlNode[] nodes)
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
            base.ToString(builder, deep);
        }
    }
}
