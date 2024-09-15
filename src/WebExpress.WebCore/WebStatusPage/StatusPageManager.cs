using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebResource;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebStatusPage
{
    /// <summary>
    /// Management of status pages.
    /// </summary>
    public class StatusPageManager : IComponentPlugin, ISystemComponent
    {
        /// <summary>
        /// An event that fires when an status page is added.
        /// </summary>
        public event EventHandler<IResourceContext> AddStatusPage;

        /// <summary>
        /// An event that fires when an status page is removed.
        /// </summary>
        public event EventHandler<IResourceContext> RemoveStatusPage;

        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory where the status pages are listed.
        /// </summary>
        private StatusPageDictionary Dictionary { get; } = new StatusPageDictionary();

        /// <summary>
        /// Returns the default Items.
        /// </summary>
        private StatusPageDictionaryItem Defaults { get; } = new StatusPageDictionaryItem();

        /// <summary>
        /// Returns all status pages.
        /// </summary>
        public IEnumerable<IStatusPageContext> StatusPages => Dictionary.Values
            .SelectMany(x => x.Values)
            .Select(x => new StatusPageContext()
            {
                PluginContext = x.PluginContext,
                Code = x.StatusCode,
                Title = x.Title,
                Icon = x.Icon
            }
        );

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal StatusPageManager()
        {
            ComponentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            ComponentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        public void Initialization(IHttpServerContext context)
        {
            HttpServerContext = context;

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:statuspagemanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and registers status pages from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose status pages are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            var assembly = pluginContext?.Assembly;

            foreach (var resource in assembly.GetTypes()
                .Where(x => x.IsClass == true && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterface(typeof(IStatusPage).Name) != null))
            {
                var id = resource.Name?.ToLower();
                var statusCode = -1;
                var icon = string.Empty;
                var title = resource.Name;
                //var moduleId = string.Empty;
                var defaultItem = false;

                foreach (var customAttribute in resource.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IApplicationAttribute))))
                {
                    if (customAttribute.AttributeType == typeof(StatusCodeAttribute))
                    {
                        statusCode = Convert.ToInt32(customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString());
                    }
                    else if (customAttribute.AttributeType == typeof(TitleAttribute))
                    {
                        title = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(IconAttribute))
                    {
                        icon = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    //else if (customAttribute.AttributeType.Name == typeof(ModuleAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ModuleAttribute<>).Namespace)
                    //{
                    //    moduleId = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower();
                    //}
                }

                foreach (var customAttribute in resource.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IStatusPageAttribute))))
                {
                    if (customAttribute.AttributeType == typeof(DefaultAttribute))
                    {
                        defaultItem = true;
                    }
                }

                if (statusCode > 0)
                {
                    if (!Dictionary.ContainsKey(pluginContext))
                    {
                        Dictionary.Add(pluginContext, new StatusPageDictionaryItem());
                    }

                    var item = Dictionary[pluginContext];
                    if (!item.ContainsKey(statusCode))
                    {
                        item.Add(statusCode, new StatusPageItem()
                        {
                            Id = id,
                            StatusCode = statusCode,
                            StatusPageClass = resource,
                            PluginContext = pluginContext,
                            Title = title,
                            Icon = new UriResource(icon),
                            //ModuleId = moduleId
                        });
                        HttpServerContext.Log.Debug
                        (
                            InternationalizationManager.I18N
                            (
                                "webexpress:statuspagemanager.register",
                                statusCode,
                                //moduleId,
                                resource.Name
                            )
                        );
                    }
                    else
                    {
                        HttpServerContext.Log.Debug
                        (
                            InternationalizationManager.I18N
                            (
                                "webexpress:statuspagemanager.duplicat",
                                statusCode,
                                //moduleId,
                                resource.Name
                            )
                        );
                    }

                    // default
                    if (!Defaults.ContainsKey(statusCode))
                    {
                        Defaults.Add(statusCode, new StatusPageItem()
                        {
                            Id = id,
                            StatusCode = statusCode,
                            StatusPageClass = resource,
                            PluginContext = pluginContext,
                            //ModuleId = moduleId
                        });
                    }
                    else if (defaultItem)
                    {
                        Defaults[statusCode] = new StatusPageItem()
                        {
                            Id = id,
                            StatusCode = statusCode,
                            StatusPageClass = resource,
                            PluginContext = pluginContext,
                            //ModuleId = moduleId
                        };
                    }

                }
                else
                {
                    HttpServerContext.Log.Debug
                    (
                        InternationalizationManager.I18N
                        (
                            "webexpress:statuspagemanager.statuscode",
                            //moduleId,
                            resource.Name
                        )
                    );
                }
            }
        }

        /// <summary>
        /// Discovers and registers entries from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the status pages.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                Register(pluginContext);
            }
        }

        /// <summary>
        /// Returns the status codes for a given plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <returns>An enumeration of the status codes for the given plugin.</returns>
        internal IEnumerable<int> GetStatusCodes(IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return Enumerable.Empty<int>();
            }

            if (Dictionary.ContainsKey(pluginContext))
            {
                return Dictionary[pluginContext].Keys;
            }

            return Enumerable.Empty<int>();
        }

        /// <summary>
        /// Returns the class for an status code.
        /// </summary>
        /// <param name="status">The status code.</param>
        /// <returns>The first status page found to the given states or null.</returns>
        private StatusPageItem GetStatusPage(int status)
        {
            if (Defaults == null)
            {
                return null;
            }

            if (!Defaults.ContainsKey(status))
            {
                return null;
            }

            return Defaults[status];
        }

        /// <summary>
        /// Returns the class for an status page.
        /// </summary>
        /// <param name="status">The status code.</param>
        /// <param name="pluginContext">The plugin context where the status pages are located.</param>
        /// <returns>The first status page found to the given states or null.</returns>
        private StatusPageItem GetStatusPage(int status, IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return null;
            }

            if (!Dictionary.ContainsKey(pluginContext))
            {
                return null;
            }

            if (!Dictionary[pluginContext].ContainsKey(status))
            {
                return null;
            }

            return Dictionary[pluginContext][status];
        }

        /// <summary>
        /// Creates a status page.
        /// </summary>
        /// <param name="massage">The status message.</param>
        /// <param name="status">The status code.</param>
        /// <param name="pluginContext">The module context where the status pages are located or null for an undefined page (may be from another module) that matches the status code.</param>
        /// <returns>The created status page or null.</returns>
        public IStatusPage CreateStatusPage(string massage, int status, IPluginContext pluginContext)
        {
            var responseItem = GetStatusPage(status, pluginContext);

            responseItem ??= GetStatusPage(status);

            if (responseItem == null)
            {
                return null;
            }

            var statusPage = responseItem.StatusPageClass.Assembly.CreateInstance(responseItem.StatusPageClass?.FullName) as IStatusPage;
            statusPage.StatusMessage = massage;
            statusPage.StatusCode = status;

            return statusPage;
        }

        /// <summary>
        /// Removes all status pages associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the status pages to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {

        }

        /// <summary>
        /// Raises the AddStatusPage event.
        /// </summary>
        /// <param name="statusPage">The status page.</param>
        private void OnAddStatusPage(IResourceContext statusPage)
        {
            AddStatusPage?.Invoke(this, statusPage);
        }

        /// <summary>
        /// Raises the RemoveComponent event.
        /// </summary>
        /// <param name="statusPage">The status page.</param>
        private void OnRemoveStatusPage(IResourceContext statusPage)
        {
            RemoveStatusPage?.Invoke(this, statusPage);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            foreach (var statusCode in GetStatusCodes(pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(4) +
                    InternationalizationManager.I18N
                    (
                        "webexpress:statuspagemanager.statuspage",
                        statusCode
                    )
                );
            }
        }
    }
}
