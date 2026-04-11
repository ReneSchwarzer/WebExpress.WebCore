using System;
using System.Collections.Generic;
using System.Text;

namespace WebExpress.WebCore.WebHtml.Parser
{
    /// <summary>
    /// Breaks an HTML string into a flat sequence of <see cref="HtmlToken"/> objects.
    /// </summary>
    /// <remarks>
    /// The tokenizer is deliberately lenient: it makes a best-effort attempt to
    /// produce useful tokens even when the input HTML is malformed.  Recoverable
    /// situations (e.g. an unclosed tag or an unquoted attribute value) are handled
    /// silently; only situations that make further tokenization impossible result in a
    /// <see cref="HtmlParseException"/>.
    /// </remarks>
    public class HtmlTokenizer
    {
        private readonly string _input;
        private int _position;

        /// <summary>
        /// Returns the set of HTML void-element tag names that are always
        /// treated as self-closing even when the input does not include a
        /// trailing slash.
        /// </summary>
        private static readonly HashSet<string> VoidElements =
            new(StringComparer.OrdinalIgnoreCase)
            {
                "area", "base", "br", "col", "embed", "hr", "img", "input",
                "keygen", "link", "meta", "param", "source", "track", "wbr"
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlTokenizer"/> class.
        /// </summary>
        /// <param name="input">The HTML string to tokenize.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is <c>null</c>.</exception>
        public HtmlTokenizer(string input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
        }

        /// <summary>
        /// Tokenizes the entire input and returns all tokens, including a final
        /// <see cref="HtmlTokenType.EndOfFile"/> token.
        /// </summary>
        /// <returns>A list of <see cref="HtmlToken"/> objects.</returns>
        public IReadOnlyList<HtmlToken> Tokenize()
        {
            var tokens = new List<HtmlToken>();
            _position = 0;

            while (_position < _input.Length)
            {
                var token = ReadNextToken();
                if (token != null)
                {
                    tokens.Add(token);
                }
            }

            tokens.Add(new HtmlToken());
            return tokens;
        }

        // ------------------------------------------------------------------
        // Private helpers
        // ------------------------------------------------------------------

        private char Current => _position < _input.Length ? _input[_position] : '\0';
        private char Peek(int offset = 1) => (_position + offset) < _input.Length ? _input[_position + offset] : '\0';

        private HtmlToken ReadNextToken()
        {
            if (Current == '<')
            {
                return ReadTagOrSpecial();
            }

            return ReadText();
        }

        private HtmlToken ReadTagOrSpecial()
        {
            var start = _position;
            _position++; // consume '<'

            if (Current == '!')
            {
                return ReadBangToken(start);
            }

            if (Current == '/')
            {
                return ReadEndTag();
            }

            if (char.IsLetter(Current) || Current == '_')
            {
                return ReadStartTag();
            }

            // Anything else – treat the stray '<' as text.
            return new HtmlToken(HtmlTokenType.Text, "<");
        }

        // Handles <!DOCTYPE ...> and <!-- ... -->
        private HtmlToken ReadBangToken(int start)
        {
            _position++; // consume '!'

            if (Current == '-' && Peek() == '-')
            {
                return ReadComment();
            }

            if (_input.IndexOf("DOCTYPE", _position, StringComparison.OrdinalIgnoreCase) == _position)
            {
                return ReadDoctype();
            }

            // Unknown bang construct – consume until '>' and emit as text.
            var builder = new StringBuilder("<!");
            while (_position < _input.Length && Current != '>')
            {
                builder.Append(Current);
                _position++;
            }
            if (Current == '>')
            {
                builder.Append('>');
                _position++;
            }
            return new HtmlToken(HtmlTokenType.Text, builder.ToString());
        }

        private HtmlToken ReadComment()
        {
            _position += 2; // consume '--'
            var builder = new StringBuilder();

            while (_position < _input.Length)
            {
                if (Current == '-' && Peek() == '-' && Peek(2) == '>')
                {
                    _position += 3; // consume '-->'
                    break;
                }
                builder.Append(Current);
                _position++;
            }

            return new HtmlToken(HtmlTokenType.Comment, builder.ToString().Trim());
        }

        private HtmlToken ReadDoctype()
        {
            _position += 7; // consume 'DOCTYPE'
            SkipWhitespace();

            var nameBuilder = new StringBuilder();
            while (_position < _input.Length && Current != '>' && !char.IsWhiteSpace(Current))
            {
                nameBuilder.Append(Current);
                _position++;
            }

            // Skip anything remaining until '>'
            while (_position < _input.Length && Current != '>')
            {
                _position++;
            }
            if (Current == '>')
            {
                _position++;
            }

            return new HtmlToken(HtmlTokenType.Doctype, nameBuilder.ToString(), []);
        }

        private HtmlToken ReadEndTag()
        {
            _position++; // consume '/'
            SkipWhitespace();

            var tagName = ReadTagName();
            SkipWhitespace();

            // Consume closing '>'
            if (Current == '>')
            {
                _position++;
            }

            return new HtmlToken(HtmlTokenType.EndTag, tagName.ToLowerInvariant(), []);
        }

        private HtmlToken ReadStartTag()
        {
            var tagName = ReadTagName().ToLowerInvariant();
            var attributes = ReadAttributes();

            var selfClose = false;
            if (Current == '/')
            {
                selfClose = true;
                _position++; // consume '/'
            }

            if (Current == '>')
            {
                _position++; // consume '>'
            }

            if (selfClose || VoidElements.Contains(tagName))
            {
                return new HtmlToken(HtmlTokenType.SelfClosingTag, tagName, attributes);
            }

            return new HtmlToken(HtmlTokenType.StartTag, tagName, attributes);
        }

        private string ReadTagName()
        {
            var builder = new StringBuilder();
            while (_position < _input.Length && !char.IsWhiteSpace(Current) &&
                   Current != '>' && Current != '/' && Current != '\0')
            {
                builder.Append(Current);
                _position++;
            }
            return builder.ToString();
        }

        private IReadOnlyList<HtmlTokenAttribute> ReadAttributes()
        {
            var list = new List<HtmlTokenAttribute>();

            while (_position < _input.Length)
            {
                SkipWhitespace();

                if (Current == '>' || Current == '/' || Current == '\0')
                {
                    break;
                }

                var attr = ReadAttribute();
                if (attr != null)
                {
                    list.Add(attr);
                }
            }

            return list;
        }

        private HtmlTokenAttribute ReadAttribute()
        {
            var name = ReadAttributeName();
            if (string.IsNullOrEmpty(name))
            {
                // Skip an unexpected character and continue.
                if (_position < _input.Length)
                {
                    _position++;
                }
                return null;
            }

            SkipWhitespace();

            if (Current != '=')
            {
                // Boolean attribute.
                return new HtmlTokenAttribute(name.ToLowerInvariant());
            }

            _position++; // consume '='
            SkipWhitespace();

            var value = ReadAttributeValue();
            return new HtmlTokenAttribute(name.ToLowerInvariant(), value);
        }

        private string ReadAttributeName()
        {
            var builder = new StringBuilder();
            while (_position < _input.Length && Current != '=' && Current != '>' &&
                   Current != '/' && !char.IsWhiteSpace(Current) && Current != '\0')
            {
                builder.Append(Current);
                _position++;
            }
            return builder.ToString();
        }

        private string ReadAttributeValue()
        {
            if (Current == '"' || Current == '\'')
            {
                var quote = Current;
                _position++; // consume opening quote
                var builder = new StringBuilder();
                while (_position < _input.Length && Current != quote)
                {
                    builder.Append(Current);
                    _position++;
                }
                if (Current == quote)
                {
                    _position++; // consume closing quote
                }
                return builder.ToString();
            }

            // Unquoted value – read until whitespace or '>'.
            var unquotedBuilder = new StringBuilder();
            while (_position < _input.Length && !char.IsWhiteSpace(Current) &&
                   Current != '>' && Current != '\0')
            {
                unquotedBuilder.Append(Current);
                _position++;
            }
            return unquotedBuilder.ToString();
        }

        private HtmlToken ReadText()
        {
            var builder = new StringBuilder();
            while (_position < _input.Length && Current != '<')
            {
                builder.Append(Current);
                _position++;
            }

            var text = builder.ToString();
            return text.Length > 0 ? new HtmlToken(HtmlTokenType.Text, text) : null;
        }

        private void SkipWhitespace()
        {
            while (_position < _input.Length && char.IsWhiteSpace(Current))
            {
                _position++;
            }
        }
    }
}
