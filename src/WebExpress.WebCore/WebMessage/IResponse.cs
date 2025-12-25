namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Defines the contract for a response according to RFC 2616 Section 6.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Returns the response header fields.
        /// </summary>
        ResponseHeaderFields Header { get; }

        /// <summary>
        /// Returns or sets the response content.
        /// </summary>
        object Content { get; set; }

        /// <summary>
        /// Returns the status code of the response.
        /// </summary>
        int Status { get; }

        /// <summary>
        /// Returns or sets the reason phrase of the response.
        /// </summary>
        string Reason { get; }
    }
}