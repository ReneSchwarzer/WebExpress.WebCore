namespace WebExpress.WebCore.WebHtml.Parser
{
    /// <summary>
    /// Represents a single attribute within an HTML tag token.
    /// </summary>
    public class HtmlTokenAttribute
    {
        /// <summary>
        /// Gets the attribute name (lower-cased).
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the attribute value, or <c>null</c> if the attribute is boolean
        /// (has no explicit value, e.g. <c>disabled</c>).
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets a value indicating whether this is a boolean (valueless) attribute.
        /// </summary>
        public bool IsBoolean => Value is null;

        /// <summary>
        /// Initializes a new valueless (boolean) attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        public HtmlTokenAttribute(string name)
        {
            Name = name;
            Value = null;
        }

        /// <summary>
        /// Initializes a new attribute with a value.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        public HtmlTokenAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Returns a human-readable description of this attribute for debugging purposes.
        /// </summary>
        public override string ToString() =>
            IsBoolean ? Name : $"{Name}=\"{Value}\"";
    }
}
