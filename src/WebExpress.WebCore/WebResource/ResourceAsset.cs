using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// Delivery of a resource embedded in the assembly.
    /// </summary>
    public class ResourceAsset : ResourceBinary
    {
        /// <summary>
        /// Protection against concurrency.
        /// </summary>
        private object Gard { get; set; }

        /// <summary>
        /// Returns the root directory.
        /// </summary>
        public string AssetDirectory { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResourceAsset()
        {
            Gard = new object();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);

            AssetDirectory = ResourceContext.PluginContext.Assembly.GetName().Name;
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public override Response Process(Request request)
        {
            lock (Gard)
            {
                var assembly = ResourceContext.PluginContext.Assembly;
                var buf = assembly.GetManifestResourceNames().ToList();
                var resources = assembly.GetManifestResourceNames().Where(x => x.StartsWith(AssetDirectory, System.StringComparison.OrdinalIgnoreCase));
                var contextPath = ResourceContext.ContextPath;
                var url = request.Uri.ExtendedPath.ToString();
                var fileName = Path.GetFileName(url);
                var file = string.Join('.', AssetDirectory.Trim('.'), "assets", url.Replace("/", ".").Trim('.'));

                Data = GetData(file, assembly, resources.ToList());

                if (Data == null)
                {
                    return new ResponseNotFound();
                }

                var response = base.Process(request);
                response.Header.CacheControl = "public, max-age=31536000";

                var extension = Path.GetExtension(fileName);
                extension = !string.IsNullOrWhiteSpace(extension) ? extension.ToLower() : "";

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
                    case ".xml":
                        response.Header.ContentType = "text/xml";
                        break;
                    case ".html":
                    case ".htm":
                        response.Header.ContentType = "text/html";
                        break;
                    case ".exe":
                        response.Header.ContentDisposition = "attatchment; filename=" + fileName + "; size=" + Data.LongLength;
                        response.Header.ContentType = "application/octet-stream";
                        break;
                    case ".zip":
                        response.Header.ContentDisposition = "attatchment; filename=" + fileName + "; size=" + Data.LongLength;
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
                }

                request.ServerContext.Log.Debug(InternationalizationManager.I18N
                    (
                        "webexpress:resource.file",
                        request.RemoteEndPoint, request.Uri
                    ));

                return response;
            }
        }

        /// <summary>
        /// Reads the data of a specified resource.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The data.</returns>
        private static byte[] GetData(string file, Assembly assembly, IEnumerable<string> resources)
        {
            var item = resources.Where(x => x.Equals(file, System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (item == null)
            {
                return null;
            }

            using var stream = assembly.GetManifestResourceStream(item);
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
    }
}