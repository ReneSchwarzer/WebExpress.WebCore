using System.Net.WebSockets;
using WebExpress.WebCore.WebSocket;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy web socket for testing purposes.
    /// </summary>
    public sealed class TestSocketA : ISocket
    {
        private readonly ISocketContext _socketContext;
        private readonly ISocketWriteStream _stream;

        /// <summary>
        /// Initializes a new instance of the TestSocketA class using the specified 
        /// socket context and write stream.
        /// </summary>
        /// <param name="socketContext">
        /// The socket context that manages the state and configuration for the socket 
        /// connection.
        /// </param>
        /// <param name="stream">
        /// The write stream used to send data through the socket. Cannot be null.
        /// </param>
        public TestSocketA(ISocketContext socketContext, ISocketWriteStream stream)
        {
            _socketContext = socketContext ?? throw new ArgumentNullException(nameof(stream), "Parameter cannot be null or empty.");
            _stream = stream ?? throw new ArgumentNullException(nameof(stream), "Parameter cannot be null or empty.");
        }

        /// <summary>
        /// Handles logic to be executed when a new connection is established with the 
        /// socket server.
        /// </summary>
        /// <param name="connectMessage">
        /// An optional message containing information about the connection request. May be 
        /// null if no message is provided.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        public async Task OnConnectedAsync(ISocketMessage connectMessage = null, CancellationToken cancellationToken = default)
        {
        }

        /// <summary>
        /// Handles an incoming socket message asynchronously.
        /// </summary>
        /// <param name="message">
        /// The message received from the socket to be processed. Cannot be null.
        /// </param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous message handling operation.
        /// </returns>
        public async Task OnReceiveAsync(ISocketMessage message, CancellationToken cancellationToken = default)
        {
        }

        /// <summary>
        /// Handles logic to be executed when the WebSocket connection is closed.
        /// </summary>
        /// <param name="closeStatus">
        /// The status code indicating the reason for the WebSocket closure.
        /// </param>
        /// <param name="closeDescription">
        /// A description providing additional details about the reason for closure. May be 
        /// null or empty.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        public async Task OnDisconnectedAsync(WebSocketCloseStatus closeStatus, string closeDescription)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles an error that has occurred during asynchronous processing.
        /// </summary>
        /// <param name="exception">
        /// The exception that represents the error to handle. Cannot be null.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous error handling operation.
        /// </returns>
        public async Task OnErrorAsync(Exception exception)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
