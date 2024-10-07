using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebPage.Model;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebScope;
using WebExpress.WebCore.WebSitemap;
using WebExpress.WebCore.WebStatusPage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// The page manager manages page elements, which can be called with a URI (Uniform page Identifier).
    /// </summary>
    public class PageManager : IPageManager
    {
        private readonly IComponentHub _componentManager;
        private readonly IHttpServerContext _httpServerContext;
        private readonly PageDictionary _dictionary = [];

        /// <summary>
        /// An event that fires when an page is added.
        /// </summary>
        public event EventHandler<IPageContext> AddPage;

        /// <summary>
        /// An event that fires when an page is removed.
        /// </summary>
        public event EventHandler<IPageContext> RemovePage;

        /// <summary>
        /// Returns all page contexts.
        /// </summary>
        public IEnumerable<IPageContext> Pages => _dictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x.PageContexts);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private PageManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentManager = componentManager;

            _componentManager.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            _componentManager.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            _componentManager.ModuleManager.AddModule += (sender, moduleContext) =>
            {
                AssignToModule(moduleContext);
            };

            _componentManager.ModuleManager.RemoveModule += (sender, moduleContext) =>
            {
                DetachFromModule(moduleContext);
            };

            _componentManager.SitemapManager.Register<PageContext>
            (
                new EndpointRegistration()
                {
                    Factory = (resourceContext, uri, culture) => CreatePageInstance(resourceContext as IPageContext, culture),
                    ContextResolver = (type, moduleContext) => moduleContext != null ? GetPages(type, moduleContext) : GetPages(type),
                    EndpointResolver = () => Pages,
                    HandleRequest = (endpoint, endpontContext, request) =>
                    {
                        var typeOfT = endpoint.GetType().GetGenericArguments()[0];
                        object[] parameters = [endpoint, endpontContext as IPageContext, request];

                        var context = (IRenderContext)Activator.CreateInstance(typeOfT, parameters);

                        (endpoint as IPage).Process(context);

                        return new ResponseOK()
                        {
                            Content = context.VisualTree.Render(new VisualTreeContext(context))
                        };
                    }
                }
            );

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:pagemanager.initialization")
            );
        }

        /// <summary>
        /// Returns an enumeration of all containing page contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose pages are to be registered.</param>
        /// <returns>An enumeration of page contexts.</returns>
        public IEnumerable<IPageContext> GetPages(IPluginContext pluginContext)
        {
            if (!_dictionary.ContainsKey(pluginContext))
            {
                return [];
            }

            return _dictionary[pluginContext].Values
                .SelectMany(x => x.PageContexts);
        }

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <typeparam name="T">The page type.</typeparam>
        /// <returns>An enumeration of page contextes.</returns>
        public IEnumerable<IPageContext> GetPages<T>() where T : IPage
        {
            return GetPages(typeof(T));
        }

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <param name="pageType">The page type.</param>
        /// <returns>An enumeration of page contextes.</returns>
        public IEnumerable<IPageContext> GetPages(Type pageType)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.PageContexts, (x, y) => new { x.PageClass, PageContext = y })
                .Where(x => x.PageClass.Equals(pageType))
                .Select(x => x.PageContext);
        }

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <param name="pageType">The page type.</param>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of page contextes.</returns>
        public IEnumerable<IPageContext> GetPages(Type pageType, IModuleContext moduleContext)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.PageContexts, (x, y) => new { x.PageClass, PageContext = y })
                .Where(x => x.PageContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.PageClass.Equals(pageType))
                .Select(x => x.PageContext);
        }

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <typeparam name="T">The page type.</typeparam>
        /// <param name="moduleContext">The context of the module.</param>
        /// <returns>An enumeration of page contextes.</returns>
        public IEnumerable<IPageContext> GetPages<T>(IModuleContext moduleContext) where T : IPage
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.PageContexts, (x, y) => new { x.PageClass, PageContext = y })
                .Where(x => x.PageContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.PageClass.Equals(typeof(T)))
                .Select(x => x.PageContext);
        }

        /// <summary>
        /// Returns the page context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>An page context or null.</returns>
        public IPageContext GetPage(IModuleContext moduleContext, string pageId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.PageContexts, (x, y) => new { x.PageClass, PageContext = y })
                .Where(x => x.PageContext.ModuleContext?.ApplicationContext != null)
                .Where(x => x.PageContext.ModuleContext.ApplicationContext.ApplicationId.Equals(moduleContext.ApplicationContext.ApplicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.PageContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.PageContext.EndpointId.Equals(pageId, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.PageContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the page context.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        /// <param name="pageType">The page type.</param>
        /// <returns>An page context or null.</returns>
        public IPageContext GetPage(IModuleContext moduleContext, Type pageType)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.PageContexts, (x, y) => new { x.PageClass, PageContext = y })
                .Where(x => x.PageContext.ModuleContext?.ApplicationContext != null)
                .Where(x => x.PageContext.ModuleContext.ApplicationContext.ApplicationId.Equals(moduleContext.ApplicationContext.ApplicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.PageContext.ModuleContext.ModuleId.Equals(moduleContext.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.PageClass.Equals(pageType))
                .Select(x => x.PageContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the page context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>An page context or null.</returns>
        public IPageContext GetPage(string applicationId, string moduleId, string pageId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.PageContexts)
                .Where(x => x.ModuleContext != null && x.ModuleContext.ApplicationContext != null)
                .Where(x => x.ModuleContext.ApplicationContext.ApplicationId.Equals(applicationId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.ModuleContext.ModuleId.Equals(moduleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => x.EndpointId.Equals(pageId, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        /// <summary>
        /// Creates a new page and returns it. If a page already exists (through caching), the existing instance is returned.
        /// </summary>
        /// <param name="pageContext">The context used for page creation.</param>
        /// <param name="culture">The culture with the language settings.</param>
        /// <returns>The created or cached page.</returns>
        private IPage CreatePageInstance(IPageContext pageContext, CultureInfo culture)
        {
            var resourceItem = _dictionary.Values
                .SelectMany(x => x.Values)
                .FirstOrDefault(x => x.PageContexts.Contains(pageContext));

            if (resourceItem != null && resourceItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IPage, IPageContext>(resourceItem.PageClass, pageContext, _componentManager);

                if (instance is II18N i18n)
                {
                    i18n.Culture = culture;
                }

                if (resourceItem.Cache)
                {
                    resourceItem.Instance = instance;
                }

                return instance;
            }

            return resourceItem?.Instance as IPage;
        }

        /// <summary>
        /// Discovers and registers pages from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose pages are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            var assembly = pluginContext?.Assembly;

            _dictionary.Add(pluginContext, []);
            var dict = _dictionary[pluginContext];

            foreach (var resourceType in assembly.GetTypes()
                .Where(x => x.IsClass == true && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterface(typeof(IPage).Name) != null)
                .Where(x => x.GetInterface(typeof(IStatusPage).Name) == null))
            {
                var id = resourceType.FullName?.ToLower();
                var segment = default(ISegmentAttribute);
                var title = resourceType.Name;
                var parent = default(string);
                var contextPath = string.Empty;
                var includeSubPaths = false;
                var moduleId = string.Empty;
                var scopes = new List<string>();
                var conditions = new List<ICondition>();
                var optional = false;
                var cache = false;

                foreach (var customAttribute in resourceType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IResourceAttribute))))
                {
                    var buf = typeof(ModuleAttribute<>);

                    if (customAttribute.AttributeType.GetInterfaces().Contains(typeof(ISegmentAttribute)))
                    {
                        segment = resourceType.GetCustomAttributes(customAttribute.AttributeType, false).FirstOrDefault() as ISegmentAttribute;
                    }
                    else if (customAttribute.AttributeType == typeof(TitleAttribute))
                    {
                        title = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ParentAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ParentAttribute<>).Namespace)
                    {
                        parent = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower();
                    }
                    else if (customAttribute.AttributeType == typeof(ContextPathAttribute))
                    {
                        contextPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(IncludeSubPathsAttribute))
                    {
                        includeSubPaths = Convert.ToBoolean(customAttribute.ConstructorArguments.FirstOrDefault().Value);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ModuleAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ModuleAttribute<>).Namespace)
                    {
                        moduleId = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower();
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ScopeAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ScopeAttribute<>).Namespace)
                    {
                        scopes.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower());
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ConditionAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ConditionAttribute<>).Namespace)
                    {
                        var condition = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                        conditions.Add(Activator.CreateInstance(condition) as ICondition);
                    }
                    else if (customAttribute.AttributeType == typeof(CacheAttribute))
                    {
                        cache = true;
                    }
                    else if (customAttribute.AttributeType == typeof(OptionalAttribute))
                    {
                        optional = true;
                    }
                }

                if (resourceType.GetInterfaces().Where(x => x == typeof(IScope)).Any())
                {
                    scopes.Add(resourceType.FullName?.ToLower());
                }

                if (string.IsNullOrEmpty(moduleId))
                {
                    // no module specified
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate
                        (
                            "webexpress:pagemanager.moduleless",
                            id
                        )
                    );

                    continue;
                }

                if (!dict.ContainsKey(id))
                {
                    var pageItem = new PageItem(_componentManager.PageManager)
                    {
                        PageId = id,
                        Title = title,
                        ParentId = parent,
                        PageClass = resourceType,
                        ModuleId = moduleId,
                        Scopes = scopes,
                        Cache = cache,
                        Conditions = conditions,
                        ContextPath = new UriResource(contextPath),
                        IncludeSubPaths = includeSubPaths,
                        PathSegment = segment.ToPathSegment(),
                        Optional = optional,
                        Log = _httpServerContext?.Log
                    };

                    pageItem.AddPage += (s, e) =>
                    {
                        OnAddPage(e);
                    };

                    pageItem.RemovePage += (s, e) =>
                    {
                        OnRemovePage(e);
                    };

                    dict.Add(id, pageItem);

                    _httpServerContext?.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress:pagemanager.addresource",
                            id,
                            moduleId
                        )
                    );
                }

                // assign the resource to existing modules.
                foreach (var moduleContext in _componentManager.ModuleManager.GetModules(pluginContext, moduleId))
                {
                    AssignToModule(moduleContext);
                }
            }
        }

        /// <summary>
        /// Discovers and registers resources from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the resources.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                Register(pluginContext);
            }
        }

        /// <summary>
        /// Removes all pages associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the pages to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return;
            }

            // the plugin has not been registered in the manager
            if (!_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            foreach (var resourceItem in _dictionary[pluginContext].Values)
            {
                resourceItem.Dispose();
            }

            _dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Assign existing resources to the module.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void AssignToModule(IModuleContext moduleContext)
        {
            foreach (var resourceItem in _dictionary.Values
                .SelectMany(x => x.Values)
                .Where(x => x.ModuleId.Equals(moduleContext?.ModuleId, StringComparison.OrdinalIgnoreCase))
                .Where(x => !x.IsAssociatedWithModule(moduleContext)))
            {
                resourceItem.AddModule(moduleContext);
            }
        }

        /// <summary>
        /// Remove an existing modules to the application.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        private void DetachFromModule(IModuleContext moduleContext)
        {
            foreach (var resourceItem in _dictionary.Values
                .SelectMany(x => x.Values)
                .Where(x => !x.IsAssociatedWithModule(moduleContext)))
            {
                resourceItem.DetachModule(moduleContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of all containing page items of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose pages are to be registered.</param>
        /// <returns>An enumeration of pages items.</returns>
        private IEnumerable<PageItem> GetPageItems(IPluginContext pluginContext)
        {
            if (!_dictionary.ContainsKey(pluginContext))
            {
                return [];
            }

            return _dictionary[pluginContext].Values;
        }

        /// <summary>
        /// Raises the AddPage event.
        /// </summary>
        /// <param name="resourceContext">The page context.</param>
        private void OnAddPage(IPageContext resourceContext)
        {
            AddPage?.Invoke(this, resourceContext);
        }

        /// <summary>
        /// Raises the RemovePage event.
        /// </summary>
        /// <param name="pageContext">The page context.</param>
        private void OnRemovePage(IPageContext pageContext)
        {
            RemovePage?.Invoke(this, pageContext);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            foreach (var resourcenItem in GetPageItems(pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(deep) +
                    I18N.Translate
                    (
                        "webexpress:pagemanager.resource",
                        resourcenItem.PageId,
                        string.Join(",", resourcenItem.ModuleId)
                    )
                );
            }
        }
    }
}
