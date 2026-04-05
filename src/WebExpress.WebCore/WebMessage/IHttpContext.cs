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
        /// Returns the context of the web server.
        /// </summary>
        IHttpServerContext HttpServerContext { get; }

        /// <summary>
        /// Returns the context id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the request data.
        /// </summary>
        IRequest Request { get; }

        /// <summary>
        /// Returns the ip address and port number of the server that receives the request.
        /// </summary>
        EndPoint LocalEndPoint { get; }

        /// <summary>
        /// Returns the ip address and port number of the client where the request originated.
        /// </summary>
        EndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Returns the set of features.
        /// </summary>
        IFeatureCollection Features { get; }

        /// <summary>
        /// Returns the encoding used.
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Returns the URI.
        /// </summary>
        Uri Uri { get; }
    }
}