using System.Globalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebModule;

namespace WebExpress.WebCore.WebResource
{
    public abstract class Resource : IResource
    {
        /// <summary>
        /// Returns the context of the application.
        /// </summary>
        public IApplicationContext ApplicationContext => ResourceContext?.ModuleContext?.ApplicationContext;

        /// <summary>
        /// Returns the context of the module.
        /// </summary>
        public IModuleContext ModuleContext => ResourceContext?.ModuleContext;

        /// <summary>
        /// Returns the resource context where the resource exists.
        /// </summary>
        public IResourceContext ResourceContext { get; private set; }

        /// <summary>
        /// Provides the culture.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Resource()
        {
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="resourceContext">The context of the resource.</param>
        public virtual void Initialization(IResourceContext resourceContext)
        {
            ResourceContext = resourceContext;
        }

        /// <summary>
        /// Preprocessing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        public virtual void PreProcess(Request request)
        {
            return;
        }

        /// <summary>
        /// Processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The response.</returns>
        public abstract Response Process(Request request);

        /// <summary>
        /// Post-processing of the resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns>The response.</returns>
        public virtual Response PostProcess(Request request, Response response)
        {
            return response;
        }
    }
}
