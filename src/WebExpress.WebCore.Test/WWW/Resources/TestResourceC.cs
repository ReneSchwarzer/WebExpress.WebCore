using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test.WWW.Resources
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    public sealed class TestResourceC : IResource
    {
        /// <summary>
        /// Initialization of the resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="resourceContext">The context of the resource.</param>
        public TestResourceC(IResourceManager resourceManager, IResourceContext resourceContext)
        {
            // test the injection
            if (resourceManager is null)
            {
                throw new ArgumentNullException(nameof(resourceManager), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (resourceContext is null)
            {
                throw new ArgumentNullException(nameof(resourceContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The processed response.</returns>
        public Response Process(Request request)
        {
            // test the request
            if (request is null)
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
