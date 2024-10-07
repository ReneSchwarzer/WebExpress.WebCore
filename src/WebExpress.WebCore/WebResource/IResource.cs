using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// Defines the contract for a resource component.
    /// </summary>
    public interface IResource : IEndpoint
    {
        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        Response Process(Request request);
    }
}
