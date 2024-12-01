using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebPlugin;
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
        private readonly SettingPageDictionary _dictionary = [];

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
        public IEnumerable<ISettingPageContext> SettingPages => _dictionary.SettingPages;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
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
                    var settingPage = CreateSettingPageInstance(endpontContext as ISettingPageContext);
                    var settingPageType = settingPage.GetType();
                    var context = default(IRenderContext);
                    var pageContetx = endpontContext as IPageContext;

                    if (settingPageType.IsGenericType)
                    {
                        var typeOfT = settingPageType.GetGenericArguments()[0];
                        var parameters = new object[] { settingPage, endpontContext as IPageContext, request };

                        context = Activator.CreateInstance(typeOfT, parameters) as IRenderContext;
                    }
                    else
                    {
                        context = new RenderContext(pageContetx, request);
                    }

                    settingPage.Process(context);

                    return new ResponseOK()
                    {
                        Content = context.VisualTree.Render(new VisualTreeContext(context))
                    };
                }
            };

            AddSettingPage += (sender, e) => endpointtRegistration.AddEndpoint?.Invoke(sender, e);
            RemoveSettingPage += (sender, e) => endpointtRegistration.RemoveEndpoint?.Invoke(sender, e);

            _componentHub.EndpointManager.Register<PageContext>(endpointtRegistration);

            _httpServerContext.Log.Debug(I18N.Translate("webexpress.webapp:pagesettingmanager.initialization"));
        }

        /// <summary>
        /// Creates a new setting page and returns it. If a page already exists (through caching), the existing instance is returned.
        /// </summary>
        /// <param name="settinPageContext">The context used for setting page creation.</param>
        /// <returns>The created or cached page.</returns>
        private ISettingPage CreateSettingPageInstance(ISettingPageContext settinPageContext)
        {
            var settingPageItem = _dictionary.Values
                .SelectMany(a => a.Values)
                .SelectMany(c => c.Values)
                .SelectMany(s => s.Values)
                .SelectMany(g => g.Values)
                .SelectMany(i => i)
                .FirstOrDefault(x => x.SettingPageContext.Equals(settinPageContext));

            if (settingPageItem != null && settingPageItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<ISettingPage, ISettingPageContext>(settingPageItem.SettingPageClass, settinPageContext, _httpServerContext, _componentHub);

                if (settingPageItem.Cache)
                {
                    settingPageItem.Instance = instance;
                }

                return instance;
            }

            return settingPageItem?.Instance;
        }

        /// <summary>
        /// Discovers and binds setting pages to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose setting pages are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
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
            var assembly = pluginContext.Assembly;

            foreach (var settingPageType in assembly.GetTypes()
                    .Where(x => x.IsClass && x.IsSealed && (x.GetInterfaces().Contains(typeof(ISettingPage)))))
            {
                var id = settingPageType.FullName?.ToLower();
                var title = settingPageType.Name;
                var segment = default(ISegmentAttribute);
                var parent = default(Type);
                var contextPath = string.Empty;
                var scopes = new List<string>();
                var context = default(string);
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
                    else if (customAttribute.AttributeType == typeof(SettingContextAttribute))
                    {
                        context = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
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
                        scopes.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower());
                    }
                }

                // assign the fragment to existing applications
                foreach (var applicationContext in applicationContexts)
                {
                    var settingPageContext = new SettingPageContext(_componentHub.EndpointManager, parent, new UriResource(contextPath), segment.ToPathSegment())
                    {
                        ApplicationContext = applicationContext,
                        PluginContext = pluginContext,
                        EndpointId = id,
                        SettingPageTitle = title,
                        Scopes = scopes,
                        Context = context,
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
                        Context = context,
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
                                "webexpress:settingpagemanager.register",
                                id,
                                section,
                                applicationContext.ApplicationId
                            )
                        );
                    }
                }
            }

            // Logging
            Log();
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
            if (applicationContext == null)
            {
                return;
            }

            foreach (var pluginDict in _dictionary.Values)
            {
                foreach (var appDict in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var settingPageItem in appDict.Values
                        .SelectMany(s => s.Values)
                        .SelectMany(g => g.Values)
                        .SelectMany(i => i))
                    {
                        OnRemoveSettingPage(settingPageItem.SettingPageContext);
                        settingPageItem.Dispose();
                    }
                }

                pluginDict.Remove(applicationContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of setting page contextes.
        /// </summary>
        /// <param name="settingPageType">The setting page type.</param>
        /// <returns>An enumeration of setting page contextes.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(Type settingPageType)
        {
            return _dictionary.Values
                .SelectMany(a => a.Values)
                .SelectMany(c => c.Values)
                .SelectMany(s => s.Values)
                .SelectMany(g => g.Values)
                .SelectMany(i => i)
                .Where(x => x.SettingPageClass.Equals(settingPageType))
                .Select(x => x.SettingPageContext);
        }

        /// <summary>
        /// Returns an enumeration of setting page contextes.
        /// </summary>
        /// <param name="settingPageType">The setting page type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of setting page contextes.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(Type settingPageType, IApplicationContext applicationContext)
        {
            return _dictionary.Values
                .SelectMany(a => a)
                .Where(a => a.Key.Equals(applicationContext))
                .Select(a => a.Value)
                .SelectMany(c => c.Values)
                .SelectMany(s => s.Values)
                .SelectMany(g => g.Values)
                .SelectMany(i => i)
                .Where(x => x.SettingPageClass.Equals(settingPageType))
                .Select(x => x.SettingPageContext);
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
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        private void Log()
        {
            //output.Add
            //(
            //    string.Empty.PadRight(deep) +
            //    I18N.Translate("webexpress.webui:settingpagemanager.titel")
            //);

            //var log = new List<string>
            //    {
            //            I18N.Translate("webexpress.webapp:pagesettingmanager.register"),
            //    "    SettingContext       = " + context ?? "null",
            //            "    SettingSection       = " + section.ToString(),
            //            "    SettingGroup         = " + group ?? "null",
            //            "    SettingPage.Id       = " + page?.Id ?? "null",
            //            "    SettingPage.Hide     = " + (page?.Hide != null ? page?.Hide.ToString() : "null")
            //    };

            //_httpServerContext.Log.Debug(string.Join(Environment.NewLine, log));
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