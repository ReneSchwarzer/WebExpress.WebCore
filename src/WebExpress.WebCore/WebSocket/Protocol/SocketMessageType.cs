namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Defines the type of WebSocket message represented or sent
    /// in the native WebExpress WebSocket protocol.
    /// </summary>
    public enum SocketMessageType
    {
        /// <summary>
        /// Specifies that the data is UTF‑8 encoded text.
        /// </summary>
        Text,

        /// <summary>
        /// Specifies that the data is binary.
        /// </summary>
        Binary,

        /// <summary>
        /// Indicates a close control frame.
        /// </summary>
        Close,

        /// <summary>
        /// Indicates a ping control frame.
        /// </summary>
        Ping,

        /// <summary>
        /// Indicates a pong control frame.
        /// </summary>
        Pong,

        /// <summary>
        /// Indicates a continuation frame for fragmented messages.
        /// </summary>
        Continuation
    }

    /// <summary>
    /// Provides helper and conversion methods for <see cref="SocketMessageType"/>.
    /// </summary>
    public static class SocketMessageTypeExtensions
    {
        /// <summary>
        /// Converts a WebSocket opcode into a <see cref="SocketMessageType"/>.
        /// </summary>
        public static SocketMessageType FromOpcode(int opcode)
        {
            return opcode switch
            {
                0x0 => SocketMessageType.Continuation,
                0x1 => SocketMessageType.Text,
                0x2 => SocketMessageType.Binary,
                0x8 => SocketMessageType.Close,
                0x9 => SocketMessageType.Ping,
                0xA => SocketMessageType.Pong,
                _ => SocketMessageType.Binary // fallback
            };
        }

        /// <summary>
        /// Converts a <see cref="SocketMessageType"/> into the corresponding WebSocket opcode.
        /// </summary>
        public static int ToOpcode(this SocketMessageType type)
        {
            return type switch
            {
                SocketMessageType.Continuation => 0x0,
                SocketMessageType.Text => 0x1,
                SocketMessageType.Binary => 0x2,
                SocketMessageType.Close => 0x8,
                SocketMessageType.Ping => 0x9,
                SocketMessageType.Pong => 0xA,
                _ => 0x2
            };
        }

        /// <summary>
        /// Returns true if the message type represents a control frame.
        /// </summary>
        public static bool IsControl(this SocketMessageType type)
        {
            return type == SocketMessageType.Close
                || type == SocketMessageType.Ping
                || type == SocketMessageType.Pong;
        }

        /// <summary>
        /// Returns true if the message type represents a data frame (text or binary).
        /// </summary>
        public static bool IsData(this SocketMessageType type)
        {
            return type == SocketMessageType.Text
                || type == SocketMessageType.Binary;
        }

        /// <summary>
        /// Returns true if the message type represents a continuation frame.
        /// </summary>
        public static bool IsContinuation(this SocketMessageType type)
        {
            return type == SocketMessageType.Continuation;
        }
    }
}