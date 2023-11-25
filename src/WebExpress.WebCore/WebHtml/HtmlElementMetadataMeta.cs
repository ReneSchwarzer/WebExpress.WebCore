using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Used to define metadata that cannot be defined with any other HTML element.
    /// </summary>
    public class HtmlElementMetadataMeta : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// Returns or sets the attribute name.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Returns or sets the value.
        /// </summary>
        public string Value
        {
            get => GetAttribute(Key);
            set => SetAttribute(Key, value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMetadataMeta()
            : base("meta")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMetadataMeta(string key)
            : this()
        {
            Key = key;
            SetAttribute(Key, "");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementMetadataMeta(string key, string value)
            : this()
        {
            Key = key;
            SetAttribute(Key, value);
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            builder.AppendLine();
            builder.Append(string.Empty.PadRight(deep));
            builder.Append("<");
            builder.Append(ElementName);
            builder.Append(" ");
            builder.Append(Key);
            builder.Append("='");
            builder.Append(Value);
            builder.Append("'>");
        }
    }
}
