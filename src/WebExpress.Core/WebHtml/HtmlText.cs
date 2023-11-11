using System.Text;

namespace WebExpress.Core.WebHtml
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
        /// Constructor
        /// </summary>
        public HtmlText()
        {
        }

        /// <summary>
        /// Constructor
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
