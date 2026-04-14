using System.Globalization;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// The search context for searches within the sitemap.
    /// </summary>
    public class SearchContext
    {
        /// <summary>
        /// Gets the culture.
        /// </summary>
        public CultureInfo Culture { get; internal set; }

        /// <summary>
        /// Gets the http context.
        /// </summary>
        public IHttpContext HttpContext { get; internal set; }

        /// <summary>
        /// Gets the server context.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; internal set; }
    }
}
