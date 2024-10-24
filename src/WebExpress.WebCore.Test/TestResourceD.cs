using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:resourceab1x.label")]
    [Segment("resd", "webindex:homepage.label")]
    public sealed class TestResourceD : IResource
    {
        /// <summary>
        /// Initialization of the resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="resourceContext">The context of the resource.</param>
        /// <param name="resourceManager">The resource manager.</param>
        public TestResourceD(IResourceContext resourceContext, IResourceManager resourceManager)
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
        /// Processes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The processed response.</returns>
        public Response Process(WebMessage.Request request)
        {
            // test the request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Parameter cannot be null or empty.");
            }

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
