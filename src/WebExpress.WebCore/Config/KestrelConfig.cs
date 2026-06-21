using System.Xml.Serialization;

namespace WebExpress.WebCore.Config
{
    /// <summary>
    /// Optional fine-tuning of the underlying Kestrel server. The whole &lt;kestrel&gt; element as well
    /// as every individual property is optional: any value that is left unset (<c>null</c>) keeps the
    /// behavior the web server applied before this configuration block existed, so adding the element
    /// can never change defaults a deployment did not explicitly opt into.
    /// </summary>
    [XmlRoot("kestrel", IsNullable = false)]
    public sealed class KestrelConfig
    {
        /// <summary>
        /// Allows synchronous IO on the request and response streams. WebExpress renders and sends
        /// responses synchronously, which is why this defaults to <c>true</c> when not specified.
        /// </summary>
        [XmlElement("allowsynchronousio")]
        public bool? AllowSynchronousIO { get; set; }

        /// <summary>
        /// Allows the response headers to be compressed. Defaults to <c>true</c> when not specified.
        /// </summary>
        [XmlElement("allowresponseheadercompression")]
        public bool? AllowResponseHeaderCompression { get; set; }

        /// <summary>
        /// Controls whether the <c>Server</c> response header is emitted. Disabling it reduces the
        /// information exposed about the host. Defaults to <c>true</c> when not specified.
        /// </summary>
        [XmlElement("addserverheader")]
        public bool? AddServerHeader { get; set; }

        /// <summary>
        /// The maximum number of concurrent client connections. When not specified the Kestrel
        /// default (unlimited) is kept.
        /// </summary>
        [XmlElement("maxconcurrentconnections")]
        public long? MaxConcurrentConnections { get; set; }

        /// <summary>
        /// The maximum allowed size of a request body, in bytes. When not specified the Kestrel
        /// default is kept.
        /// </summary>
        [XmlElement("maxrequestbodysize")]
        public long? MaxRequestBodySize { get; set; }

        /// <summary>
        /// The maximum allowed size of the combined request headers, in bytes. When not specified
        /// the Kestrel default (32 KiB) is kept.
        /// </summary>
        [XmlElement("maxrequestheaderstotalsize")]
        public int? MaxRequestHeadersTotalSize { get; set; }

        /// <summary>
        /// The maximum number of open, upgraded connections (e.g. WebSockets). Upgraded connections
        /// are not counted against the regular connection limit. When not specified the Kestrel
        /// default is kept.
        /// </summary>
        [XmlElement("maxconcurrentupgradedconnections")]
        public long? MaxConcurrentUpgradedConnections { get; set; }

        /// <summary>
        /// The maximum size of the request buffer, in bytes. When not specified the Kestrel default is kept.
        /// </summary>
        [XmlElement("maxrequestbuffersize")]
        public long? MaxRequestBufferSize { get; set; }

        /// <summary>
        /// The maximum size of the response buffer, in bytes. When not specified the Kestrel default is kept.
        /// </summary>
        [XmlElement("maxresponsebuffersize")]
        public long? MaxResponseBufferSize { get; set; }

        /// <summary>
        /// The maximum allowed size of the request line (request method, uri and protocol), in bytes.
        /// When not specified the Kestrel default is kept.
        /// </summary>
        [XmlElement("maxrequestlinesize")]
        public int? MaxRequestLineSize { get; set; }

        /// <summary>
        /// The keep-alive timeout, in seconds. A value that closes idle connections after a period of
        /// inactivity. When not specified the Kestrel default is kept.
        /// </summary>
        [XmlElement("keepalivetimeout")]
        public int? KeepAliveTimeout { get; set; }

        /// <summary>
        /// The amount of time, in seconds, the server waits for the request headers to be received in
        /// full before closing the connection. When not specified the Kestrel default is kept.
        /// </summary>
        [XmlElement("requestheaderstimeout")]
        public int? RequestHeadersTimeout { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public KestrelConfig()
        {
        }
    }
}
