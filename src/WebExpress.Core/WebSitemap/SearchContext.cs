using System.Globalization;
using WebExpress.Core.WebMessage;

namespace WebExpress.Core.WebSitemap
{
    /// <summary>
    /// The search context for searches within the sitemap.
    /// </summary>
    public class SearchContext
    {
        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture { get; internal set; }

        /// <summary>
        /// Returns the http context.
        /// </summary>
        public HttpContext HttpContext { get; internal set; }

        /// <summary>
        /// Returns the server context.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; internal set; }
    }
}
