using System;
using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebHtml.Parser
{
    /// <summary>
    /// Parses an HTML string and reconstructs the corresponding
    /// <see cref="IHtmlNode"/> object tree using the classes from
    /// <c>WebExpress.WebCore.WebHtml</c>.
    /// </summary>
    /// <remarks>
    /// The parser consumes the token stream produced by <see cref="HtmlTokenizer"/>
    /// and builds a DOM-like hierarchy of <see cref="IHtmlNode"/> objects.  It is
    /// deliberately tolerant: unknown tags are mapped to a generic
    /// <see cref="HtmlElement"/>, and unclosed tags are automatically closed when
    /// an end token for an ancestor is encountered (or when the token stream ends).
    /// </remarks>
    public class HtmlParser
    {
        // ------------------------------------------------------------------
        // Public entry points
        // ------------------------------------------------------------------

        /// <summary>
        /// Parses the supplied HTML string and returns the top-level nodes.
        /// </summary>
        /// <param name="html">The HTML string to parse.</param>
        /// <returns>
        /// A read-only list of <see cref="IHtmlNode"/> objects that form the
        /// top-level content of the parsed document.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="html"/> is <c>null</c>.</exception>
        /// <exception cref="HtmlParseException">Thrown when the HTML is so malformed that recovery is impossible.</exception>
        public IReadOnlyList<IHtmlNode> Parse(string html)
        {
            if (html is null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            var tokenizer = new HtmlTokenizer(html);
            var tokens = tokenizer.Tokenize();
            return ParseTokens(tokens);
        }

        /// <summary>
        /// Parses the supplied HTML string and returns the single root node.
        /// </summary>
        /// <param name="html">The HTML string to parse.</param>
        /// <returns>
        /// The first top-level <see cref="IHtmlNode"/>, or <c>null</c> if the
        /// input produces no nodes.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="html"/> is <c>null</c>.</exception>
        public IHtmlNode ParseSingle(string html)
        {
            var nodes = Parse(html);
            return nodes.Count > 0 ? nodes[0] : null;
        }

        // ------------------------------------------------------------------
        // Core parsing logic
        // ------------------------------------------------------------------

        private IReadOnlyList<IHtmlNode> ParseTokens(IReadOnlyList<HtmlToken> tokens)
        {
            var index = 0;
            var roots = new List<IHtmlNode>();

            ParseNodes(tokens, ref index, null, roots);

            return roots;
        }

        /// <summary>
        /// Recursively consumes tokens and appends the resulting nodes to
        /// <paramref name="target"/>.  Parsing stops when an end tag matching
        /// <paramref name="parentTagName"/> is found or the stream ends.
        /// </summary>
        private void ParseNodes(
            IReadOnlyList<HtmlToken> tokens,
            ref int index,
            string parentTagName,
            List<IHtmlNode> target)
        {
            while (index < tokens.Count)
            {
                var token = tokens[index];

                switch (token.Type)
                {
                    case HtmlTokenType.EndOfFile:
                        return;

                    case HtmlTokenType.Doctype:
                        index++;
                        // DOCTYPE tokens are informational; no node is emitted.
                        break;

                    case HtmlTokenType.Comment:
                        index++;
                        target.Add(new HtmlComment(token.Value));
                        break;

                    case HtmlTokenType.Text:
                        index++;
                        var textValue = token.Value;
                        if (!string.IsNullOrEmpty(textValue))
                        {
                            target.Add(new HtmlText(textValue));
                        }
                        break;

                    case HtmlTokenType.SelfClosingTag:
                        index++;
                        var selfClosing = HtmlElementFactory.Create(token.TagName);
                        ApplyAttributes(selfClosing, token.Attributes);
                        target.Add(selfClosing);
                        break;

                    case HtmlTokenType.StartTag:
                        index++;
                        var element = HtmlElementFactory.Create(token.TagName);
                        ApplyAttributes(element, token.Attributes);

                        if (element.CloseTag)
                        {
                            var children = new List<IHtmlNode>();
                            ParseNodes(tokens, ref index, token.TagName, children);
                            element.Add(children.ToArray());
                        }

                        target.Add(element);
                        break;

                    case HtmlTokenType.EndTag:
                        if (string.Equals(token.TagName, parentTagName, StringComparison.OrdinalIgnoreCase))
                        {
                            index++; // consume the matching end tag
                        }
                        // If it doesn't match our parent, stop and let the caller handle it.
                        return;

                    default:
                        index++;
                        break;
                }
            }
        }

        // ------------------------------------------------------------------
        // Attribute mapping
        // ------------------------------------------------------------------

        private static void ApplyAttributes(HtmlElement element, IReadOnlyList<HtmlTokenAttribute> attributes)
        {
            if (attributes is null || attributes.Count == 0)
            {
                return;
            }

            foreach (var attr in attributes)
            {
                if (attr.IsBoolean)
                {
                    element.AddUserAttribute(attr.Name);
                }
                else
                {
                    element.AddUserAttribute(attr.Name, attr.Value);
                }
            }
        }
    }
}
