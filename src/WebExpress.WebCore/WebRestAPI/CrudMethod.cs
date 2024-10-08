using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Represents the crud methods for a rest api.
    /// </summary>
    public enum CrudMethod
    {
        /// <summary>
        /// Represents the HTTP POST method.
        /// </summary>
        POST = RequestMethod.POST,

        /// <summary>
        /// Represents the HTTP GET method.
        /// </summary>
        GET = RequestMethod.GET,

        /// <summary>
        /// Represents the HTTP PATCH method.
        /// </summary>
        PATCH = RequestMethod.PATCH,

        /// <summary>
        /// Represents the HTTP DELETE method.
        /// </summary>
        DELETE = RequestMethod.DELETE
    }
}
