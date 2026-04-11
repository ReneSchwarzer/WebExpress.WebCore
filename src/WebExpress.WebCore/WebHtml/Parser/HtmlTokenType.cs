namespace WebExpress.WebCore.WebHtml.Parser
{
    /// <summary>
    /// Specifies the type of an HTML token produced by the tokenizer.
    /// </summary>
    public enum HtmlTokenType
    {
        /// <summary>
        /// Represents a DOCTYPE declaration, e.g. <c>&lt;!DOCTYPE html&gt;</c>.
        /// </summary>
        Doctype,

        /// <summary>
        /// Represents an opening tag, e.g. <c>&lt;div class="foo"&gt;</c>.
        /// </summary>
        StartTag,

        /// <summary>
        /// Represents a closing tag, e.g. <c>&lt;/div&gt;</c>.
        /// </summary>
        EndTag,

        /// <summary>
        /// Represents a self-closing tag, e.g. <c>&lt;br/&gt;</c> or void elements like <c>&lt;img&gt;</c>.
        /// </summary>
        SelfClosingTag,

        /// <summary>
        /// Represents a text node.
        /// </summary>
        Text,

        /// <summary>
        /// Represents an HTML comment, e.g. <c>&lt;!-- remark --&gt;</c>.
        /// </summary>
        Comment,

        /// <summary>
        /// Represents the end of the token stream.
        /// </summary>
        EndOfFile
    }
}
