using System.Collections.Generic;
using System.Globalization;
using WebExpress.WebCore.Config;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebLog;

namespace WebExpress.WebCore
{
    /// <summary>
    /// Provides the server-wide information that plugins and components need to do their work:
    /// the routing entry point, the configured endpoints, the server version, the well-known
    /// directories (packages, assets, data, configuration), the culture, and the central log.
    /// A single instance is shared for the lifetime of the running server.
    /// </summary>
    public interface IHttpServerContext
    {
        /// <summary>
        /// Gets the route of the web server.
        /// </summary>
        IRoute Route { get; }

        /// <summary>
        /// Gets the endpoints to which the web server responds.
        /// </summary>
        ICollection<EndpointConfig> Endpoints { get; }

        /// <summary>
        /// Gets the version of the http(s) server.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the package home directory.
        /// </summary>
        string PackagePath { get; }

        /// <summary>
        /// Gets the asset home directory.
        /// </summary>
        string AssetPath { get; }

        /// <summary>
        /// Gets the data home directory.
        /// </summary>
        string DataPath { get; }

        /// <summary>
        /// Gets the configuration directory.
        /// </summary>
        string ConfigPath { get; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        /// Gets the log for writing status messages to the console and to a log file.
        /// </summary>
        ILog Log { get; }

        /// <summary>
        /// Gets the host.
        /// </summary>
        IHost Host { get; }
    }
}
