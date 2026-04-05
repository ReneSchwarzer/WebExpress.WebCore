using WebExpress.WebCore.WebSocket;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy web socket for testing purposes.
    /// </summary>
    public sealed class TestSocketA : ISocket
    {
        private readonly ISocketContext _socketContext;

        /// <summary>
        /// Initializes a new instance of the TestSocketA class using the specified 
        /// socket context and write stream.
        /// </summary>
        /// <param name="socketContext">
        /// The socket context that manages the state and configuration for the socket 
        /// connection.
        /// </param>
        /// <param name="connectionId">The connection id.</param>
        public TestSocketA(ISocketContext socketContext, Guid connectionId)
        {
            _socketContext = socketContext ?? throw new ArgumentNullException(nameof(socketContext), "Parameter cannot be null or empty.");
        }

        /// <summary>
        /// Handles logic to be executed when a new connection is established with the 
        /// socket server.
        /// </summary>
        /// <param name="socketConnection">The socket connection.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        public async Task OnConnectedAsync(ISocketConnection socketConnection)
        {
        }

        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
