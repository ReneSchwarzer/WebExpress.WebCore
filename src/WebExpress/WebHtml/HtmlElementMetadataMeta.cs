using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Wird für die Definition von Metadaten verwenden, die mit keinem anderen HTML-Element definiert werden können.
    /// </summary>
    public class HtmlElementMetadataMeta : HtmlElement, IHtmlElementMetadata
    {
        /// <summary>
        /// Liefert oder setzt den Attributnamen
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
