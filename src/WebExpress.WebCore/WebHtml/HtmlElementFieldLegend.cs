using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a label for an <fieldset>element.
    /// </summary>
    public class HtmlElementFieldLegend : HtmlElement
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
        public HtmlElementFieldLegend()
            : base("legend")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementFieldLegend(string text)
            : this()
        {
            Text = text;
        }
    }
}
