namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the status code for an HTTP response (see RFC 7231). 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class StatusCodeAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the class with the specified status code.
        /// </summary>
        /// <param name="code">The status code.</param>
        public StatusCodeAttribute(int code)
        {
            StatusCode = code;
        }
    }
}
