using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// The content of a page is determined by the visual tree.
    /// </summary>
    public class VisualTree : IVisualTree
    {
        /// <summary>
        /// Returns the favicons.
        /// </summary>
        public List<Favicon> Favicons { get; } = [];

        /// <summary>
        /// Returns the internal stylesheet.  
        /// </summary>
        public List<string> Styles { get; } = [];

        /// <summary>
        /// Returns the links to the java script files to be used, which are inserted in the header.
        /// </summary>
        public List<string> HeaderScriptLinks { get; } = [];

        /// <summary>
        /// Returns the links to the java script files to be used.
        /// </summary>
        public List<string> ScriptLinks { get; } = [];

        /// <summary>
        /// Returns the links to the java script files to be used, which are inserted in the header.
        /// </summary>
        public List<string> HeaderScripts { get; } = [];

        /// <summary>
        /// Returns the links to the java script files to be used.
        /// </summary>
        public IDictionary<string, string> Scripts { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Returns the links to the css files to be used.
        /// </summary>
        public List<string> CssLinks { get; } = [];

        /// <summary>
        /// Returns the meta information.
        /// </summary>
        public List<KeyValuePair<string, string>> Meta { get; } = [];

        /// <summary>
        /// Returns the content.
        /// </summary>
        public IHtmlNode Content { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public VisualTree()
        {
        }

        /// <summary>
        /// Adds or replaces a java script if it exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="code">The java script code.</param>
        public virtual void AddScript(string key, string code)
        {
            if (key == null) return;

            var k = key.ToLower();
            var dict = Scripts;

            if (dict.ContainsKey(k))
            {
                dict[k] = code;
            }
            else
            {
                dict?.Add(k, code);
            }
        }

        /// <summary>
        /// Adds a java script.
        /// </summary>
        /// <param name="url">The link of the java script file.</param>
        public virtual void AddScriptLink(string url)
        {
            ScriptLinks?.Add(url);
        }

        /// <summary>
        /// Adds a java script in the header.
        /// </summary>
        /// <param name="url">The link of the java script file.</param>
        public virtual void AddHeaderScriptLinks(string url)
        {
            HeaderScriptLinks?.Add(url);
        }

        /// <summary>
        /// Convert to html.
        /// </summary>
        /// <param name="context">The context for rendering the page.</param>
        /// <returns>The page as an html tree.</returns>
        public virtual IHtmlNode Render(RenderContext context)
        {
            var html = new HtmlElementRootHtml();
            html.Head.Title = InternationalizationManager.I18N(context.Request, context.Page?.Title);
            html.Head.Favicons = Favicons?.Select(x => new Favicon(x.Url, x.Mediatype));
            //html.Head.Base = Context.ContextPath.ToString();
            html.Head.Styles = Styles;
            html.Head.Meta = Meta;
            html.Head.Scripts = HeaderScripts;
            html.Body.Elements.Add(Content);
            html.Body.Scripts = [.. Scripts.Values];

            html.Head.CssLinks = CssLinks.Where(x => x != null).Select(x => x.ToString());
            html.Head.ScriptLinks = HeaderScriptLinks?.Where(x => x != null).Select(x => x.ToString());

            return html;
        }
    }
}
