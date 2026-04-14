namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Defines the contract for a response according to RFC 2616 Section 6.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Gets the response header fields.
        /// </summary>
        ResponseHeaderFields Header { get; }

        /// <summary>
        /// Gets or sets the response content.
        /// </summary>
        object Content { get; set; }

        /// <summary>
        /// Gets the status code of the response.
        /// </summary>
        int Status { get; }

        /// <summary>
        /// Gets the reason phrase of the response.
        /// </summary>
        string Reason { get; }
    }
}