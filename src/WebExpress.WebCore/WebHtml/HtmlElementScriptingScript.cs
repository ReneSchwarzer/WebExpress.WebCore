using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents an internal script or a link to an external script. Java script is used as the programming language.
    /// </summary>
    public class HtmlElementScriptingScript : HtmlElement, IHtmlElementScripting
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the scripting language.
        /// </summary>
        public string Language
        {
            get => GetAttribute("language");
            set => SetAttribute("language", value);
        }

        /// <summary>
        /// Gets or sets the media type.
        /// </summary>
        public string Type
        {
            get => GetAttribute("type");
            set => SetAttribute("type", value);
        }

        /// <summary>
        /// Gets or sets the link to the script file.
        /// </summary>
        public string Src
        {
            get => GetAttribute("src");
            set => SetAttribute("src", value);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementScriptingScript()
            : base("script")
        {
            Type = "text/javascript";
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="code">The text.</param>
        public HtmlElementScriptingScript(string code)
            : this()
        {
            Code = code;
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            ToPreString(builder, deep);

            if (!string.IsNullOrWhiteSpace(Code))
            {
                // the code is emitted as written. dropping the line breaks outside a debug
                // build saved a few bytes, but a line break is syntax in JavaScript: after
                // a // comment everything up to the end of the script is swallowed, and
                // statements relying on automatic semicolon insertion run into each other.
                // Both turn a working script into a parse error that appears only in the
                // configuration that strips them. Shrinking source is a minifier's job.
                builder.Append(Code);
            }

            ToPostString(builder, deep, false);
        }
    }
}
