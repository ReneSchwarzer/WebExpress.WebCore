using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// The prototype of a rest api.
    /// </summary>
    public abstract class RestApi : IRestApi
    {
        /// <summary>
        /// Returns the rest api context.
        /// </summary>
        public IRestApiContext RestApiContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="restApiContext">The context of the rest api resource.</param>
        public RestApi(IRestApiContext restApiContext)
        {
            RestApiContext = restApiContext;
        }

        /// <summary>
        /// Creates data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        public virtual Response CreateData(Request request)
        {
            return new ResponseBadRequest(new StatusMessage("Not implemented."));
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>The response containing the result of the operation.</returns>
        public virtual Response GetData(Request request)
        {
            return new ResponseBadRequest(new StatusMessage("Not implemented."));
        }

        /// <summary>
        /// Updates data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        public virtual Response UpdateData(Request request)
        {
            return new ResponseBadRequest(new StatusMessage("Not implemented."));
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        public virtual Response DeleteData(Request request)
        {
            return new ResponseBadRequest(new StatusMessage("Not implemented."));
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
