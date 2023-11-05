using System.Linq;
using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Markiert Text, der vom umgebenden Inhalt zum Zweck der bidirektionalen Formatierung isoliert werden soll. Hiermit kann ein Textabschnitt mit einer unterschiedlichen oder unbekannten Textrichtung gekennzeichnet werden.
    /// </summary>
    public class HtmlElementTextSemanticsBdi : HtmlElement, IHtmlElementTextSemantics
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
        /// Liefert oder setzt die Schreibrichtung 
        /// </summary>
        public string Dir
        {
            get => GetAttribute("dir");
            set => SetAttribute("dir", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextSemanticsBdi()
            : base("bdi")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementTextSemanticsBdi(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsBdi(params IHtmlNode[] nodes)
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
