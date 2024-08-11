using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// see RFC 2616
    /// </summary>
    public class RequestHeaderFields
    {
        /// <summary>
        /// Returns the host.
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Returns the connection. Keep-Alive or close.
        /// </summary>
        public string Connection { get; private set; }

        /// <summary>
        /// Returns the content length.
        /// </summary>
        public long ContentLength { get; private set; }

        /// <summary>
        /// Returns the content type.
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Returns the language of the content.
        /// </summary>
        public string ContentLanguage { get; private set; }

        /// <summary>
        /// Returns the encoding of the content.
        /// </summary>
        public Encoding ContentEncoding { get; private set; }

        /// <summary>
        /// Returns the user agent.
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// Returns the accepted media types.
        /// </summary>
        public ICollection<string> Accept { get; private set; }

        /// <summary>
        /// Returns the accepted encodings.
        /// </summary>
        public string AcceptEncoding { get; private set; }

        /// <summary>
        /// Returns the accepted languages.
        /// </summary>
        public IEnumerable<string> AcceptLanguage { get; private set; }

        /// <summary>
        /// Returns the access data name and password.
        /// </summary>
        public RequestAuthorization Authorization { get; private set; }

        /// <summary>
        /// Returns the cookies.
        /// </summary>
        public ICollection<Cookie> Cookies { get; } = new List<Cookie>();

        /// <summary>
        /// Returns the referer. The referer header echoes the absolute or partial address from 
        /// which a resource was requested. The Referer header allows a server to identify referring 
        /// pages from which people visit or where requested resources are used.
        /// </summary>
        public string Referer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="contextFeatures">Initial set of features.</param>
        internal RequestHeaderFields(IFeatureCollection contextFeatures)
        {
            var requestFeature = contextFeatures.Get<IHttpRequestFeature>();

            Host = requestFeature.Headers.Host;
            Connection = requestFeature.Headers.Connection;
            ContentType = requestFeature.Headers.ContentType;
            ContentLength = requestFeature.Headers.ContentLength ?? 0;
            ContentLanguage = requestFeature.Headers.ContentLanguage;
            ContentEncoding = requestFeature.Headers.ContentEncoding.Any() ? Encoding.GetEncoding(requestFeature.Headers.ContentEncoding) : Encoding.Default;
            Accept = requestFeature.Headers.Accept;
            AcceptEncoding = requestFeature.Headers.AcceptEncoding;
            AcceptLanguage = requestFeature.Headers.AcceptLanguage.SelectMany(x => x.Split(';', StringSplitOptions.RemoveEmptyEntries));
            UserAgent = requestFeature.Headers.UserAgent;
            Referer = requestFeature.Headers.Referer;

            foreach (var cookie in requestFeature.Headers.Cookie)
            {
                var split = cookie.Split('=');
                var key = split[0];
                var value = split[1];

                Cookies.Add(new Cookie(key, value));
            }

            Authorization = RequestAuthorization.Parse(requestFeature.Headers.Authorization);
        }
    }
}
