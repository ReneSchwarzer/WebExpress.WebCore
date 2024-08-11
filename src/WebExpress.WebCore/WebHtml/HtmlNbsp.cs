using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a non-breakable character.
    /// </summary>
    public class HtmlNbsp : IHtmlNode
    {
        /// <summary>
        /// Retuens or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlNbsp()
        {
            Value = "&nbsp;";
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
