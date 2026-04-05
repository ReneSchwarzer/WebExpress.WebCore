using System;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Specifies the maximum allowed message size for WebSocket messages
    /// processed by the decorated endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MaxMessageSizeAttribute : Attribute, ISocketAttribute
    {
        /// <summary>
        /// Gets the maximum allowed message size in bytes.
        /// </summary>
        public ulong MaxMessageSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxMessageSizeAttribute"/> class
        /// with the specified maximum message size.
        /// </summary>
        /// <param name="maxMessageSize">
        /// The maximum allowed message size in bytes.
        /// </param>
        public MaxMessageSizeAttribute(ulong maxMessageSize)
        {
            MaxMessageSize = maxMessageSize;
        }
    }
}