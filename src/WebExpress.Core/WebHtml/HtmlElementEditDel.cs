using System.Linq;
using System.Text;

namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Selects a part that has been removed from the document.
    /// </summary>
    public class HtmlElementEditDel : HtmlElement, IHtmlElementEdit
    {
        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Text
        {
            get => string.Join(string.Empty, Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Elements.Clear(); Elements.Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Returns or sets the uri of a source that triggered the change (e.g. a ticket number in a bugtrack system).
        /// </summary>
        public string Cite
        {
            get => GetAttribute("cite");
            set => SetAttribute("cite", value);
        }

        /// <summary>
        /// Returns or sets the indexes the date and time when the text was modified..
        /// If the value cannot be recognized as a date with an optional time, this element has no relation to time.
        /// </summary>
        public string DateTime
        {
            get => GetAttribute("datetime");
            set => SetAttribute("datetime", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementEditDel()
            : base("del")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementEditDel(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementEditDel(params IHtmlNode[] nodes)
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
