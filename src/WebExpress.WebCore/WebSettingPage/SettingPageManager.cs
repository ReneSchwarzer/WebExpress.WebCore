using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebScope;
using WebExpress.WebCore.WebSettingPage.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Management of settings pages.
    /// </summary>
    public sealed class SettingPageManager : ISettingPageManager
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly SettingPageDictionary _dictionary = new();
        private static readonly Dictionary<Type, Delegate> _delegateCache = [];

        /// <summary>
        /// An event that fires when an setting page is added.
        /// </summary>
        public event EventHandler<ISettingPageContext> AddSettingPage;

        /// <summary>
        /// An event that fires when an setting page is removed.
        /// </summary>
        public event EventHandler<ISettingPageContext> RemoveSettingPage;

        /// <summary>
        /// Returns the collection of setting pages.
        /// </summary>
        public IEnumerable<ISettingPageContext> SettingPages => _dictionary.All;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used via Reflection.")]
        private SettingPageManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;
            _httpServerContext = httpServerContext;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            var endpointtRegistration = new EndpointRegistration()
            {
                EndpointResolver = (type, applicationContext) => applicationContext != null ? GetSettingPages(type, applicationContext) : GetSettingPages(type),
                EndpointsResolver = () => SettingPages,
                HandleRequest = (request, endpontContext) =>
                {
                    var pageInstance = CreateSettingPageInstance(endpontContext as ISettingPageContext);
                    var pageType = pageInstance.GetType();
                    var pageContext = endpontContext as IPageContext;
                    var renderContext = new RenderContext(pageInstance, pageContext, request);
                    var visualTreeContext = new VisualTreeContext(renderContext);

                    var visualTreeType = pageType.GetInterface(typeof(ISettingPage<>).Name).GetGenericArguments()[0];
                    if (!_delegateCache.TryGetValue(pageType, out var del))
                    {
                        // create and compile the expression
                        var renderContextParam = Expression.Parameter(typeof(IRenderContext), "renderContext");
                        var visualTreeParam = Expression.Parameter(visualTreeType, "visualTree");
                        var processMethod = pageType.GetMethod("Process", [typeof(IRenderContext), visualTreeType]);
                        var callProzessMethod = Expression.Call
                        (
                            Expression.Constant(pageInstance),
                            processMethod,
                            renderContextParam,
                            visualTreeParam
                        );
                        var lambda = Expression.Lambda(callProzessMethod, renderContextParam, visualTreeParam)
                            .Compile();

                        _delegateCache[pageType] = lambda;
                        del = lambda;
                    }

                    // create visual tree instance
                    var visualTreeInstance = default(IVisualTree);
                    var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                    var constructors = visualTreeType?.GetConstructors(flags);

                    if (constructors != null)
                    {
                        foreach (var constructor in constructors.OrderByDescending(x => x.GetParameters().Length))
                        {
                            // injection
                            var parameters = constructor.GetParameters();
                            var hubProperties = _componentHub.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                            var contextIdProperty = pageContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .Where(x => x.PropertyType == typeof(IComponentId))
                                .FirstOrDefault();

                            var parameterValues = parameters.Select(parameter =>
                                parameter.ParameterType == typeof(IComponentHub) ? componentHub :
                                parameter.ParameterType == typeof(IHttpServerContext) ? httpServerContext :
                                parameter.ParameterType == typeof(IPageContext) ? pageContext :
                                parameter.ParameterType == typeof(IComponentId) ? contextIdProperty?.GetValue(pageContext) :
                                hubProperties.Where(x => x.PropertyType == parameter.ParameterType)
                                    .FirstOrDefault()?
                                    .GetValue(componentHub) ?? null
                            ).ToArray();

                            if (constructor.Invoke(parameterValues) is IVisualTree visualTree)
                            {
                                visualTreeInstance = visualTree;
                            }
                        }
                    }
                    else
                    {
                        visualTreeInstance = Activator.CreateInstance<IVisualTree>();
                    }

                    // execute the cached delegate
                    del.DynamicInvoke(renderContext, visualTreeInstance);

                    return new ResponseOK()
                    {
                        Content = visualTreeInstance.Render(visualTreeContext)
                    };
                }
            };

            AddSettingPage += (sender, e) => endpointtRegistration.AddEndpoint?.Invoke(sender, e);
            RemoveSettingPage += (sender, e) => endpointtRegistration.RemoveEndpoint?.Invoke(sender, e);

            _componentHub.EndpointManager.Register<SettingPageContext>(endpointtRegistration);

            _httpServerContext.Log.Debug(I18N.Translate("webexpress.webapp:pagesettingmanager.initialization"));
        }

        /// <summary>
        /// Creates a new setting page and returns it. If a page already exists (through caching), the existing instance is returned.
        /// </summary>
        /// <param name="settingPageContext">The context used for setting page creation.</param>
        /// <returns>The created or cached page.</returns>
        private IEndpoint CreateSettingPageInstance(ISettingPageContext settingPageContext)
        {
            return _dictionary.CreateSettingPageInstance(settingPageContext, _componentHub, _httpServerContext);
        }

        /// <summary>
        /// Discovers and binds setting pages to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose setting pages are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_dictionary.Contains(pluginContext))
            {
                return;
            }

            Register(pluginContext, _componentHub.ApplicationManager.GetApplications(pluginContext));
        }

        /// <summary>
        /// Discovers and binds setting pages to an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application whose pages are to be associated.</param>
        private void Register(IApplicationContext applicationContext)
        {
            if (_dictionary.Contains(applicationContext))
            {
                return;
            }

            foreach (var pluginContext in _componentHub.PluginManager.GetPlugins(applicationContext))
            {
                Register(pluginContext, [applicationContext]);
            }
        }

        /// <summary>
        /// Registers pages for a given plugin and application context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContexts">The application context (optional).</param>
        private void Register(IPluginContext pluginContext, IEnumerable<IApplicationContext> applicationContexts)
        {
            var assembly = pluginContext.Assembly;

            foreach (var settingPageType in assembly.GetTypes()
                .Where(x => x.IsClass == true && x.IsSealed && x.IsPublic)
                .Where(x => x.GetInterface(typeof(ISettingPage<>).Name) != null))
            {
                var id = settingPageType.FullName?.ToLower();
                var title = settingPageType.Name;
                var segment = default(ISegmentAttribute);
                var parent = default(Type);
                var contextPath = string.Empty;
                var scopes = new List<Type>();
                var category = default(string);
                var group = default(string);
                var section = SettingSection.Primary;
                var hide = false;
                var icon = default(string);
                var cache = false;

                // determining attributes
                foreach (var customAttribute in settingPageType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IEndpointAttribute))))
                {
                    if (customAttribute.AttributeType.GetInterfaces().Contains(typeof(ISegmentAttribute)))
                    {
                        segment = settingPageType.GetCustomAttributes(customAttribute.AttributeType, false).FirstOrDefault() as ISegmentAttribute;
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ParentAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ParentAttribute<>).Namespace)
                    {
                        parent = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                    }
                    else if (customAttribute.AttributeType == typeof(ContextPathAttribute))
                    {
                        contextPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(SettingCategoryAttribute))
                    {
                        category = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(SettingGroupAttribute))
                    {
                        group = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(SettingSectionAttribute))
                    {
                        section = Enum.Parse<SettingSection>(customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString());
                    }
                    else if (customAttribute.AttributeType == typeof(SettingHideAttribute))
                    {
                        hide = true;
                    }
                    else if (customAttribute.AttributeType == typeof(IconAttribute))
                    {
                        icon = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(CacheAttribute))
                    {
                        cache = true;
                    }
                }

                foreach (var customAttribute in settingPageType.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(ISettingPageAttribute))))
                {
                    if (customAttribute.AttributeType == typeof(TitleAttribute))
                    {
                        title = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ScopeAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ScopeAttribute<>).Namespace)
                    {
                        scopes.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault());
                    }
                }

                if (settingPageType.GetInterfaces().Where(x => x == typeof(IScope)).Any())
                {
                    scopes.Add(settingPageType);
                }

                // assign the fragment to existing applications
                foreach (var applicationContext in applicationContexts)
                {
                    var settingPageContext = new SettingPageContext(_componentHub.EndpointManager, parent, new UriResource(contextPath), segment.ToPathSegment())
                    {
                        ApplicationContext = applicationContext,
                        PluginContext = pluginContext,
                        EndpointId = new ComponentId(id),
                        PageTitle = title,
                        Scopes = scopes,
                        Category = category,
                        Section = section,
                        Group = group,
                        Hide = hide,
                        Icon = icon,
                        Cache = cache
                    };

                    // Create meta information of the setting page
                    var settingPageItem = new SettingPageItem()
                    {
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        SettingPageContext = settingPageContext,
                        SettingPageClass = settingPageType,
                        Context = category,
                        Section = section,
                        Group = group,
                        Cache = cache
                    };

                    // Insert the settings page into the dictionary
                    if (_dictionary.AddSettingPageItem(settingPageItem))
                    {
                        OnAddSettingPage(settingPageContext);

                        _httpServerContext?.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress.webcore:settingpagemanager.register",
                                id,
                                section,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Removes all elemets associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the elemets to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            _dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Removes all setting pages associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the fragments to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            foreach (var settingPageContext in _dictionary.Remove(applicationContext))
            {
                OnRemoveSettingPage(settingPageContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of setting page contextes.
        /// </summary>
        /// <param name="settingPageType">The setting page type.</param>
        /// <returns>An enumeration of setting page contextes.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(Type settingPageType)
        {
            return _dictionary.GetSettingPages(settingPageType);
        }

        /// <summary>
        /// Returns an enumeration of setting page contextes.
        /// </summary>
        /// <param name="settingPageType">The setting page type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of setting page contextes.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(Type settingPageType, IApplicationContext applicationContext)
        {
            return _dictionary.GetSettingPages(settingPageType, applicationContext);
        }

        /// <summary>
        /// Returns the categories associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of category names.</returns>
        public IEnumerable<string> GetCategories(IApplicationContext applicationContext)
        {
            return _dictionary.GetCategories(applicationContext);
        }

        /// <summary>
        /// Returns the groups associated with the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve groups.</param>
        /// <returns>An enumeration of group names.</returns>
        public IEnumerable<string> GetGroups(IApplicationContext applicationContext, string category)
        {
            return _dictionary.GetGroups(applicationContext, category);
        }

        /// <summary>
        /// Returns an enumeration of setting page contexts for the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve setting pages.</param>
        /// <returns>An enumeration of setting page contexts.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(IApplicationContext applicationContext, string category)
        {
            return _dictionary.GetSettingPages(applicationContext, category);
        }

        /// <summary>
        /// Returns the first setting page context for the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve setting pages.</param>
        /// <returns>The first setting page context or null.</returns>
        public ISettingPageContext GetFirstSettingPage(IApplicationContext applicationContext, string category)
        {
            var pages = _dictionary.GetSettingPages(applicationContext, category);
            var preferences = pages.Where(x => x.Section == SettingSection.Preferences);
            var primary = pages.Where(x => x.Section == SettingSection.Primary);
            var secondary = pages.Where(x => x.Section == SettingSection.Secondary);

            if (preferences.Any())
            {
                return preferences.OrderBy(x => x.PageTitle).FirstOrDefault();
            }

            if (primary.Any())
            {
                return primary.OrderBy(x => x.PageTitle).FirstOrDefault();
            }

            if (secondary.Any())
            {
                return secondary.OrderBy(x => x.PageTitle).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Returns an enumeration of setting page contexts for the specified application context, category, and group.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve setting pages.</param>
        /// <param name="group">The group for which to retrieve setting pages.</param>
        /// <returns>An enumeration of setting page contexts.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(IApplicationContext applicationContext, string category, string group)
        {
            return _dictionary.GetSettingPages(applicationContext, category, group);
        }

        /// <summary>
        /// Raises the AddSettingPage event.
        /// </summary>
        /// <param name="settingPageContext">The setting page context.</param>
        private void OnAddSettingPage(ISettingPageContext settingPageContext)
        {
            AddSettingPage?.Invoke(this, settingPageContext);
        }

        /// <summary>
        /// Raises the RemoveSettingPage event.
        /// </summary>
        /// <param name="settingPageContext">The setting page context.</param>
        private void OnRemoveSettingPage(ISettingPageContext settingPageContext)
        {
            RemoveSettingPage?.Invoke(this, settingPageContext);
        }

        /// <summary>
        /// Raises the event when an plugin is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the plugin being added.</param>
        private void OnAddPlugin(object sender, IPluginContext e)
        {
            Register(e);
        }

        /// <summary>  
        /// Raises the event when a plugin is removed.  
        /// </summary>  
        /// <param name="sender">The source of the event.</param>  
        /// <param name="e">The context of the plugin being removed.</param>  
        private void OnRemovePlugin(object sender, IPluginContext e)
        {
            Remove(e);
        }

        /// <summary>
        /// Raises the event when an application is added.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being added.</param>
        private void OnAddApplication(object sender, IApplicationContext e)
        {
            Register(e);
        }

        /// <summary>
        /// Raises the event when an application is removed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The context of the application being removed.</param>
        private void OnRemoveApplication(object sender, IApplicationContext e)
        {
            Remove(e);
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

            GC.SuppressFinalize(this);
        }
    }
}