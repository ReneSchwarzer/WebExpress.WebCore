using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using System.Net;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.Test.Message
{
    public class UnitTestRequest
    {
        /// <summary>
        /// Creates the feature collection.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="method">The request method.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns>The feature collection.</returns>
        protected static IFeatureCollection Prepare(BinaryReader reader, RequestMethod method, string queryString)
        {
            var contextFeatures = new FeatureCollection();
            contextFeatures.Set<IHttpConnectionFeature>(new HttpConnectionFeature()
            {
                LocalPort = 80,
                LocalIpAddress = IPAddress.Loopback,
                RemoteIpAddress = IPAddress.Loopback,
                ConnectionId = "43D885B6-DB4D-4EDF-9908-B122A5FFC829"
            });
            contextFeatures.Set<IHttpRequestFeature>(new HttpRequestFeature()
            {
                Path = "/",
                Protocol = "HTTP/1.1",
                Method = method.ToString(),
                QueryString = queryString,
                Headers = new HeaderDictionary()
            });
            contextFeatures.Set<IHttpRequestIdentifierFeature>(new HttpRequestIdentifierFeature() { TraceIdentifier = "16FC666F-D7DC-47FF-9FB8-9D0F8DFCEF99" });
            contextFeatures.Set<ISessionFeature>(new SessionFeature() { });

            var requestFeature = contextFeatures.Get<IHttpRequestFeature>();
            requestFeature.Headers.Host = "localhost";
            requestFeature.Headers.Connection = "keep-alive";
            requestFeature.Headers.ContentType = "";
            requestFeature.Headers.ContentLength = 0;
            requestFeature.Headers.ContentLanguage = "";
            requestFeature.Headers.ContentEncoding = "utf-8";
            requestFeature.Headers.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            requestFeature.Headers.AcceptEncoding = "gzip, deflate, br";
            requestFeature.Headers.AcceptLanguage = "de,en;q=0.9,en-GB;q=0.8,de-DE;q=0.7,en-US;q=0.6";
            requestFeature.Headers.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36 Edg/98.0.1108.50";

            //requestFeature.Headers.Cookie;

            requestFeature.Headers.Authorization = "";

            return contextFeatures;
        }
    }
}
