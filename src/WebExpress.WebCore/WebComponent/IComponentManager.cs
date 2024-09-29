using System;
using System.Collections.Generic;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebEvent;
using WebExpress.WebCore.WebJob;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPackage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebSession;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebTask;

namespace WebExpress.WebCore.WebComponent
{
    /// <summary>
    /// Interface of the central management of manager components.
    /// </summary>
    public interface IComponentManager : IManager
    {
        /// <summary>
        /// An event that fires when an component is added.
        /// </summary>
        event EventHandler<IManager> AddComponent;

        /// <summary>
        /// An event that fires when an component is removed.
        /// </summary>
        event EventHandler<IManager> RemoveComponent;

        /// <summary>
        /// Returns the reference to the context of the host.
        /// </summary>
        IHttpServerContext HttpServerContext { get; }

        /// <summary>
        /// Returns all registered components.
        /// </summary>
        IEnumerable<IManager> Managers { get; }

        /// <summary>
        /// Returns the log manager.
        /// </summary>
        /// <returns>The instance of the log manager or null.</returns>
        LogManager LogManager { get; }

        /// <summary>
        /// Returns the package manager.
        /// </summary>
        /// <returns>The instance of the package manager or null.</returns>
        PackageManager PackageManager { get; }

        /// <summary>
        /// Returns the plugin manager.
        /// </summary>
        /// <returns>The instance of the plugin manager or null.</returns>
        public IPluginManager PluginManager { get; }
        /// <summary>
        /// Returns the application manager.
        /// </summary>
        /// <returns>The instance of the application manager or null.</returns>
        IApplicationManager ApplicationManager { get; }

        /// <summary>
        /// Returns the module manager.
        /// </summary>
        /// <returns>The instance of the module manager or null.</returns>
        IModuleManager ModuleManager { get; }

        /// <summary>
        /// Returns the event manager.
        /// </summary>
        /// <returns>The instance of the event manager or null.</returns>
        EventManager EventManager { get; }

        /// <summary>
        /// Returns the job manager.
        /// </summary>
        /// <returns>The instance of the job manager or null.</returns>
        JobManager JobManager { get; }

        /// <summary>
        /// Returns the status page manager.
        /// </summary>
        /// <returns>The instance of the status page manager or null.</returns>
        StatusPageManager StatusPageManager { get; }

        /// <summary>
        /// Returns the resource manager.
        /// </summary>
        /// <returns>The instance of the resource manager or null.</returns>
        IResourceManager ResourceManager { get; }

        /// <summary>
        /// Returns the sitemap manager.
        /// </summary>
        /// <returns>The instance of the sitemap manager or null.</returns>
        ISitemapManager SitemapManager { get; }

        /// <summary>
        /// Returns the internationalization manager.
        /// </summary>
        /// <returns>The instance of the internationalization manager or null.</returns>
        IInternationalizationManager InternationalizationManager { get; }

        /// <summary>
        /// Returns the session manager.
        /// </summary>
        /// <returns>The instance of the session manager or null.</returns>
        SessionManager SessionManager { get; }

        /// <summary>
        /// Returns the task manager.
        /// </summary>
        /// <returns>The instance of the task manager manager or null.</returns>
        TaskManager TaskManager { get; }

        /// <summary>
        /// Returns a component based on its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The instance of the component or null.</returns>
        IManager GetComponent(string id);

        /// <summary>
        /// Returns a component based on its type.
        /// </summary>
        /// <typeparam name="T">The component class.</typeparam>
        /// <returns>The instance of the component or null.</returns>
        T GetComponent<T>() where T : IManager;
    }
}
