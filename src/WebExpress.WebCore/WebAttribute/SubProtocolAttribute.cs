using System;
using System.Net.WebSockets;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the sub protocole for an WebSocket (see RFC 6455). 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SubProtocolAttribute : Attribute, ISocketAttribute
    {
        /// <summary>
        /// Returns the sub protocol.
        /// </summary>
        public string SubProtocol { get; }

        /// <summary>
        /// Initializes a new instance of the class with the specified sub protocol.
        /// </summary>
        /// <param name="subProtocol">The sub protocol.</param>
        public SubProtocolAttribute(string subProtocol)
        {
            SubProtocol = subProtocol;
        }
    }
}
