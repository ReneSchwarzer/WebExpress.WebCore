using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Segment("a1y", "webindex:homepage.label")]
    [Parent<TestResourceA1X>]
    [Module<TestModuleA1>]
    public sealed class TestResourceA1Y : IResource
    {
        /// <summary>
        /// Instillation of the resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        public TestResourceA1Y()
        {
            ResourceCounter.Add(this);
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

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
