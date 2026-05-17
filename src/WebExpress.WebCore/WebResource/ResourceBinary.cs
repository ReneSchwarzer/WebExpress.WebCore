using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// A binary resource.
    /// </summary>
    public abstract class ResourceBinary : Resource
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="resourceContext">The resource context.</param>
        public ResourceBinary(IResourceContext resourceContext)
            : base(resourceContext)
        {
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public override IResponse Process(IRequest request)
        {
            var response = new ResponseOK();
            response.Header.ContentLength = Data is not null ? Data.Length : 0;
            response.Header.ContentType = "binary/octet-stream";

            response.Content = Data;

            return response;
        }
    }
}
