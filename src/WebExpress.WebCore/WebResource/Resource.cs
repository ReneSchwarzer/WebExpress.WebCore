using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// Represents a resource in the web application.
    /// </summary>
    public abstract class Resource : IResource
    {
        /// <summary>
        /// Returns the resource context where the resource exists.
        /// </summary>
        public IResourceContext ResourceContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// <param name="resourceContext">The context of the resource.</param>
        /// </summary>
        public Resource(IResourceContext resourceContext)
        {
            ResourceContext = resourceContext;
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public abstract Response Process(Request request);

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
