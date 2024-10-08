using WebExpress.WebCore.WebMessage;

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
        public virtual void CreateData(Request request)
        {

        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <returns>The data.</returns>
        public virtual object GetData(Request request)
        {
            return null;
        }

        /// <summary>
        /// Updates data.
        /// </summary>
        /// <param name="request">The request.</param>
        public virtual void UpdateData(Request request)
        {

        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="request">The request.</param>
        public virtual void DeleteData(Request request)
        {

        }
    }
}
