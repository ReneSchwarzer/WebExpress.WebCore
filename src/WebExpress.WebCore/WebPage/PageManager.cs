using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage.Model;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebScope;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// The page manager manages page elements, which can be called with a URI (Uniform page Identifier).
    /// </summary>
    public class PageManager : IPageManager
    {
        private readonly IComponentHub _componentHub;
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
            .SelectMany(x => x.Values)
            .Select(x => x.PageContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private PageManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentHub = componentManager;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            _componentHub.EndpointManager.Register<PageContext>
            (
                new EndpointRegistration()
                {
                    EndpointResolver = (type, applicationContext) => applicationContext != null ? GetPages(type, applicationContext) : GetPages(type),
                    EndpointsResolver = () => Pages,
                    HandleRequest = (request, endpontContext) =>
                    {
                        var page = CreatePageInstance(endpontContext as IPageContext, request.Culture);
                        var pageType = page.GetType();
                        var context = default(IRenderContext);
                        var pageContetx = endpontContext as IPageContext;

                        if (pageType.IsGenericType)
                        {
                            var typeOfT = pageType.GetGenericArguments()[0];
                            var parameters = new object[] { page, endpontContext as IPageContext, request };

                            context = Activator.CreateInstance(typeOfT, parameters) as IRenderContext;
                        }
                        else
                        {
                            context = new RenderContext(endpontContext?.ApplicationContext, request, pageContetx.Scopes);
                        }

                        page.Process(context);

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
            if (_dictionary.TryGetValue(pluginContext, out var pluginResources))
            {
                return pluginResources
                    .SelectMany(x => x.Value)
                    .Select(x => x.Value.PageContext);
            }

            return [];
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
                .SelectMany(x => x.Values)
                .Where(x => x.PageClass.Equals(pageType))
                .Select(x => x.PageContext);
        }

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <param name="pageType">The page type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of page contextes.</returns>
        public IEnumerable<IPageContext> GetPages(Type pageType, IApplicationContext applicationContext)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.PageClass.Equals(pageType))
                .Where(x => x.PageContext.ApplicationContext.Equals(applicationContext))
                .Select(x => x.PageContext);
        }

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <typeparam name="T">The page type.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of page contextes.</returns>
        public IEnumerable<IPageContext> GetPages<T>(IApplicationContext applicationContext) where T : IPage
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.PageClass.Equals(typeof(T)))
                .Where(x => x.PageContext.ApplicationContext.Equals(applicationContext))
                .Select(x => x.PageContext);
        }

        /// <summary>
        /// Returns the page context.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>An page context or null.</returns>
        public IPageContext GetPage(IApplicationContext applicationContext, string pageId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.PageContext.ApplicationContext.Equals(applicationContext))
                .Where(x => x.PageContext.EndpointId.Equals(pageId))
                .Select(x => x.PageContext)
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns the page context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>An page context or null.</returns>
        public IPageContext GetPage(string applicationId, string pageId)
        {
            return _dictionary.Values
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .Where(x => x.PageContext.ApplicationContext.ApplicationId.Equals(applicationId))
                .Where(x => x.PageContext.EndpointId.Equals(pageId))
                .Select(x => x.PageContext)
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
                .SelectMany(x => x.Values)
                .FirstOrDefault(x => x.PageContext.Equals(pageContext));

            if (resourceItem != null && resourceItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IPage, IPageContext>(resourceItem.PageClass, pageContext, _componentHub);

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

            return resourceItem?.Instance;
        }

        /// <summary>
        /// Discovers and binds pages to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose pages are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            Register(pluginContext, _componentHub.ApplicationManager.GetApplications(pluginContext));
        }

        /// <summary>
        /// Discovers and binds pages to an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application whose pages are to be associated.</param>
        private void Register(IApplicationContext applicationContext)
        {
            foreach (var pluginContext in _componentHub.PluginManager.GetPlugins(applicationContext))
            {
                if (_dictionary.TryGetValue(pluginContext, out var appDict) && appDict.ContainsKey(applicationContext))
                {
                    continue;
                }

                Register(pluginContext, [applicationContext]);
            }
        }

        /// <summary>
        /// Registers pages for a given plugin and application context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
        {
            var assembly = pluginContext?.Assembly;

            foreach (var resourceType in assembly.GetTypes()
                .Where(x => x.IsClass == true && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterface(typeof(IPage).Name) != null))
            {
                var id = resourceType.FullName?.ToLower();
                var segment = default(ISegmentAttribute);
                var title = resourceType.Name;
                var parent = default(Type);
                var contextPath = string.Empty;
                var includeSubPaths = false;
                var scopes = new List<string>();
                var conditions = new List<ICondition>();
                var cache = false;

                foreach (var customAttribute in resourceType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEndpointAttribute))))
                {
                    if (customAttribute.AttributeType.GetInterfaces().Contains(typeof(ISegmentAttribute)))
                    {
                        segment = resourceType.GetCustomAttributes(customAttribute.AttributeType, false).FirstOrDefault() as ISegmentAttribute;
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ParentAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ParentAttribute<>).Namespace)
                    {
                        parent = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                    }
                    else if (customAttribute.AttributeType == typeof(ContextPathAttribute))
                    {
                        contextPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(IncludeSubPathsAttribute))
                    {
                        includeSubPaths = Convert.ToBoolean(customAttribute.ConstructorArguments.FirstOrDefault().Value);
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
                }

                foreach (var customAttribute in resourceType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IPageAttribute))))
                {
                    if (customAttribute.AttributeType == typeof(TitleAttribute))
                    {
                        title = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ScopeAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ScopeAttribute<>).Namespace)
                    {
                        scopes.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower());
                    }
                }

                if (resourceType.GetInterfaces().Where(x => x == typeof(IScope)).Any())
                {
                    scopes.Add(resourceType.FullName?.ToLower());
                }

                // assign the job to existing applications
                foreach (var applicationContext in _componentHub.ApplicationManager.GetApplications(pluginContext))
                {
                    var pageContext = new PageContext(_componentHub.EndpointManager, parent, new UriResource(contextPath), segment.ToPathSegment())
                    {
                        PageTitle = title,
                        EndpointId = resourceType.FullName.ToLower(),
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        Cache = cache,
                        Scopes = scopes,
                        Conditions = conditions,
                        IncludeSubPaths = includeSubPaths
                    };

                    var pageItem = new PageItem(_componentHub.PageManager)
                    {
                        Title = title,
                        PageContext = pageContext,
                        ParentType = parent,
                        PageClass = resourceType,
                        Scopes = scopes,
                        Cache = cache,
                        Conditions = conditions,
                        ContextPath = new UriResource(contextPath),
                        IncludeSubPaths = includeSubPaths,
                        PathSegment = segment.ToPathSegment(),
                        Log = _httpServerContext?.Log
                    };

                    if (_dictionary.AddPageItem(pluginContext, applicationContext, pageItem))
                    {
                        OnAddPage(pageContext);

                        _httpServerContext?.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:pagemanager.addresource",
                                id,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
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

            foreach (var resourceItem in _dictionary[pluginContext].Values
                .SelectMany(x => x.Values))
            {
                OnRemovePage(resourceItem.PageContext);
                resourceItem.Dispose();
            }

            _dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Removes all jobs associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the jobs to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                return;
            }

            foreach (var pluginDict in _dictionary.Values)
            {
                foreach (var appDict in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var resourceItem in appDict.Values)
                    {
                        OnRemovePage(resourceItem.PageContext);
                        resourceItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of all containing page items of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose pages are to be registered.</param>
        /// <returns>An enumeration of pages items.</returns>
        private IEnumerable<PageItem> GetPageItems(IPluginContext pluginContext)
        {
            if (_dictionary.TryGetValue(pluginContext, out var pluginResources))
            {
                return pluginResources
                    .SelectMany(x => x.Value)
                    .Select(x => x.Value);
            }

            return [];
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
        /// Handles the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being removed.</param>
        private void OnRemoveApplication(object sender, IApplicationContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Handles the event when an plugin is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the plugin being added.</param>
        private void OnAddPlugin(object sender, IPluginContext e)
        {
            Register(e);
        }

        /// <summary>  
        /// Handles the event when a plugin is removed.  
        /// </summary>  
        /// <param name="sender">The source of the event.</param>  
        /// <param name="e">The context of the plugin being removed.</param>  
        private void OnRemovePlugin(object sender, IPluginContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Handles the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext e)
        {
            Register(e);
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
                        resourcenItem?.PageContext?.EndpointId,
                        string.Join(",", resourcenItem?.PageContext?.ApplicationContext?.ApplicationId)
                    )
                );
            }
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            _componentHub.PluginManager.AddPlugin -= OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin -= OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication -= OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication -= OnRemoveApplication;
        }
    }
}
