using System.Linq;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a part added to the document.
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
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementEditIns()
            : base("ins")
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementEditIns(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the class.
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
