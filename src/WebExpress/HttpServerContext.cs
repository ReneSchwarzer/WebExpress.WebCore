using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using WebExpress.Config;
using WebExpress.WebUri;

namespace WebExpress
{
    /// <summary>
    /// The context of the http server.
    /// </summary>
    public class HttpServerContext : IHttpServerContext
    {
        /// <summary>
        /// Returns the uri of the web server.
        /// </summary>
        public string Uri { get; protected set; }

        /// <summary>
        /// Returns the endpoints to which the web server responds.
        /// </summary>
        public ICollection<EndpointConfig> Endpoints { get; protected set; }

        /// <summary>
        /// Returns the version of the http(s) server.
        /// </summary>
        public string Version { get; protected set; }

        /// <summary>
        /// Returns the package home directory.
        /// </summary>
        public string PackagePath { get; protected set; }

        /// <summary>
        /// Returns the asset home directory.
        /// </summary>
        public string AssetPath { get; protected set; }

        /// <summary>
        /// Returns the data home directory.
        /// </summary>
        public string DataPath { get; protected set; }

        /// <summary>
        /// Returns the configuration directory.
        /// </summary>
        public string ConfigPath { get; protected set; }

        /// <summary>
        /// Returns the basic context path.
        /// </summary>
        public UriResource ContextPath { get; protected set; }

        /// <summary>
        /// Returns the culture.
        /// </summary>
        public CultureInfo Culture { get; protected set; }

        /// <summary>
        /// Returns the log for writing status messages to the console and to a log file.
        /// </summary>
        public Log Log { get; protected set; }

        /// <summary>
        /// Returns the host.
        /// </summary>
        public IHost Host { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri">The uri of the web server.</param>
        /// <param name="endpoints">The endpoints to which the web server responds.</param>
        /// <param name="packageBaseFolder">The package home directory.chnis</param>
        /// <param name="assetBaseFolder">The asset home directory.</param>
        /// <param name="dataBaseFolder">The data home directory.</param>
        /// <param name="configBaseFolder">The configuration directory.</param>
        /// <param name="contextPath">The basic context path.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="log">The log.</param>
        /// <param name="host">The host.</param>
        public HttpServerContext
        (
            string uri,
            ICollection<EndpointConfig> endpoints,
            string packageBaseFolder,
            string assetBaseFolder,
            string dataBaseFolder,
            string configBaseFolder,
            UriResource contextPath,
            CultureInfo culture,
            Log log,
            IHost host
        )
        {
            var assembly = typeof(HttpServer).Assembly;
            Version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            Uri = uri;
            Endpoints = endpoints;
            PackagePath = packageBaseFolder;
            AssetPath = assetBaseFolder;
            DataPath = dataBaseFolder;
            ConfigPath = configBaseFolder;
            ContextPath = contextPath;
            Culture = culture;
            Log = log;
            Host = host;
        }
    }
}
