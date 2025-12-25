using System;
using System.IO;
using System.Text;

namespace WebExpress.WebCore.WebSocket.Protocol
{
    /// <summary>
    /// Parses WebSocket frames from a raw network stream.
    /// </summary>
    public static class SocketFrameParser
    {
        /// <summary>
        /// Reads and parses a single WebSocket frame from the given stream.
        /// </summary>
        /// <param name="stream">The input stream to read from.</param>
        public static SocketFrame ReadFrame(Stream stream)
        {
            var header = new byte[2];
            stream.ReadExactly(header);

            var fin = (header[0] & 0b1000_0000) != 0;
            var opcode = header[0] & 0b0000_1111;
            var masked = (header[1] & 0b1000_0000) != 0;
            var payloadLen = header[1] & 0b0111_1111;

            long actualLength = payloadLen switch
            {
                126 => ReadExtendedLength(stream, 2),
                127 => ReadExtendedLength(stream, 8),
                _ => payloadLen
            };

            byte[] maskKey = [];
            if (masked)
            {
                maskKey = new byte[4];
                stream.ReadExactly(maskKey);
            }

            var payload = new byte[actualLength];
            stream.ReadExactly(payload);

            if (masked)
            {
                for (var i = 0; i < payload.Length; i++)
                {
                    payload[i] ^= maskKey[i % 4];
                }
            }

            var messageType = SocketMessageTypeExtensions.FromOpcode(opcode);

            // special case: close frame
            if (messageType == SocketMessageType.Close)
            {
                SocketCloseStatus status = SocketCloseStatus.NormalClosure;
                string reason = null;

                if (payload.Length >= 2)
                {
                    status = (SocketCloseStatus)((payload[0] << 8) | payload[1]);

                    if (payload.Length > 2)
                    {
                        reason = Encoding.UTF8.GetString(payload, 2, payload.Length - 2);
                    }
                }

                return new SocketFrameClose(status, reason, payload)
                {
                    Fin = fin,
                    Masked = masked
                };
            }

            // default: normal frame
            return new SocketFrame
            {
                Fin = fin,
                MessageType = messageType,
                Masked = masked,
                Payload = payload
            };
        }

        /// <summary>
        /// Reads an extended payload length field (16-bit or 64-bit).
        /// </summary>
        /// <param name="stream">The input stream to read from.</param>
        /// <param name="bytes">The number of bytes to read (2 or 8).</param>
        /// <returns>The parsed length as a long.</returns>
        private static long ReadExtendedLength(Stream stream, int bytes)
        {
            var buffer = new byte[bytes];
            stream.ReadExactly(buffer);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }

            return bytes switch
            {
                2 => BitConverter.ToUInt16(buffer),
                8 => BitConverter.ToInt64(buffer),
                _ => throw new InvalidOperationException("Invalid extended length field size.")
            };
        }
    }
}