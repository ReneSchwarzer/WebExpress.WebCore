using System.IO;
using System.Xml.Serialization;
using WebExpress.WebCore.Config;

namespace WebExpress.WebCore.Test.Config
{
    /// <summary>
    /// Unit tests for the optional Kestrel configuration block and its deserialization,
    /// mirroring how the server loads its configuration via XmlSerializer.
    /// </summary>
    public class UnitTestKestrelConfig
    {
        /// <summary>
        /// Deserializes the given configuration document into an HttpServerConfig instance.
        /// </summary>
        /// <param name="xml">The configuration document.</param>
        /// <returns>The deserialized configuration.</returns>
        private static HttpServerConfig Deserialize(string xml)
        {
            var serializer = new XmlSerializer(typeof(HttpServerConfig));
            using var reader = new StringReader(xml);

            return serializer.Deserialize(reader) as HttpServerConfig;
        }

        /// <summary>
        /// Tests that a configuration without a kestrel block leaves the property null so the
        /// server keeps its built-in defaults.
        /// </summary>
        [Fact]
        public void MissingBlockIsNull()
        {
            // arrange
            var xml = "<config version=\"1\"><endpoint uri=\"http://localhost/\" /></config>";

            // act
            var config = Deserialize(xml);

            // validation
            Assert.Null(config.Kestrel);
        }

        /// <summary>
        /// Tests that all kestrel settings are read when present.
        /// </summary>
        [Fact]
        public void FullBlockIsDeserialized()
        {
            // arrange
            var xml =
                "<config version=\"1\">" +
                "  <endpoint uri=\"http://localhost/\" />" +
                "  <kestrel>" +
                "    <maxconcurrentconnections>300</maxconcurrentconnections>" +
                "    <maxrequestbodysize>3000000000</maxrequestbodysize>" +
                "    <maxrequestheaderstotalsize>65536</maxrequestheaderstotalsize>" +
                "    <allowsynchronousio>false</allowsynchronousio>" +
                "    <allowresponseheadercompression>false</allowresponseheadercompression>" +
                "    <addserverheader>false</addserverheader>" +
                "    <maxconcurrentupgradedconnections>1000</maxconcurrentupgradedconnections>" +
                "    <maxrequestbuffersize>2097152</maxrequestbuffersize>" +
                "    <maxresponsebuffersize>131072</maxresponsebuffersize>" +
                "    <maxrequestlinesize>16384</maxrequestlinesize>" +
                "    <keepalivetimeout>90</keepalivetimeout>" +
                "    <requestheaderstimeout>15</requestheaderstimeout>" +
                "  </kestrel>" +
                "</config>";

            // act
            var kestrel = Deserialize(xml).Kestrel;

            // validation
            Assert.NotNull(kestrel);
            Assert.Equal(300, kestrel.MaxConcurrentConnections);
            Assert.Equal(3000000000, kestrel.MaxRequestBodySize);
            Assert.Equal(65536, kestrel.MaxRequestHeadersTotalSize);
            Assert.False(kestrel.AllowSynchronousIO);
            Assert.False(kestrel.AllowResponseHeaderCompression);
            Assert.False(kestrel.AddServerHeader);
            Assert.Equal(1000, kestrel.MaxConcurrentUpgradedConnections);
            Assert.Equal(2097152, kestrel.MaxRequestBufferSize);
            Assert.Equal(131072, kestrel.MaxResponseBufferSize);
            Assert.Equal(16384, kestrel.MaxRequestLineSize);
            Assert.Equal(90, kestrel.KeepAliveTimeout);
            Assert.Equal(15, kestrel.RequestHeadersTimeout);
        }

        /// <summary>
        /// Tests that individual settings are independently optional: elements that are not present
        /// remain null, so only explicitly configured values override the defaults.
        /// </summary>
        [Fact]
        public void OmittedElementsRemainNull()
        {
            // arrange
            var xml =
                "<config version=\"1\">" +
                "  <endpoint uri=\"http://localhost/\" />" +
                "  <kestrel>" +
                "    <addserverheader>false</addserverheader>" +
                "  </kestrel>" +
                "</config>";

            // act
            var kestrel = Deserialize(xml).Kestrel;

            // validation
            Assert.NotNull(kestrel);
            Assert.False(kestrel.AddServerHeader);
            Assert.Null(kestrel.AllowSynchronousIO);
            Assert.Null(kestrel.AllowResponseHeaderCompression);
            Assert.Null(kestrel.MaxConcurrentConnections);
            Assert.Null(kestrel.MaxRequestBodySize);
            Assert.Null(kestrel.MaxRequestHeadersTotalSize);
            Assert.Null(kestrel.MaxConcurrentUpgradedConnections);
            Assert.Null(kestrel.MaxRequestBufferSize);
            Assert.Null(kestrel.MaxResponseBufferSize);
            Assert.Null(kestrel.MaxRequestLineSize);
            Assert.Null(kestrel.KeepAliveTimeout);
            Assert.Null(kestrel.RequestHeadersTimeout);
        }

        /// <summary>
        /// Tests that the request limits, which were previously configured through a separate limit
        /// block, are read from the consolidated kestrel block.
        /// </summary>
        [Fact]
        public void RequestLimitsAreDeserialized()
        {
            // arrange
            var xml =
                "<config version=\"1\">" +
                "  <endpoint uri=\"http://localhost/\" />" +
                "  <kestrel>" +
                "    <maxconcurrentconnections>300</maxconcurrentconnections>" +
                "    <maxrequestbodysize>3000000000</maxrequestbodysize>" +
                "    <maxrequestheaderstotalsize>65536</maxrequestheaderstotalsize>" +
                "  </kestrel>" +
                "</config>";

            // act
            var kestrel = Deserialize(xml).Kestrel;

            // validation
            Assert.NotNull(kestrel);
            Assert.Equal(300, kestrel.MaxConcurrentConnections);
            Assert.Equal(3000000000, kestrel.MaxRequestBodySize);
            Assert.Equal(65536, kestrel.MaxRequestHeadersTotalSize);
        }
    }
}
