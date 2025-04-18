namespace WebExpress.WebCore.WebStatusPage
{

    /// <summary>
    /// Represents a status message.
    /// </summary>
    public class StatusMessage
    {
        /// <summary>
        /// Returns the message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public StatusMessage(string message)
        {
            Message = message;
        }
    }
}
