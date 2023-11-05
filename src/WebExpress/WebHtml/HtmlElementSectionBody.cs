using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für den Hauptinhalt eines HTML-Dokuments. Jedes Dokument kann nur ein <body>-Element enthalten.
    /// </summary>
    public class HtmlElementSectionBody : HtmlElement, IHtmlElementSection
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Liefert oder setzt die Script-Elemente
        /// </summary>
        public List<string> Scripts { get; set; }

        /// <summary>
        /// Liefert oder setzt das text/javascript
        /// </summary>
        public List<string> ScriptLinks
        {
            get => (from x in ElementScriptLinks select x.Src).ToList();
            set
            {
                ElementScriptLinks.Clear();
                ElementScriptLinks.AddRange(from x in value
                                            select new HtmlElementScriptingScript()
                                            {
                                                Language = "javascript",
                                                Src = x,
                                                Type = "text/javascript"
                                            });
            }
        }

        /// <summary>
        /// Liefert oder setzt die externen Scripts
        /// </summary>
        private List<HtmlElementScriptingScript> ElementScriptLinks { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementSectionBody()
            : base("body")
        {
            ElementScriptLinks = new List<HtmlElementScriptingScript>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementSectionBody(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            ToPreString(builder, deep);

            foreach (var v in Elements.Where(x => x != null))
            {
                v.ToString(builder, deep + 1);
            }

            foreach (var v in ElementScriptLinks)
            {
                v.ToString(builder, deep + 1);
            }

            foreach (var script in Scripts)
            {
                new HtmlElementScriptingScript(script).ToString(builder, deep + 1);
            }

            ToPostString(builder, deep, true);
        }
    }
}
