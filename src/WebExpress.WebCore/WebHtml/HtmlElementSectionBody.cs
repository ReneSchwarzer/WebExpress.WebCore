using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the main content of an HTML document. Each document can contain only one <body>element.
    /// </summary>
    public class HtmlElementSectionBody : HtmlElement, IHtmlElementSection
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Returns or sets the script elements.
        /// </summary>
        public List<string> Scripts { get; set; }

        /// <summary>
        /// Returns or sets the text/javascript.
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
        /// Returns or sets the external scripts.
        /// </summary>
        private List<HtmlElementScriptingScript> ElementScriptLinks { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementSectionBody()
            : base("body")
        {
            ElementScriptLinks = new List<HtmlElementScriptingScript>();
        }

        /// <summary>
        /// Initializes a new instance of the class.
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
