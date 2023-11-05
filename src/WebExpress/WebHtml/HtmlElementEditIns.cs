using System.Linq;
using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Markiert einen zum Dokument hinzugefügten Teil.
    /// </summary>
    public class HtmlElementEditIns : HtmlElement, IHtmlElementEdit
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
        /// Liefert oder setzt die URI einer Quelle, die die Änderung ausgelöst hat (z.B. eine Ticketnummer in einem Bugtrack-System).
        /// </summary>
        public string Cite
        {
            get => GetAttribute("cite");
            set => SetAttribute("cite", value);
        }

        /// <summary>
        /// Liefert oder setzt die indiziert das Datum und die Uhrzeit, wann The text. geändert wurde. 
        /// Wenn der Wert nicht als Datum mit optionaler Zeitangabe erkannt werden kann, hat dieses Element keinen Bezug zur Zeit.
        /// </summary>
        public string DateTime
        {
            get => GetAttribute("datetime");
            set => SetAttribute("datetime", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementEditIns()
            : base("ins")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementEditIns(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEditIns(params IHtmlNode[] nodes)
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
