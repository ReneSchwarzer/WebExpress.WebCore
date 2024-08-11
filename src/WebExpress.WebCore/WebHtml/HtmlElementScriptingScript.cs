using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents an internal script or a link to an external script. Java script is used as the programming language.
    /// </summary>
    public class HtmlElementScriptingScript : HtmlElement, IHtmlElementScripting
    {
        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Returns or sets the scripting language.
        /// </summary>
        public string Language
        {
            get => GetAttribute("language");
            set => SetAttribute("language", value);
        }

        /// <summary>
        /// Returns or sets the media type.
        /// </summary>
        public string Type
        {
            get => GetAttribute("type");
            set => SetAttribute("type", value);
        }

        /// <summary>
        /// Returns or sets the link to the script file.
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
#if DEBUG
                builder.Append(Code);
#else
                builder.Append(Code.Replace("\r", "").Replace("\n", ""));
#endif
            }

            ToPostString(builder, deep, false);
        }
    }
}
