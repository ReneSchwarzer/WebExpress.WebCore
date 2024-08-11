using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    public class HtmlAttribute : IHtmlAttribute
    {
        /// <summary>
        /// Returns or sets the name of the attribute.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlAttribute()
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name.</param>
        public HtmlAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public HtmlAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Convert to a string using a string builder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public virtual void ToString(StringBuilder builder, int deep)
        {
            builder.Append(Name);
            builder.Append("=\"");
            builder.Append(Value);
            builder.Append("\"");
        }
    }
}
