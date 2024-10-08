using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// The page manager manages rest api resources, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public interface IRestApiManager : IEndpointManager
    {
        /// <summary>
        /// An event that fires when an rest api is added.
        /// </summary>
        event EventHandler<IRestApiContext> AddRestApi;

        /// <summary>
        /// An event that fires when an rest api is removed.
        /// </summary>
        event EventHandler<IRestApiContext> RemoveRestApi;

        /// <summary>
        /// Returns all rest api contexts.
        /// </summary>
        IEnumerable<IRestApiContext> RestApis { get; }

        /// <summary>
        /// Returns an enumeration of all containing rest api contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose rest apis are to be registered.</param>
        /// <returns>An enumeration of rest api contexts.</returns>
        IEnumerable<IRestApiContext> GetRestApi(IPluginContext pluginContext);

        /// <summary>
        /// Returns an enumeration of rest api contextes.
        /// </summary>
        /// <typeparam name="T">The rest api type.</typeparam>
        /// <returns>An enumeration of rest api contextes.</returns>
        IEnumerable<IRestApiContext> GetRestApi<T>() where T : IRestApi;

        /// <summary>
        /// Returns an enumeration of rest api contextes.
        /// </summary>
        /// <param name="pageType">The rest api type.</param>
        /// <returns>An enumeration of rest api contextes.</returns>
        IEnumerable<IRestApiContext> GetRestApi(Type pageType);

        /// <summary>
        /// Returns an enumeration of rest api contextes.
        /// </summary>
        /// <param name="restApiType">The rest api type.</param>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of rest api contextes.</returns>
        IEnumerable<IRestApiContext> GetRestApi(Type restApiType, IModuleContext moduleContext);

        /// <summary>
        /// Returns an enumeration of rest api contextes.
        /// </summary>
        /// <typeparam name="T">The rest api type.</typeparam>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of rest api contextes.</returns>
        IEnumerable<IRestApiContext> GetRestApi<T>(IModuleContext moduleContext) where T : IRestApi;

        /// <summary>
        /// Returns the rest api context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="restApiId">The rest api id.</param>
        /// <returns>An rest api context or null.</returns>
        IRestApiContext GetRestApi(IModuleContext moduleContext, string restApiId);

        /// <summary>
        /// Returns the rest api context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="restApiType">The rest api type.</param>
        /// <returns>An rest api context or null.</returns>
        IRestApiContext GetRestApi(IModuleContext moduleContext, Type restApiType);

        /// <summary>
        /// Returns the rest api context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="restApiId">The rest api id.</param>
        /// <returns>An rest api context or null.</returns>
        IRestApiContext GetRestApi(string applicationId, string moduleId, string restApiId);
    }
}
