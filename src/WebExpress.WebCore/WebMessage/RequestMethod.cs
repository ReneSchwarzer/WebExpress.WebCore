namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Enumeration of HTTP request methods.
    /// </summary>
    public enum RequestMethod
    {
        /// <summary>
        /// No request method specified.
        /// </summary>
        NONE,

        /// <summary>
        /// The GET method requests a representation of the specified resource.
        /// </summary>
        GET,

        /// <summary>
        /// The POST method submits an entity to the specified resource.
        /// </summary>
        POST,

        /// <summary>
        /// The PUT method replaces all current representations of the target resource with the request payload.
        /// </summary>
        PUT,

        /// <summary>
        /// The HEAD method asks for a response identical to a GET request, but without the response body.
        /// </summary>
        HEAD,

        /// <summary>
        /// The DELETE method deletes the specified resource.
        /// </summary>
        DELETE,

        /// <summary>
        /// The PATCH method applies partial modifications to a resource.
        /// </summary>
        PATCH // RFC 5789 
    }

    /// <summary>
    /// Provides extension methods for the <see cref="RequestMethod"/> enumeration.
    /// </summary>
    public static class RequestMethodExtensions
    {

        /// <summary>
        /// Converts the <see cref="RequestMethod"/> enumeration value to its string representation.
        /// </summary>
        /// <param name="layout">The <see cref="RequestMethod"/> enumeration value.</param>
        /// <returns>A string representation of the <see cref="RequestMethod"/> enumeration value.</returns>
        public static string ToString(this RequestMethod layout)
        {
            return layout switch
            {
                RequestMethod.GET => "GET",
                RequestMethod.POST => "POST",
                RequestMethod.PUT => "PUT",
                RequestMethod.HEAD => "HEAD",
                RequestMethod.DELETE => "DELETE",
                RequestMethod.PATCH => "PATCH",
                _ => string.Empty,
            };
        }
    }
}
