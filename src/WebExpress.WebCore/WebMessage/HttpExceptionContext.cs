using Microsoft.AspNetCore.Http.Features;
using System;
using System.Net;

namespace WebExpress.WebCore.WebMessage
{
    public class HttpExceptionContext : HttpContext
    {
        /// <summary>
        /// Returns or sets an error message if the context could not be created.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="exception">An exception that prevented the creation of the context.</param>
        /// <param name="contextFeatures">Initial set of features.</param>
        public HttpExceptionContext(Exception exception, IFeatureCollection contextFeatures)
        {
            var connectionFeature = contextFeatures.Get<IHttpConnectionFeature>();
            var requestFeature = contextFeatures.Get<IHttpRequestFeature>();

            Features = contextFeatures;
            Id = connectionFeature.ConnectionId;

            LocalEndPoint = new IPEndPoint(connectionFeature.LocalIpAddress, connectionFeature.LocalPort);
            RemoteEndPoint = new IPEndPoint(connectionFeature.RemoteIpAddress, connectionFeature.RemotePort);

            Exception = exception;
        }
    }
}
