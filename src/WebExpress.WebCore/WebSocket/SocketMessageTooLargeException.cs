using System;

namespace WebExpress.WebCore.WebSocket
{
    /// <summary>
    /// Represents an error that occurs when an incoming WebSocket message
    /// exceeds the maximum allowed message size configured for the endpoint.
    /// </summary>
    public class SocketMessageTooLargeException : Exception
    {
        /// <summary>
        /// Gets the total number of bytes received for the message.
        /// </summary>
        public ulong ActualSize { get; }

        /// <summary>
        /// Gets the maximum allowed message size in bytes.
        /// </summary>
        public ulong MaxSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketMessageTooLargeException"/> class
        /// with the specified actual and maximum message sizes.
        /// </summary>
        /// <param name="actualSize">The number of bytes received.</param>
        /// <param name="maxSize">The maximum allowed message size in bytes.</param>
        public SocketMessageTooLargeException(ulong actualSize, ulong maxSize)
            : base($"WebSocket message size {actualSize} bytes exceeds the maximum allowed size of {maxSize} bytes.")
        {
            ActualSize = actualSize;
            MaxSize = maxSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketMessageTooLargeException"/> class
        /// with a custom error message and the specified actual and maximum message sizes.
        /// </summary>
        /// <param name="message">The custom exception message.</param>
        /// <param name="actualSize">The number of bytes received.</param>
        /// <param name="maxSize">The maximum allowed message size in bytes.</param>
        public SocketMessageTooLargeException(string message, ulong actualSize, ulong maxSize)
            : base(message)
        {
            ActualSize = actualSize;
            MaxSize = maxSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketMessageTooLargeException"/> class
        /// with a custom error message, an inner exception, and the specified size values.
        /// </summary>
        /// <param name="message">The custom exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="actualSize">The number of bytes received.</param>
        /// <param name="maxSize">The maximum allowed message size in bytes.</param>
        public SocketMessageTooLargeException(string message, Exception innerException, ulong actualSize, ulong maxSize)
            : base(message, innerException)
        {
            ActualSize = actualSize;
            MaxSize = maxSize;
        }
    }
}
