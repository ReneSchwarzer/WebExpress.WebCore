using System.Collections.Generic;
using System.Globalization;
using WebExpress.Config;
using WebExpress.WebUri;

namespace WebExpress
{
    /// <summary>
    /// The context interface of the http server.
    /// </summary>
    public interface IHttpServerContext
    {
        /// <summary>
        /// Returns the uri of the web server.
        /// </summary>
        string Uri { get; }

        /// <summary>
        /// Returns the endpoints to which the web server responds.
        /// </summary>
        ICollection<EndpointConfig> Endpoints { get; }

        /// <summary>
        /// Returns the version of the http(s) server.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Returns the package home directory.
        /// </summary>
        string PackagePath { get; }

        /// <summary>
        /// Returns the asset home directory.
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// Returns the data home directory.
        /// </summary>
        string DataPath { get; }

        /// <summary>
        /// Returns the configuration directory.
        /// </summary>
        string ConfigPath { get; }

        /// <summary>
        /// Returns the basic context path.
        /// </summary>
        UriResource ContextPath { get; }

        /// <summary>
        /// Returns the culture.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        /// Returns the log for writing status messages to the console and to a log file.
        /// </summary>
        Log Log { get; }

        /// <summary>
        /// Returns the host.
        /// </summary>
        IHost Host { get; }
    }
}
