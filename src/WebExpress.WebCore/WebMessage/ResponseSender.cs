using Microsoft.AspNetCore.Http.Features;
using System;
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
        public async Task SendAsync(HttpContext context, IResponse response, bool keepAlive = false)
        {
            try
            {
                var responseFeature = context.Features.Get<IHttpResponseFeature>();
                var responseBodyFeature = context.Features.Get<IHttpResponseBodyFeature>();

                responseFeature.StatusCode = response.Status;
                responseFeature.ReasonPhrase = response.Reason;
                responseFeature.Headers.KeepAlive = "true";

                if (response.Header.Location != null)
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
                    responseFeature.Headers.SetCookie = string.Join(" ", response.Header.Cookies);
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
    }
}