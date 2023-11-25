using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// The prototype of a website.
    /// </summary>
    /// <typeparam name="T">An implementation of the visualization tree.</typeparam>
    public abstract class Page<T> : Resource, IPage where T : RenderContext, new()
    {
        /// <summary>
        /// Returns or sets the page title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Page()
        {

        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The context of the resource.</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);
        }

        /// <summary>
        /// Redirect to another page.
        /// The function throws the RedirectException.
        /// </summary>
        /// <param name="uri">The uri to redirect to.</param>
        public void Redirecting(string uri)
        {
            throw new RedirectException(uri?.ToString());
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public override Response Process(Request request)
        {
            var context = new T()
            {
                Page = this,
                Request = request
            };

            Process(context);

            return new ResponseOK()
            {
                Content = context.VisualTree.Render(context)
            };
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="context">The context for rendering the page.</param>
        public abstract void Process(T context);
    }
}
