using System.Globalization;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    public class RenderContext : II18N
    {
        /// <summary>
        /// The page where the control is rendered.
        /// </summary>
        public IPage Page { get; internal set; }

        /// <summary>
        /// Returns the request.
        /// </summary>
        public Request Request { get; internal set; }

        /// <summary>
        /// Returns the host context.
        /// </summary>
        public IHttpServerContext Host => Request.ServerContext;

        /// <summary>
        /// The uri of the request.
        /// </summary>
        public UriResource Uri => Request.Uri;

        /// <summary>
        /// Returns the context path.
        /// </summary>
        public UriResource ContextPath => Page?.ResourceContext?.ContextPath;

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture
        {
            get { return Page?.Culture; }
            set { }
        }

        /// <summary>
        /// Provides the context of the associated application.
        /// </summary>
        public IApplicationContext ApplicationContext => Page?.ApplicationContext;

        /// <summary>
        /// Returns the contents of a page.
        /// </summary>
        public IVisualTree VisualTree { get; protected set; }

        /// <summary>
        /// Returns the log for writing status messages to the console and to a log file.
        /// </summary>
        public Log Log { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RenderContext()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">The page where the control is rendered.</param>
        /// <param name="request">The request.</param>
        /// <param name="visualTree">The visual tree.</param>
        public RenderContext(IPage page, Request request, IVisualTree visualTree)
        {
            Page = page;
            Request = request;
            VisualTree = visualTree;
            Culture = (Page as Resource).Culture;
        }

        /// <summary>
        /// Copy-Constructor
        /// </summary>
        /// <param name="context">The context to copy./param>
        public RenderContext(RenderContext context)
            : this(context?.Page, context?.Request, context?.VisualTree)
        {
        }
    }
}
