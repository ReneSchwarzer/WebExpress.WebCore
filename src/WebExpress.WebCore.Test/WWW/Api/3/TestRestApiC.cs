using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebRestApi;
using WebExpress.WebCore.WebStatusPage;

namespace WebExpress.WebCore.Test.WWW.Api._3
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    public sealed class TestRestApiC : IRestApi
    {
        /// <summary>
        /// Initialization of the rest api resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="restApiContext">The context of the restapi resource.</param>
        public TestRestApiC(IComponentHub componentHub, IRestApiContext restApiContext)
        {
            // test the injection
            if (componentHub is null)
            {
                throw new ArgumentNullException(nameof(componentHub), "Parameter cannot be null or empty.");
            }

            // test the injection
            if (restApiContext is null)
            {
                throw new ArgumentNullException(nameof(restApiContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Creates data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        public Response CreateData(Request request)
        {
            return new ResponseBadRequest(new StatusMessage("Not implemented."));
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        [Method(RequestMethod.GET)]
        public Response GetData(Request request)
        {
            return new ResponseBadRequest(new StatusMessage("Not implemented."));
        }

        /// <summary>
        /// Updates data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        public Response UpdateData(Request request)
        {
            // test the request
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request), "Parameter cannot be null or empty.");
            }

            return new ResponseBadRequest(new StatusMessage("Not implemented."));
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response containing the result of the operation.</returns>
        public Response DeleteData(Request request)
        {
            // test the request
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request), "Parameter cannot be null or empty.");
            }

            return new ResponseBadRequest(new StatusMessage("Not implemented."));
        }
    }
}
