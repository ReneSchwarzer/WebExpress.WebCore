using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Definiert entweder ein internes Skript oder einen Link auf ein externes Skript. Als Programmiersprache wird JavaScript verwendet.
    /// </summary>
    public class HtmlElementScriptingScript : HtmlElement, IHtmlElementScripting
    {
        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Liefert oder setzt die Scriptsprache
        /// </summary>
        public string Language
        {
            get => GetAttribute("language");
            set => SetAttribute("language", value);
        }

        /// <summary>
        /// Liefert oder setzt den Medientyp
        /// </summary>
        public string Type
        {
            get => GetAttribute("type");
            set => SetAttribute("type", value);
        }

        /// <summary>
        /// Liefert oder setzt den Link auf die Scriptdatei
        /// </summary>
        public string Src
        {
            get => GetAttribute("src");
            set => SetAttribute("src", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementScriptingScript()
            : base("script")
        {
            Type = "text/javascript";
        }

        /// <summary>
        /// Constructor
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
