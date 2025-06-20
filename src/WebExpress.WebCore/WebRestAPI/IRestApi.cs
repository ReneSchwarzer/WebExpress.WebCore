using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Defines the contract for a rest api resource.
    /// </summary>
    public interface IRestApi : IEndpoint
    {
        /// <summary>
        /// Creates data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        Response CreateData(Request request);

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        Response GetData(Request request);

        /// <summary>
        /// Updates data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        Response UpdateData(Request request);

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        Response DeleteData(Request request);
    }
}
