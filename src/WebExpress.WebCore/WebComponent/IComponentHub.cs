using System;
using System.Collections.Generic;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebEvent;
using WebExpress.WebCore.WebJob;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPackage;
using WebExpress.WebCore.WebPage;
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
    public interface IComponentHub : IComponentManager
    {
        /// <summary>
        /// An event that fires when an component is added.
        /// </summary>
        event EventHandler<IComponentManager> AddComponent;

        /// <summary>
        /// An event that fires when an component is removed.
        /// </summary>
        event EventHandler<IComponentManager> RemoveComponent;

        /// <summary>
        /// Returns the reference to the context of the host.
        /// </summary>
        IHttpServerContext HttpServerContext { get; }

        /// <summary>
        /// Returns all registered components.
        /// </summary>
        IEnumerable<IComponentManager> Managers { get; }

        /// <summary>
        /// Returns the log manager.
        /// </summary>
        /// <returns>The instance of the log manager.</returns>
        LogManager LogManager { get; }

        /// <summary>
        /// Returns the package manager.
        /// </summary>
        /// <returns>The instance of the package manager.</returns>
        PackageManager PackageManager { get; }

        /// <summary>
        /// Returns the plugin manager.
        /// </summary>
        /// <returns>The instance of the plugin manager.</returns>
        public IPluginManager PluginManager { get; }
        /// <summary>
        /// Returns the application manager.
        /// </summary>
        /// <returns>The instance of the application manager.</returns>
        IApplicationManager ApplicationManager { get; }

        /// <summary>
        /// Returns the module manager.
        /// </summary>
        /// <returns>The instance of the module manager.</returns>
        IModuleManager ModuleManager { get; }

        /// <summary>
        /// Returns the event manager.
        /// </summary>
        /// <returns>The instance of the event manager.</returns>
        EventManager EventManager { get; }

        /// <summary>
        /// Returns the job manager.
        /// </summary>
        /// <returns>The instance of the job manager.</returns>
        JobManager JobManager { get; }

        /// <summary>
        /// Returns the status page manager.
        /// </summary>
        /// <returns>The instance of the status page manager.</returns>
        StatusPageManager StatusPageManager { get; }

        /// <summary>
        /// Returns the resource manager.
        /// </summary>
        /// <returns>The instance of the resource manager.</returns>
        IResourceManager ResourceManager { get; }

        /// <summary>
        /// Returns the page manager.
        /// </summary>
        /// <returns>The instance of the page manager.</returns>
        IPageManager PageManager { get; }

        /// <summary>
        /// Returns the sitemap manager.
        /// </summary>
        /// <returns>The instance of the sitemap manager.</returns>
        ISitemapManager SitemapManager { get; }

        /// <summary>
        /// Returns the internationalization manager.
        /// </summary>
        /// <returns>The instance of the internationalization manager.</returns>
        IInternationalizationManager InternationalizationManager { get; }

        /// <summary>
        /// Returns the session manager.
        /// </summary>
        /// <returns>The instance of the session manager.</returns>
        SessionManager SessionManager { get; }

        /// <summary>
        /// Returns the task manager.
        /// </summary>
        /// <returns>The instance of the task manager manager.</returns>
        TaskManager TaskManager { get; }

        /// <summary>
        /// Returns a component based on its id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The instance of the component.</returns>
        IComponentManager GetComponent(string id);

        /// <summary>
        /// Returns a component based on its type.
        /// </summary>
        /// <typeparam name="T">The component class.</typeparam>
        /// <returns>The instance of the component.</returns>
        T GetComponent<T>() where T : IComponentManager;
    }
}
