using System;
using System.Collections.Generic;
using System.Globalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// The resource manager manages WebExpress elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public interface IResourceManager : IManager
    {
        /// <summary>
        /// An event that fires when an resource is added.
        /// </summary>
        event EventHandler<IResourceContext> AddResource;

        /// <summary>
        /// An event that fires when an resource is removed.
        /// </summary>
        event EventHandler<IResourceContext> RemoveResource;

        /// <summary>
        /// Returns all resource contexts.
        /// </summary>
        IEnumerable<IResourceContext> Resources { get; }

        /// <summary>
        /// Returns an enumeration of all containing resource contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose resources are to be registered.</param>
        /// <returns>An enumeration of resource contexts.</returns>
        IEnumerable<IResourceContext> GetResorces(IPluginContext pluginContext);

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <typeparam name="T">The resource type.</typeparam>
        /// <returns>An enumeration of resource contextes.</returns>
        IEnumerable<IResourceContext> GetResorces<T>() where T : IResource;

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>An enumeration of resource contextes.</returns>
        IEnumerable<IResourceContext> GetResorces(Type resourceType);

        /// <summary>
        /// Returns an enumeration of resource contextes.
        /// </summary>
        /// <typeparam name="T">The resource type.</typeparam>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of resource contextes.</returns>
        IEnumerable<IResourceContext> GetResorces<T>(IModuleContext moduleContext) where T : IResource;

        /// <summary>
        /// Returns the resource context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>An resource context or null.</returns>
        IResourceContext GetResorce(IModuleContext moduleContext, string resourceId);

        /// <summary>
        /// Returns the resource context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>An resource context or null.</returns>
        IResourceContext GetResorce(IModuleContext moduleContext, Type resourceType);

        /// <summary>
        /// Returns the resource context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="resourceId">The resource id.</param>
        /// <returns>An resource context or null.</returns>
        IResourceContext GetResorce(string applicationId, string moduleId, string resourceId);

        /// <summary>
        /// Creates a new resource and returns it. If a resource already exists (through caching), the existing instance is returned.
        /// </summary>
        /// <param name="resourceContext">The context used for resource creation.</param>
        /// <param name="culture">The culture with the language settings.</param>
        /// <returns>The created or cached resource.</returns>
        IResource CreateResourceInstance(IResourceContext resourceContext, CultureInfo culture);
    }
}
