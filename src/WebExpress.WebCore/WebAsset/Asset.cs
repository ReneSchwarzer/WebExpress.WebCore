using System.IO;
using System.Reflection;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebAsset
{
    /// <summary>
    /// Delivery of a resource embedded in the assembly.
    /// </summary>
    public class Asset : IAsset
    {
        private readonly IComponentHub _componentHub;
        private readonly IAssetContext _assetContext;
        private readonly IHttpServerContext _httpServerContext;
        private readonly string _embeddedResource;
        private byte[] _data;

        /// <summary>
        /// Gets the root directory.
        /// </summary>
        public string AssetDirectory { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="assetContext">The asset context.</param>
        /// <param name="httpServerContext">The server context.</param>
        /// <param name="embeddedResource">The embedded resource name.</param>
        public Asset(IComponentHub componentHub, IAssetContext assetContext, IHttpServerContext httpServerContext, string embeddedResource)
        {
            _componentHub = componentHub;
            _assetContext = assetContext;
            _httpServerContext = httpServerContext;
            _embeddedResource = embeddedResource;

            var assembly = _assetContext.PluginContext.Assembly;
            _data = GetData(assembly);
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public IResponse Process(IRequest request)
        {
            if (_data is null)
            {
                return new ResponseNotFound();
            }

            var extension = Path.GetExtension(_assetContext.EndpointId.ToString())?.ToLower() ?? "";
            var response = new ResponseOK();

            response.Header.CacheControl = "public, max-age=31536000";
            response.Header.ContentLength = _data.Length;
            response.Content = _data;

            switch (extension)
            {
                case ".pdf":
                    response.Header.ContentType = "application/pdf";
                    break;
                case ".txt":
                    response.Header.ContentType = "text/plain";
                    break;
                case ".css":
                    response.Header.ContentType = "text/css";
                    break;
                case ".js":
                    response.Header.ContentType = "application/javascript";
                    break;
                case ".xml":
                    response.Header.ContentType = "text/xml";
                    break;
                case ".html":
                case ".htm":
                    response.Header.ContentType = "text/html";
                    break;
                case ".zip":
                    response.Header.ContentDisposition = "attatchment; filename=" + _assetContext.EndpointId + "; size=" + _data.LongLength;
                    response.Header.ContentType = "application/zip";
                    break;
                case ".doc":
                case ".docx":
                    response.Header.ContentType = "application/msword";
                    break;
                case ".xls":
                case ".xlx":
                    response.Header.ContentType = "application/vnd.ms-excel";
                    break;
                case ".ppt":
                    response.Header.ContentType = "application/vnd.ms-powerpoint";
                    break;
                case ".gif":
                    response.Header.ContentType = "image/gif";
                    break;
                case ".png":
                    response.Header.ContentType = "image/png";
                    break;
                case ".svg":
                    response.Header.ContentType = "image/svg+xml";
                    break;
                case ".jpeg":
                case ".jpg":
                    response.Header.ContentType = "image/jpg";
                    break;
                case ".ico":
                    response.Header.ContentType = "image/x-icon";
                    break;
                case ".mp3":
                    response.Header.ContentType = "audio/mpeg";
                    break;
                case ".mp4":
                    response.Header.ContentType = "video/mp4";
                    break;
                default:
                    response.Header.ContentType = "binary/octet-stream";
                    break;
            }

            _httpServerContext.Log?.Debug(I18N.Translate
            (
                "webexpress.webcore:asset.file",
                request.RemoteEndPoint, request.Uri
            ));

            return response;
        }

        /// <summary>
        /// Reads the data of a specified resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The data.</returns>
        private byte[] GetData(Assembly assembly)
        {
            if (assembly is null || _embeddedResource is null)
            {
                return [];
            }

            using var stream = assembly.GetManifestResourceStream(_embeddedResource);
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _data = null;
        }
    }
}