using WebExpress.WebCore.WebComponent;
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
        void CreateData(Request request);

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>The data.</returns>
        object GetData();

        /// <summary>
        /// Updates data.
        /// </summary>
        /// <param name="request">The request.</param>
        void UpdateData(Request request);

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="id">The id of the data to delete.</param>
        /// <param name="request">The request.</param>
        void DeleteData(Request request);
    }
}
