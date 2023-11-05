using System.Text;

namespace WebExpress.WebHtml
{
    public class HtmlNbsp : IHtmlNode
    {
        /// <summary>
        /// Liefert oder setzt die Text
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Constructor
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
