using System;

namespace WebExpress.WebCore.WebHtml.Parser
{
    /// <summary>
    /// Represents an error that occurs while tokenizing or parsing an HTML string.
    /// </summary>
    public class HtmlParseException : Exception
    {
        /// <summary>
        /// Returns the zero-based character position in the input at which the
        /// error was detected, or <c>-1</c> if the position is unknown.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParseException"/> class
        /// with the specified error message.
        /// </summary>
        /// <param name="message">A descriptive message that explains the reason for the failure.</param>
        public HtmlParseException(string message)
            : base(message)
        {
            Position = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParseException"/> class
        /// with the specified error message and the position in the input where the
        /// error occurred.
        /// </summary>
        /// <param name="message">A descriptive message that explains the reason for the failure.</param>
        /// <param name="position">The zero-based character position in the input where the error was detected.</param>
        public HtmlParseException(string message, int position)
            : base(message)
        {
            Position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParseException"/> class
        /// with the specified error message and a reference to the inner exception
        /// that caused this exception.
        /// </summary>
        /// <param name="message">A descriptive message that explains the reason for the failure.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or <c>null</c> if no inner exception is specified.</param>
        public HtmlParseException(string message, Exception innerException)
            : base(message, innerException)
        {
            Position = -1;
        }
    }
}
