using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Represents the result of a CRUD operation performed via a REST API.
    /// </summary>
    public interface IRestApiResult
    {
        /// <summary>
        /// Converts the current instance into a <see cref="Response"/> object.
        /// </summary>
        /// <returns>A Response object representing the result of the conversion.</returns>
        Response ToResponse();
    }
}