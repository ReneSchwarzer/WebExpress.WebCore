using System;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Defines WebSocket close status codes according to RFC 6455.
    /// </summary>
    public enum SocketCloseStatus : ushort
    {
        /// <summary>
        /// Indicates that the connection was closed normally, as defined by the 
        /// WebSocket protocol.
        /// </summary>
        NormalClosure = 1000,

        /// <summary>
        /// Indicates that the connection is closing because the endpoint is going 
        /// away, such as a server shutdown or a browser navigating away from a page.
        /// </summary>
        GoingAway = 1001,

        /// <summary>
        /// Indicates that a protocol error has occurred during communication.
        /// </summary>
        ProtocolError = 1002,

        /// <summary>
        /// Indicates that the received data is not supported by the protocol or 
        /// application.
        /// </summary>
        UnsupportedData = 1003,

        /// <summary>
        /// Indicates that no status code was received from the remote endpoint.
        /// </summary>
        NoStatusReceived = 1005,

        /// <summary>
        /// Indicates that the connection was closed abnormally, without a close 
        /// frame being sent or received.
        /// </summary>
        AbnormalClosure = 1006,

        /// <summary>
        /// Indicates that the received data does not conform to the expected payload 
        /// format or contains invalid data.
        /// </summary>
        InvalidPayloadData = 1007,

        /// <summary>
        /// Indicates that a message was closed because it violated a policy defined 
        /// by the endpoint or server.
        /// </summary>
        PolicyViolation = 1008,

        /// <summary>
        /// Indicates that a message was rejected because its size exceeds the 
        /// maximum allowed limit.
        /// </summary>
        MessageTooBig = 1009,

        /// <summary>
        /// Indicates that the extension is required for the operation to proceed.
        /// </summary>
        MandatoryExtension = 1010,

        /// <summary>
        /// Indicates that an internal server error has occurred.
        /// </summary>
        InternalServerError = 1011
    }

    /// <summary>
    /// Provides helper and conversion methods for <see cref="SocketCloseStatus"/>.
    /// </summary>
    public static class SocketCloseStatusExtensions
    {
        /// <summary>
        /// Returns a human-readable description for the given close status.
        /// </summary>
        public static string GetDescription(this SocketCloseStatus status)
        {
            return status switch
            {
                SocketCloseStatus.NormalClosure => "Normal closure",
                SocketCloseStatus.GoingAway => "Going away",
                SocketCloseStatus.ProtocolError => "Protocol error",
                SocketCloseStatus.UnsupportedData => "Unsupported data",
                SocketCloseStatus.NoStatusReceived => "No status received",
                SocketCloseStatus.AbnormalClosure => "Abnormal closure",
                SocketCloseStatus.InvalidPayloadData => "Invalid payload data",
                SocketCloseStatus.PolicyViolation => "Policy violation",
                SocketCloseStatus.MessageTooBig => "Message too big",
                SocketCloseStatus.MandatoryExtension => "Mandatory extension missing",
                SocketCloseStatus.InternalServerError => "Internal server error",
                _ => "Unknown close status"
            };
        }

        /// <summary>
        /// Returns true if the close status indicates an error condition.
        /// </summary>
        public static bool IsError(this SocketCloseStatus status)
        {
            return status switch
            {
                SocketCloseStatus.NormalClosure => false,
                SocketCloseStatus.GoingAway => false,
                SocketCloseStatus.NoStatusReceived => false,
                _ => true
            };
        }

        /// <summary>
        /// Returns true if the status code is reserved for internal use.
        /// </summary>
        public static bool IsReserved(this SocketCloseStatus status)
        {
            return status switch
            {
                SocketCloseStatus.NoStatusReceived => true,
                SocketCloseStatus.AbnormalClosure => true,
                _ => false
            };
        }

        /// <summary>
        /// Attempts to convert a raw ushort value into a <see cref="SocketCloseStatus"/>.
        /// Returns null if the value is not a valid RFC 6455 close code.
        /// </summary>
        public static SocketCloseStatus? TryParse(ushort code)
        {
            return Enum.IsDefined(typeof(SocketCloseStatus), code)
                ? (SocketCloseStatus)code
                : null;
        }
    }
}