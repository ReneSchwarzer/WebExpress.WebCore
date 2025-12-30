using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// The page manager manages rest api resources, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public interface IRestApiManager : IComponentManager
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
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of rest api contextes.</returns>
        IEnumerable<IRestApiContext> GetRestApi(Type restApiType, IApplicationContext applicationContext);

        /// <summary>
        /// Returns an enumeration of rest api contextes.
        /// </summary>
        /// <typeparam name="T">The rest api type.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of rest api contextes.</returns>
        IEnumerable<IRestApiContext> GetRestApi<T>(IApplicationContext applicationContext) where T : IRestApi;

        /// <summary>
        /// Returns the rest api context.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="restApiId">The rest api id.</param>
        /// <returns>An rest api context or null.</returns>
        IRestApiContext GetRestApi(IApplicationContext applicationContext, string restApiId);

        /// <summary>
        /// Returns the rest api context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="restApiId">The rest api id.</param>
        /// <returns>An rest api context or null.</returns>
        IRestApiContext GetRestApi(string applicationId, string restApiId);
    }
}
