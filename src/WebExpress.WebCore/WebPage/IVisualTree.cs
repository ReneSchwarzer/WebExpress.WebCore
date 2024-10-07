using System.Collections.Generic;
using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.WebPage
{
    public interface IVisualTree
    {
        /// <summary>
        /// Returns the favicon.
        /// </summary>
        List<Favicon> Favicons { get; }

        /// <summary>
        /// Returns the internal stylesheet.  
        /// </summary>
        List<string> Styles { get; }

        /// <summary>
        /// Returns the links to the java script files to be used, which are inserted in the header.
        /// </summary>
        List<string> HeaderScriptLinks { get; }

        /// <summary>
        /// Returns the links to the java script files to be used.
        /// </summary>
        List<string> ScriptLinks { get; }

        /// <summary>
        /// Returns the links to the java script files to be used, which are inserted in the header.
        /// </summary>
        List<string> HeaderScripts { get; }

        /// <summary>
        /// Returns the links to the java script files to be used.
        /// </summary>
        IDictionary<string, string> Scripts { get; }

        /// <summary>
        /// Returns the links to the css files to be used.
        /// </summary>
        List<string> CssLinks { get; }

        /// <summary>
        /// Returns the meta information.
        /// </summary>
        List<KeyValuePair<string, string>> Meta { get; }

        /// <summary>
        /// Adds a java script.
        /// </summary>
        /// <param name="url">The link of the java script file.</param>
        void AddScriptLink(string url);

        /// <summary>
        /// Adds a java script in the header.
        /// </summary>
        /// <param name="url">The link of the java script file.</param>
        void AddHeaderScriptLinks(string url);

        /// <summary>
        /// Adds or replaces a java script if it exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="code">The java script code.</param>
        void AddScript(string key, string code);

        /// <summary>
        /// Convert to html.
        /// </summary>
        /// <param name="context">The context for rendering the visual tree.</param>
        /// <returns>The page as html.</returns>
        IHtmlNode Render(IVisualTreeContext context);
    }
}
