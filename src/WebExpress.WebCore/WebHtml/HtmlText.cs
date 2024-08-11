using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a text.
    /// </summary>
    public class HtmlText : IHtmlNode
    {
        /// <summary>
        /// Returns or sets the text.
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
