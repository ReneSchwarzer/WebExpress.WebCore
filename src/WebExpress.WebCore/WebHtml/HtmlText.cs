using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// A node that holds a piece of plain text placed between or inside HTML elements (a text node).
    /// Use it to add textual content to the page; for ready-made markup use <see cref="HtmlRaw"/> instead.
    /// </summary>
    public class HtmlText : IHtmlNode
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlText()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="value">The text.</param>
        public HtmlText(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public virtual void ToString(StringBuilder builder, int deep)
        {
            builder.Append(Value);
        }
    }
}
