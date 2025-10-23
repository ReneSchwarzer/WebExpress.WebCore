using System;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents an exception that is thrown when a bad request is encountered.
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BadRequestException(string message = null)
            : base(message ?? "Bad Request")
        {
        }
    }
}
