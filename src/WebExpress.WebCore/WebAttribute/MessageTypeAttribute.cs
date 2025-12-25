using System;
using WebExpress.WebCore.WebSocket.Protocol;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the status code for an HTTP response (see RFC 7231). 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageTypeAttribute : Attribute, ISocketAttribute
    {
        /// <summary>
        /// Returns the message type code.
        /// </summary>
        public SocketMessageType MessageType { get; }

        /// <summary>
        /// Initializes a new instance of the class with the specified status code.
        /// </summary>
        /// <param name="messageType">The message type.</param>
        public MessageTypeAttribute(SocketMessageType messageType)
        {
            MessageType = messageType;
        }
    }
}
