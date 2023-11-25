using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// A file resource.
    /// </summary>
    public class ResourceFile : ResourceBinary
    {
        /// <summary>
        /// Protection in case of concurrency.
        /// </summary>
        private object Gard { get; set; }

        /// <summary>
        /// Returns the root directory.
        /// </summary>
        public string RootDirectory { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ResourceFile()
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
                var contextPath = ResourceContext.ContextPath;
                var url = request.Uri.ToString()[contextPath.ToString().Length..];

                var path = System.IO.Path.GetFullPath(RootDirectory + url);

                if (!System.IO.File.Exists(path))
                {
                    return new ResponseNotFound();
                }

                Data = System.IO.File.ReadAllBytes(path);

                var response = base.Process(request);
                response.Header.CacheControl = "public, max-age=31536000";

                var extension = System.IO.Path.GetExtension(path);
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
                        response.Header.ContentDisposition = "attatchment; filename=" + System.IO.Path.GetFileName(path) + "; size=" + Data.LongLength;
                        response.Header.ContentType = "application/octet-stream";
                        break;
                    case ".zip":
                        response.Header.ContentDisposition = "attatchment; filename=" + System.IO.Path.GetFileName(path) + "; size=" + Data.LongLength;
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

                request.ServerContext.Log.Debug(InternationalizationManager.I18N("webexpress:resource.file", request.RemoteEndPoint, request.Uri));

                return response;
            }
        }
    }
}
