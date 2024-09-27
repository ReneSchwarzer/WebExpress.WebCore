using System.Globalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:homepage.label")]
    [Segment(null, "webindex:homepage.label")]
    [ContextPath(null)]
    [Module<TestModuleA1>]
    public sealed class TestPage : IPage
    {
        /// <summary>
        /// Returns or sets the title of the page.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns or sets the resource context.
        /// </summary>
        public IResourceContext ResourceContext { get; private set; }

        /// <summary>
        /// Returns or sets the culture information.
        /// </summary>
        public CultureInfo Culture { get => CultureInfo.CurrentCulture; set => throw new NotImplementedException(); }

        /// <summary>
        /// Instillation of the resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="resourceContext">The context of the resource.</param>
        public void Initialization(IResourceContext resourceContext)
        {
            ResourceContext = resourceContext;
        }

        /// <summary>
        /// Post-processes the request and response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns>The processed response.</returns>
        public Response PostProcess(WebMessage.Request request, Response response)
        {
            return null;
        }

        /// <summary>
        /// Pre-processes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void PreProcess(WebMessage.Request request)
        {

        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The processed response.</returns>
        public Response Process(WebMessage.Request request)
        {
            return null;
        }

        /// <summary>
        /// Redirects to the specified URI.
        /// </summary>
        /// <param name="uri">The URI to redirect to.</param>
        public void Redirecting(string uri)
        {

        }
    }
}
