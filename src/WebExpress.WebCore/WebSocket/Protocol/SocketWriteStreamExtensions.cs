using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Provides extension methods for writing <see cref="ISocketMessage"/> instances
    /// to an <see cref="ISocketWriteStream"/>.
    /// </summary>
    public static class SocketWriteStreamExtensions
    {
        /// <summary>
        /// Serializes and writes the specified message to the stream and finalizes it.
        /// </summary>
        /// <param name="stream">The target write stream.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
        public static async Task WriteMessageAsync
        (
            this ISocketWriteStream stream,
            ISocketMessage message,
            CancellationToken cancellationToken = default
        )
        {
            if (message is SocketMessageBinary)
            {
                var binary = (message as SocketMessageBinary)?.Data ?? [];
                await stream.WriteAsync(binary, cancellationToken);
            }
            else if (message is SocketMessageText textMessage)
            {
                {
                    var json = textMessage.ToJson();
                    var bytes = Encoding.UTF8.GetBytes(json);
                    await stream.WriteAsync(bytes, cancellationToken);
                }

                await stream.CompleteAsync(cancellationToken);
            }
        }
    }
}