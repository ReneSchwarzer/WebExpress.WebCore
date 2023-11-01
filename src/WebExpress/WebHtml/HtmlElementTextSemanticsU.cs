using System.Linq;
using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für einen Textabschnitt, der vom übrigen Inhalt abgesetzt und üblicherweise unterstrichen dargestellt wird, ohne für eine spezielle Betonung oder Wichtigkeit zu stehen. Dies könnte beispielsweise ein Eigenname auf in chinesischer Sprache sein oder ein Textabschnitt, der häufig falsch buchstabiert wird.
    /// </summary>
    public class HtmlElementTextSemanticsU : HtmlElement, IHtmlElementTextSemantics
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
        /// Constructor
        /// </summary>
        public HtmlElementTextSemanticsU()
            : base("u")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementTextSemanticsU(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsU(params IHtmlNode[] nodes)
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
