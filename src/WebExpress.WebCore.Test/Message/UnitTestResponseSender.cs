using System.IO.Pipelines;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.Test.Message
{
    /// <summary>
    /// Unit tests for <see cref="ResponseSender"/>, focusing on the handling of connection-specific
    /// headers across HTTP protocol versions where their validity differs.
    /// </summary>
    public class UnitTestResponseSender
    {
        /// <summary>
        /// Sends the given response through the sender on a connection negotiated with the specified
        /// protocol and returns the populated response feature for header inspection.
        /// </summary>
        /// <param name="protocol">The negotiated protocol, e.g. "HTTP/1.1" or "HTTP/2".</param>
        /// <param name="response">The response to send.</param>
        /// <returns>The response feature carrying the emitted headers and status.</returns>
        private static async Task<IHttpResponseFeature> Send(string protocol, IResponse response)
        {
            var features = new FeatureCollection();
            features.Set<IHttpRequestFeature>(new HttpRequestFeature { Protocol = protocol, Headers = new HeaderDictionary() });
            features.Set<IHttpResponseFeature>(new HttpResponseFeature { Headers = new HeaderDictionary() });
            features.Set<IHttpResponseBodyFeature>(new FakeResponseBodyFeature());

            await new ResponseSender().SendAsync(new FakeHttpContext(features), response);

            return features.Get<IHttpResponseFeature>();
        }

        /// <summary>
        /// Tests that the connection-specific Keep-Alive header is never emitted: persistence is
        /// owned by Kestrel and the header is forbidden on HTTP/2.
        /// </summary>
        [Theory]
        [InlineData("HTTP/1.1")]
        [InlineData("HTTP/2")]
        public async Task KeepAliveHeaderIsNeverSent(string protocol)
        {
            var feature = await Send(protocol, new ResponseOK());

            Assert.False(feature.Headers.ContainsKey("Keep-Alive"));
        }

        /// <summary>
        /// Tests that the websocket handshake headers are emitted on HTTP/1.x, where they are valid.
        /// </summary>
        [Fact]
        public async Task ConnectionAndUpgradeEmittedOnHttp1()
        {
            var response = new ResponseSwitchingProtocols("upgrade", "websocket", "s3pPLMBiTxaQ9kYGzzhZRbK+xOo=");

            var feature = await Send("HTTP/1.1", response);

            Assert.True(feature.Headers.ContainsKey("Connection"));
            Assert.True(feature.Headers.ContainsKey("Upgrade"));
        }

        /// <summary>
        /// Tests that the connection-specific Connection and Upgrade headers are suppressed on
        /// HTTP/2, where Kestrel rejects them (RFC 9113 §8.2.2).
        /// </summary>
        [Fact]
        public async Task ConnectionAndUpgradeSuppressedOnHttp2()
        {
            var response = new ResponseSwitchingProtocols("upgrade", "websocket", "s3pPLMBiTxaQ9kYGzzhZRbK+xOo=");

            var feature = await Send("HTTP/2", response);

            Assert.False(feature.Headers.ContainsKey("Connection"));
            Assert.False(feature.Headers.ContainsKey("Upgrade"));
        }

        /// <summary>
        /// A minimal <see cref="IHttpContext"/> backed by an explicit feature collection, used to
        /// drive the sender without constructing a full request pipeline.
        /// </summary>
        private sealed class FakeHttpContext : IHttpContext
        {
            public FakeHttpContext(IFeatureCollection features) => Features = features;

            public IHttpServerContext HttpServerContext => null;
            public string Id => "test";
            public IRequest Request => null;
            public EndPoint LocalEndPoint => new IPEndPoint(IPAddress.Loopback, 80);
            public EndPoint RemoteEndPoint => new IPEndPoint(IPAddress.Loopback, 12345);
            public IFeatureCollection Features { get; }
            public Encoding Encoding => Encoding.UTF8;
            public Uri Uri => new("http://localhost/");
        }

        /// <summary>
        /// A minimal response body feature that discards the written body into an in-memory stream.
        /// </summary>
        private sealed class FakeResponseBodyFeature : IHttpResponseBodyFeature
        {
            public Stream Stream { get; } = new MemoryStream();
            public PipeWriter Writer => PipeWriter.Create(Stream);
            public Task CompleteAsync() => Task.CompletedTask;
            public void DisableBuffering() { }
            public Task SendFileAsync(string path, long offset, long? count, CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task StartAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        }
    }
}
