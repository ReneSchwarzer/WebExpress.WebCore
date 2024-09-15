using System.Globalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    public class RenderContext
    {
        /// <summary>
        /// Returns the page where is rendered.
        /// </summary>
        public IPage Page { get; internal set; }

        /// <summary>
        /// Returns the request.
        /// </summary>
        public Request Request { get; internal set; }

        /// <summary>
        /// The uri of the request.
        /// </summary>
        public UriResource Uri => Request?.Uri;

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture => Request?.Culture;

        /// <summary>
        /// Provides the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext => Page?.ResourceContext?.PluginContext;

        /// <summary>
        /// Provides the context of the associated application.
        /// </summary>
        public IApplicationContext ApplicationContext => Page?.ResourceContext?.ModuleContext?.ApplicationContext;

        /// <summary>
        /// Returns the contents of a page.
        /// </summary>
        public IVisualTree VisualTree { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public RenderContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="page">The page where is rendered.</param>
        /// <param name="request">The request.</param>
        /// <param name="visualTree">The visual tree.</param>
        public RenderContext(IPage page, Request request, IVisualTree visualTree)
        {
            Page = page;
            Request = request;
            VisualTree = visualTree;
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
