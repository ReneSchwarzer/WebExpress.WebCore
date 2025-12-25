using WebExpress.WebCore.WebSocket.Protocol;

/// <summary>
/// Represents a WebSocket close frame containing the close status and 
/// an optional description.
/// </summary>
public class SocketFrameClose : SocketFrame
{
    /// <summary>
    /// Returns the status that indicates the reason the socket was closed.
    /// </summary>
    public SocketCloseStatus Status { get; }

    /// <summary>
    /// Returns the description associated with the current instance.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Initializes a new instance of the SocketFrameClose class with the specified 
    /// close status, description, and raw payload.
    /// </summary>
    /// <param name="status">
    /// The status code indicating the reason for closing the socket connection.
    /// </param>
    /// <param name="description">
    /// A human-readable description providing additional information about the close 
    /// reason. Can be null or empty if no description is needed.
    /// </param>
    /// <param name="rawPayload">
    /// The raw payload data associated with the close frame. Can be null if no 
    /// payload is included.
    /// </param>
    public SocketFrameClose(SocketCloseStatus status, string description, byte[] rawPayload)
    {
        MessageType = SocketMessageType.Close;
        Status = status;
        Description = description;
        Payload = rawPayload;
    }
}