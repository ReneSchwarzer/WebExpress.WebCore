using Microsoft.AspNetCore.Http.Features;
using System;
using System.Net;
using System.Text;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents the context of an HTTP request and response.
    /// </summary>
    public interface IHttpContext
    {
        /// <summary>
        /// Gets the context of the web server.
        /// </summary>
        IHttpServerContext HttpServerContext { get; }

        /// <summary>
        /// Gets the context id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the request data.
        /// </summary>
        IRequest Request { get; }

        /// <summary>
        /// Gets the ip address and port number of the server that receives the request.
        /// </summary>
        EndPoint LocalEndPoint { get; }

        /// <summary>
        /// Gets the ip address and port number of the client where the request originated.
        /// </summary>
        EndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Gets the set of features.
        /// </summary>
        IFeatureCollection Features { get; }

        /// <summary>
        /// Gets the encoding used.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Gets the URI.
        /// </summary>
        Uri Uri { get; }
    }
}