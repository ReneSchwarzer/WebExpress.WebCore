using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using WebExpress.WebCore.Config;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebLog;

namespace WebExpress.WebCore
{
    /// <summary>
    /// Default implementation of <see cref="IHttpServerContext"/>. It bundles the server-wide
    /// information (routing, endpoints, version, directories, culture, log) that is created once
    /// at start-up and handed to plugins and components throughout the server's lifetime.
    /// </summary>
    public class HttpServerContext : IHttpServerContext
    {
        /// <summary>
        /// Gets the route of the web server.
        /// </summary>
        public IRoute Route { get; protected set; }

        /// <summary>
        /// Gets the endpoints to which the web server responds.
        /// </summary>
        public ICollection<EndpointConfig> Endpoints { get; protected set; }

        /// <summary>
        /// Gets the version of the http(s) server.
        /// </summary>
        public string Version { get; protected set; }

        /// <summary>
        /// Gets the package home directory.
        /// </summary>
        public string PackagePath { get; protected set; }

        /// <summary>
        /// Gets the asset home directory.
        /// </summary>
        public string AssetPath { get; protected set; }

        /// <summary>
        /// Gets the data home directory.
        /// </summary>
        public string DataPath { get; protected set; }

        /// <summary>
        /// Gets the configuration directory.
        /// </summary>
        public string ConfigPath { get; protected set; }

        /// <summary>
        /// Gets the culture.
        /// </summary>
        public CultureInfo Culture { get; protected set; }

        /// <summary>
        /// Gets the log for writing status messages to the console and to a log file.
        /// </summary>
        public ILog Log { get; protected set; }

        /// <summary>
        /// Gets the host.
        /// </summary>
        public IHost Host { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="route">The uri of the route server.</param>
        /// <param name="endpoints">The endpoints to which the web server responds.</param>
        /// <param name="packageBaseFolder">The package home directory.chnis</param>
        /// <param name="assetBaseFolder">The asset home directory.</param>
        /// <param name="dataBaseFolder">The data home directory.</param>
        /// <param name="configBaseFolder">The configuration directory.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="log">The log.</param>
        /// <param name="host">The host.</param>
        public HttpServerContext
        (
            IRoute route,
            ICollection<EndpointConfig> endpoints,
            string packageBaseFolder,
            string assetBaseFolder,
            string dataBaseFolder,
            string configBaseFolder,
            CultureInfo culture,
            ILog log,
            IHost host
        )
        {
            var assembly = typeof(HttpServer).Assembly;
            Version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            Route = route;
            Endpoints = endpoints;
            PackagePath = packageBaseFolder;
            AssetPath = assetBaseFolder;
            DataPath = dataBaseFolder;
            ConfigPath = configBaseFolder;
            Culture = culture;
            Log = log;
            Host = host;
        }
    }
}
