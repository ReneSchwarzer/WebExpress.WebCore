using Microsoft.AspNetCore.Http.Features;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebParameter;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// See RFC 2616, The Request class encapsulates and extends the 
    /// original request of the HttpListener call.
    /// </summary>
    public partial class Request : RequestBase
    {
        [GeneratedRegex(@"([\w-]+)=(.*)")]
        private static partial Regex TextRegex();

        [GeneratedRegex(@"Content-Type:\s*(.*)", RegexOptions.IgnoreCase, "de-DE")]
        private static partial Regex ContentRegex();

        /// <summary>
        /// Gets the content.
        /// </summary>
        public byte[] Content { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="contextFeatures">Initial set of features.</param>
        /// <param name="header">The header.</param>
        /// <param name="httpServerContext">The context of the web server.</param>
        internal Request(IFeatureCollection contextFeatures, RequestHeaderFields header, IHttpServerContext httpServerContext)
            : base(contextFeatures, header, httpServerContext)
        {
            var requestFeature = contextFeatures.Get<IHttpRequestFeature>();

            Content = GetContent(requestFeature.Body, Header.ContentLength);

            ParseRequestParams();
        }

        /// <summary>
        /// Returns the content.
        /// </summary>
        /// <param name="body">The content of a request.</param>
        /// <param name="contentLength">The number of bytes sent in the body or zero.</param>
        /// <returns>Der Content als Byte-Array</returns>
        internal static byte[] GetContent(Stream body, long? contentLength)
        {
            if (!contentLength.HasValue || contentLength.Value == 0)
            {
                return null;
            }

            using var ms = new MemoryStream();
            body.CopyTo(ms);

            return ms.ToArray();
        }

        /// <summary>
        /// Parse the request parameters.
        /// </summary>
        protected virtual void ParseRequestParams()
        {
            if (string.IsNullOrWhiteSpace(Header.ContentType) || Content is null || Content.Length == 0)
            {
                return;
            }

            // normalize content-type
            var ct = Header.ContentType.Split(';')
                .Select(x => x.Trim())
                .ToArray();

            var mainType = ct.FirstOrDefault()?.ToLowerInvariant();
            var enctype = TypeEnctypeExtensions.Convert(mainType);

            // detect multipart/form-data even if Convert() fails
            if (mainType.StartsWith("multipart/form-data"))
            {
                enctype = TypeEnctype.Multipart;
            }

            switch (enctype)
            {
                case TypeEnctype.Multipart:
                    ParseMultipart(ct);
                    break;

                case TypeEnctype.Text:
                    ParseTextPlain();
                    break;

                case TypeEnctype.UrLEncoded:
                    ParseUrlEncoded();
                    break;

                default:
                    // unknown or unsupported content-type
                    break;
            }
        }

        /// <summary>
        /// Parses multipart form data from the provided content type parts and extracts parameters and 
        /// file uploads.
        /// </summary>
        /// <param name="contentTypeParts">
        /// An array of strings representing the parts of the Content-Type header. Each part may include 
        /// information such as the boundary used to separate multipart sections.
        /// </param>
        private void ParseMultipart(string[] contentTypeParts)
        {
            // extract boundary
            var boundary = contentTypeParts
                .FirstOrDefault(x => x.StartsWith("boundary=", StringComparison.OrdinalIgnoreCase))
                ?["boundary=".Length..];

            if (string.IsNullOrWhiteSpace(boundary))
            {
                return;
            }

            var boundaryBytes = Encoding.UTF8.GetBytes("--" + boundary);
            var endBoundaryBytes = Encoding.UTF8.GetBytes("--" + boundary + "--");

            int pos = 0;

            while (true)
            {
                // find next boundary
                int start = IndexOf(Content, boundaryBytes, pos);
                if (start < 0)
                {
                    break;
                }

                // check for end boundary
                bool isFinal = StartsWith(Content, endBoundaryBytes, start);

                // move to header start
                int headerStart = start + boundaryBytes.Length + GetLineBreakLength(Content, start + boundaryBytes.Length);

                // find header end (empty line)
                int headerSeparatorLength;
                int headerEnd = FindHeaderEnd(Content, headerStart, out headerSeparatorLength);
                if (headerEnd < 0)
                {
                    break;
                }

                var headerText = Encoding.UTF8.GetString(Content, headerStart, headerEnd - headerStart);

                // parse headers
                var name = ExtractHeaderValue(headerText, "name");
                var filename = ExtractHeaderValue(headerText, "filename");
                var contentType = ExtractContentType(headerText);

                // content start
                int dataStart = headerEnd + headerSeparatorLength;

                // find next boundary to determine data length
                int nextBoundary = IndexOf(Content, boundaryBytes, dataStart);
                if (nextBoundary < 0)
                {
                    break;
                }

                int dataLength = nextBoundary - dataStart;
                dataLength -= GetTrailingLineBreakLength(Content, nextBoundary);
                if (dataLength < 0)
                {
                    // Defensive fallback for malformed parts where boundary follows unexpectedly early.
                    dataLength = 0;
                }

                if (string.IsNullOrEmpty(filename))
                {
                    // normal field
                    var value = Encoding.UTF8.GetString(Content, dataStart, dataLength).TrimEnd();
                    AddParameter(new Parameter(name, value, ParameterScope.Parameter));
                }
                else
                {
                    // file upload
                    var bytes = new byte[dataLength];
                    Buffer.BlockCopy(Content, dataStart, bytes, 0, dataLength);

                    AddParameter(new ParameterFile(name, filename, ParameterScope.Parameter)
                    {
                        ContentType = contentType,
                        Data = bytes
                    });
                }

                if (isFinal)
                {
                    break;
                }

                pos = nextBoundary;
            }
        }

        /// <summary>
        /// Parses the request content as plain text and extracts parameters from lines in 
        /// the format 'key=value'.
        /// </summary>
        private void ParseTextPlain()
        {
            var text = Encoding.UTF8.GetString(Content);
            var lines = text.Split('\n');

            Parameter last = null;

            foreach (var line in lines)
            {
                var trimmed = line.TrimEnd('\r');

                var match = TextRegex().Match(trimmed);
                if (match.Success)
                {
                    last = new Parameter(match.Groups[1].Value, match.Groups[2].Value, ParameterScope.Parameter);
                    AddParameter(last);
                }
                else
                {
                    last?.Value += "\r\n" + trimmed;
                }
            }

            last?.Value = last.Value.TrimEnd();
        }

        /// <summary>
        /// Parses the request content as a URL-encoded form and adds each key-value pair 
        /// as a parameter.
        /// </summary>
        private void ParseUrlEncoded()
        {
            var text = Encoding.UTF8.GetString(Content);
            foreach (var pair in text.Split('&'))
            {
                var parts = pair.Split('=');
                var key = parts[0];
                var value = parts.Length > 1 ? parts[1].Replace('+', ' ') : string.Empty;

                AddParameter(new Parameter(key, value, ParameterScope.Parameter));
            }
        }

        /// <summary>
        /// Searches for the first occurrence of a specified byte sequence within a byte array, 
        /// starting at a given index.
        /// </summary>
        /// <remarks>
        /// The search is performed using ordinal byte comparison. If needle is an empty array,
        /// the method returns start. If start is greater than haystack.Length - needle.Length, 
        /// the method returns -1.
        /// </remarks>
        /// <param name="haystack">
        /// The byte array to search within.
        /// </param>
        /// <param name="needle">
        /// The byte sequence to locate within the haystack array.
        /// </param>
        /// <param name="start">
        /// The zero-based index in the haystack array at which to begin searching. Must be 
        /// non-negative and less than or equal to haystack.Length.
        /// </param>
        /// <returns>
        /// The zero-based index of the first occurrence of needle within haystack, starting at 
        /// the specified index; or -1 if the sequence is not found.
        /// </returns>
        private static int IndexOf(byte[] haystack, byte[] needle, int start)
        {
            for (int i = start; i <= haystack.Length - needle.Length; i++)
            {
                if (StartsWith(haystack, needle, i))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Finds the end of multipart headers and returns the separator length.
        /// Supports both CRLF and LF line endings.
        /// </summary>
        private static int FindHeaderEnd(byte[] content, int headerStart, out int separatorLength)
        {
            var crlfSeparator = Encoding.UTF8.GetBytes("\r\n\r\n");
            var lfSeparator = Encoding.UTF8.GetBytes("\n\n");

            var crlfEnd = IndexOf(content, crlfSeparator, headerStart);
            var lfEnd = IndexOf(content, lfSeparator, headerStart);

            if (crlfEnd >= 0 && (lfEnd < 0 || crlfEnd <= lfEnd))
            {
                separatorLength = crlfSeparator.Length;
                return crlfEnd;
            }

            if (lfEnd >= 0)
            {
                separatorLength = lfSeparator.Length;
                return lfEnd;
            }

            separatorLength = 0;
            return -1;
        }

        /// <summary>
        /// Returns the line break length at the specified offset.
        /// Supports CRLF and LF.
        /// </summary>
        private static int GetLineBreakLength(byte[] content, int offset)
        {
            if (offset + 1 < content.Length && content[offset] == (byte)'\r' && content[offset + 1] == (byte)'\n')
            {
                return 2;
            }

            if (offset < content.Length && content[offset] == (byte)'\n')
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Returns the line break length directly before the specified offset.
        /// Supports CRLF and LF.
        /// </summary>
        private static int GetTrailingLineBreakLength(byte[] content, int offset)
        {
            if (offset >= 2 && content[offset - 2] == (byte)'\r' && content[offset - 1] == (byte)'\n')
            {
                return 2;
            }

            if (offset >= 1 && content[offset - 1] == (byte)'\n')
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Determines whether a specified segment of a byte array begins with the given prefix.
        /// </summary>
        /// <remarks>
        /// If <paramref name="offset"/> plus the length of <paramref name="prefix"/> exceeds the
        /// length of <paramref name="data"/>, the method returns <see langword="false"/>.
        /// </remarks>
        /// <param name="data">
        /// The byte array to examine.
        /// </param>
        /// <param name="prefix">
        /// The byte sequence to compare against the segment of <paramref name="data"/>.
        /// </param>
        /// <param name="offset">
        /// The zero-based index in <paramref name="data"/> at which to begin the comparison.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the segment of <paramref name="data"/> starting at 
        /// <paramref name="offset"/> begins with <paramref name="prefix"/>; otherwise, 
        /// <see langword="false"/>.
        /// </returns>
        private static bool StartsWith(byte[] data, byte[] prefix, int offset)
        {
            if (offset + prefix.Length > data.Length)
            {
                return false;
            }

            for (int i = 0; i < prefix.Length; i++)
            {
                if (data[offset + i] != prefix[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Extracts the value associated with the specified key from a header string formatted 
        /// as key-value pairs.
        /// </summary>
        /// <param name="header">
        /// The header string containing key-value pairs, where values are enclosed in double quotes.
        /// </param>
        /// <param name="key">
        /// The key whose associated value is to be extracted from the header. The search is case-insensitive.
        /// </param>
        /// <returns>
        /// The value associated with the specified key if found; otherwise, an empty string.
        /// </returns>
        private static string ExtractHeaderValue(string header, string key)
        {
            var match = Regex.Match(header, key + "=\"([^\"]*)\"", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        /// <summary>
        /// Extracts the value of the Content-Type header from the specified header string.
        /// </summary>
        /// <param name="header">
        /// The header string from which to extract the Content-Type value. This should contain a 
        /// line starting with 'Content-Type:'.
        /// </param>
        /// <returns>
        /// A string containing the value of the Content-Type header if found; otherwise, 
        /// an empty string.
        /// </returns>
        private static string ExtractContentType(string header)
        {
            var match = ContentRegex().Match(header);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }
    }
}
