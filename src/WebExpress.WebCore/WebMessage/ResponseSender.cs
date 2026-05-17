using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebExpress.WebCore.WebHtml;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Provides functionality for sending HTTP responses.
    /// </summary>
    public class ResponseSender
    {
        /// <summary>
        /// Sends the specified response message asynchronously to the provided context.
        /// </summary>
        /// <param name="context">The context of the request.</param>
        /// <param name="response">The reply message.</param>
        /// <param name="keepAlive">Indicates whether the connection should be kept alive after sending the response.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendAsync(IHttpContext context, IResponse response, bool keepAlive = false)
        {
            try
            {
                var responseFeature = context.Features.Get<IHttpResponseFeature>();
                var responseBodyFeature = context.Features.Get<IHttpResponseBodyFeature>();

                if (responseFeature is null)
                {
                    // write error to server log
                    var log = WebEx.ComponentHub.LogManager.DefaultLog;
                    log.Error(context.RemoteEndPoint + ": The HTTP response feature is not available in the current context.");

                    return;
                }

                responseFeature.StatusCode = response.Status;
                responseFeature.ReasonPhrase = response.Reason;
                responseFeature.Headers.KeepAlive = "true";

                if (response.Header.Location is not null)
                {
                    responseFeature.Headers.Location = response.Header.Location;
                }

                if (!string.IsNullOrWhiteSpace(response.Header.CacheControl))
                {
                    responseFeature.Headers.CacheControl = response.Header.CacheControl;
                }

                if (!string.IsNullOrWhiteSpace(response.Header.ContentType))
                {
                    responseFeature.Headers.ContentType = response.Header.ContentType;
                }

                if (response.Header.WWWAuthenticate)
                {
                    responseFeature.Headers.WWWAuthenticate = "Basic realm=\"Bereich\"";
                }

                if (response.Header.Cookies.Count != 0)
                {
                    // Cookie.ToString() only emits "name=value" and discards
                    // Path / Expires / Domain / SameSite - which makes the
                    // browser default-path the cookie to the request URI's
                    // directory. Build a proper Set-Cookie header per cookie
                    // so attributes survive the round trip.
                    var headerValues = response.Header.Cookies
                        .Cast<Cookie>()
                        .Select(SerializeSetCookie)
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToArray();
                    if (headerValues.Length > 0)
                    {
                        responseFeature.Headers.SetCookie = new StringValues(headerValues);
                    }
                }

                if (!string.IsNullOrWhiteSpace(response.Header.Upgrade))
                {
                    responseFeature.Headers.Upgrade = response.Header.Upgrade;
                }

                if (!string.IsNullOrWhiteSpace(response.Header.Connection))
                {
                    responseFeature.Headers.Connection = response.Header.Connection;
                }

                if (!string.IsNullOrWhiteSpace(response.Header.SecWebSocketAccept))
                {
                    responseFeature.Headers.SecWebSocketAccept = response.Header.SecWebSocketAccept;
                }

                if (response?.Content is byte[] byteContent)
                {
                    responseFeature.Headers.ContentLength = byteContent.Length;
                    await responseBodyFeature.Stream.WriteAsync(byteContent);
                    await responseBodyFeature.Stream.FlushAsync();
                }
                else if (response?.Content is string strContent)
                {
                    var content = context.Encoding.GetBytes(strContent);

                    responseFeature.Headers.ContentLength = content.Length;
                    await responseBodyFeature.Stream.WriteAsync(content);
                    await responseBodyFeature.Stream.FlushAsync();
                }
                else if (response?.Content is IHtmlNode htmlContent)
                {
                    var content = context.Encoding.GetBytes(htmlContent?.ToString());

                    responseFeature.Headers.ContentLength = content.Length;
                    await responseBodyFeature.Stream.WriteAsync(content);
                    await responseBodyFeature.Stream.FlushAsync();
                }

                if (!keepAlive)
                {
                    responseBodyFeature.Stream.Close();
                }
            }
            catch (Exception ex)
            {
                // write error to server log
                var log = WebEx.ComponentHub.LogManager.DefaultLog;
                log.Error(context.RemoteEndPoint + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Serialises a <see cref="Cookie"/> as a single Set-Cookie header
        /// value preserving Path, Domain, Expires and the HttpOnly / Secure
        /// flags. SameSite defaults to <c>Lax</c> because System.Net.Cookie
        /// does not model the attribute directly.
        /// </summary>
        /// <param name="cookie">The cookie to serialise.</param>
        /// <returns>A Set-Cookie header value, or null when the cookie is empty.</returns>
        private static string SerializeSetCookie(Cookie cookie)
        {
            if (cookie is null || string.IsNullOrEmpty(cookie.Name))
            {
                return null;
            }

            var parts = new List<string>
            {
                $"{cookie.Name}={cookie.Value ?? string.Empty}"
            };

            if (!string.IsNullOrEmpty(cookie.Path))
            {
                parts.Add($"Path={cookie.Path}");
            }
            if (!string.IsNullOrEmpty(cookie.Domain))
            {
                parts.Add($"Domain={cookie.Domain}");
            }
            if (cookie.Expires != DateTime.MinValue)
            {
                // RFC 7231 IMF-fixdate: "Wed, 21 Oct 2015 07:28:00 GMT".
                parts.Add($"Expires={cookie.Expires.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture)}");
            }
            if (cookie.HttpOnly)
            {
                parts.Add("HttpOnly");
            }
            if (cookie.Secure)
            {
                parts.Add("Secure");
            }

            // System.Net.Cookie has no SameSite property; default to Lax for
            // first-party fit-for-purpose behaviour without cross-site leaks.
            parts.Add("SameSite=Lax");

            return string.Join("; ", parts);
        }
    }
}