using System;
using System.Security.Cryptography;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Provides methods for generating non-deterministic unique identifiers.
    /// </summary>
    /// <remarks>
    /// Unlike <see cref="DeterministicId"/>, this class does not use caching
    /// and does not attempt to produce stable identifiers. Each call produces
    /// a new random identifier.
    /// </remarks>
    public static class RandomId
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        /// <summary>
        /// Generates a random unique identifier. The identifier is not deterministic
        /// and will differ on every call.
        /// </summary>
        /// <returns>
        /// A unique, non-deterministic identifier string.
        /// </returns>
        public static string Create()
        {
            Span<byte> buffer = stackalloc byte[16]; // 128-bit random
            RandomNumberGenerator.Fill(buffer);

            // Convert to hex
            var sb = new StringBuilder(35);
            sb.Append("id_");

            foreach (var b in buffer)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}