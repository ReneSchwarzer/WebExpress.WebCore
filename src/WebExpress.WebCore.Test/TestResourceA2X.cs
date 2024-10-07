using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:resourcea2x.label")]
    [Segment("a2x", "webindex:homepage.label")]
    [ContextPath(null)]
    [Module<TestModuleA2>]
    public sealed class TestResourceA2X : IResource
    {
        /// <summary>
        /// Instillation of the resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="resourceContext">The context of the resource.</param>
        public TestResourceA2X(IResourceManager resourceManager, IResourceContext resourceContext)
        {
            // test the injection
            if (resourceManager == null)
            {
                throw new ArgumentNullException(nameof(resourceManager), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (resourceContext == null)
            {
                throw new ArgumentNullException(nameof(resourceContext), "Parameter cannot be null or empty.");
            }
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
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
