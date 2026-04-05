using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Provides methods for generating deterministic unique identifiers 
    /// based on the caller's file path, line number, and an optional 
    /// index value.
    /// </summary>
    /// <remarks>
    /// This class utilizes a caching mechanism to ensure that repeated 
    /// calls with the same parameters return the same identifier, 
    /// enhancing performance and consistency.
    /// </remarks>
    public static class DeterministicId
    {
        private static readonly ConcurrentDictionary<string, string> Cache = new();

        /// <summary>
        /// Generates a deterministic unique identifier based on the caller's file
        /// path, line number, and an optional index value.
        /// </summary>
        /// <remarks>
        /// This method uses a caching mechanism to ensure that repeated calls with 
        /// the same parameters return the same identifier. The generated identifier 
        /// is based on a FNV-1a hash of the signature formed from the file path, 
        /// line number, and index.
        /// </remarks>
        /// <param name="context">
        /// An optional object that provides additional context for generating the 
        /// identifier. If specified, its hash code is included in the identifier 
        /// to further distinguish it.
        /// </param>
        /// <param name="file">
        /// The full path of the source file where the method is called. This 
        /// value is automatically supplied by the compiler.
        /// </param>
        /// <param name="line">
        /// The line number in the source file where the method is called. This 
        /// value is automatically supplied by the compiler.
        /// </param>
        /// <returns>
        /// A unique identifier string that represents the combination of the file path, 
        /// line number, and optional index.
        /// </returns>
        public static string Create
        (
            object context = null,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0
        )
        {
            var stack = new StackTrace(skipFrames: 1, fNeedFileInfo: false);
            var frames = stack.GetFrames();

            var sb = new StringBuilder(128);

            sb.Append(file);
            sb.Append(':');
            sb.Append(line);

            if (context is not null)
            {
                sb.Append(':');
                sb.Append(context.GetHashCode());
            }

            foreach (var f in frames)
            {
                var m = f.GetMethod();
                sb.Append(m.Name);
                sb.Append(f.GetILOffset());
            }

            var signature = sb.ToString();

            if (Cache.TryGetValue(signature, out var cached))
            {
                return cached;
            }

            // fnv-1a hash
            var hash = Fnv1a(signature);

            var id = "id_" + hash.ToString("X");

            Cache[signature] = id;

            return id;
        }

        /// <summary>
        /// Calculates the 32-bit FNV-1a hash value for the specified string.
        /// </summary>
        /// <remarks>
        /// The FNV-1a algorithm is a non-cryptographic hash function known 
        /// for its simplicity and speed. It is commonly used for hash tables
        /// and checksums, but should not be used for cryptographic purposes.
        /// </remarks>
        /// <param name="text">
        /// The input string for which to compute the hash. This parameter 
        /// cannot be null.
        /// </param>
        /// <returns>
        /// A 32-bit unsigned integer representing the FNV-1a hash of the 
        /// input string.
        /// </returns>
        private static uint Fnv1a(string text)
        {
            unchecked
            {
                uint hash = 2166136261;
                for (int i = 0; i < text.Length; i++)
                {
                    hash = (hash ^ text[i]) * 16777619;
                }

                return hash;
            }
        }
    }
}
