using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml.Parser
{
    /// <summary>
    /// Represents a single token produced by the <see cref="HtmlTokenizer"/>.
    /// </summary>
    public class HtmlToken
    {
        /// <summary>
        /// Gets the type of this token.
        /// </summary>
        public HtmlTokenType Type { get; }

        /// <summary>
        /// Gets the tag name for start, end, and self-closing tokens, or the
        /// doctype name for doctype tokens. Returns <c>null</c> for text, comment,
        /// and end-of-file tokens.
        /// </summary>
        public string TagName { get; }

        /// <summary>
        /// Gets the raw text value for text and comment tokens. For start and
        /// self-closing tokens this property is unused (<c>null</c>).
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Gets the attributes associated with start and self-closing tokens.
        /// The key is the attribute name (lower-case) and the value is the
        /// attribute value, or <c>null</c> for boolean (valueless) attributes.
        /// </summary>
        public IReadOnlyList<HtmlTokenAttribute> Attributes { get; }

        /// <summary>
        /// Initializes a new end-of-file token.
        /// </summary>
        public HtmlToken()
        {
            Type = HtmlTokenType.EndOfFile;
            Attributes = [];
        }

        /// <summary>
        /// Initializes a new text or comment token.
        /// </summary>
        /// <param name="type">The token type. Must be <see cref="HtmlTokenType.Text"/> or
        /// <see cref="HtmlTokenType.Comment"/>.</param>
        /// <param name="value">The raw text or comment content.</param>
        public HtmlToken(HtmlTokenType type, string value)
        {
            Type = type;
            Value = value;
            Attributes = [];
        }

        /// <summary>
        /// Initializes a new tag or doctype token.
        /// </summary>
        /// <param name="type">The token type.</param>
        /// <param name="tagName">The tag or doctype name.</param>
        /// <param name="attributes">The list of attributes. May be empty but must not be <c>null</c>.</param>
        public HtmlToken(HtmlTokenType type, string tagName, IReadOnlyList<HtmlTokenAttribute> attributes)
        {
            Type = type;
            TagName = tagName;
            Attributes = attributes ?? [];
        }

        /// <summary>
        /// Returns a human-readable description of this token for debugging purposes.
        /// </summary>
        public override string ToString() => Type switch
        {
            HtmlTokenType.StartTag => $"<{TagName}>",
            HtmlTokenType.EndTag => $"</{TagName}>",
            HtmlTokenType.SelfClosingTag => $"<{TagName}/>",
            HtmlTokenType.Doctype => $"<!DOCTYPE {TagName}>",
            HtmlTokenType.Text => $"\"{Value}\"",
            HtmlTokenType.Comment => $"<!-- {Value} -->",
            HtmlTokenType.EndOfFile => "<EOF>",
            _ => base.ToString()
        };
    }
}
