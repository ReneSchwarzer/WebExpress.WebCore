using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebFragment.Model;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebScope;
using WebExpress.WebCore.WebSection;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// The fragment manager. Fragments are independent parts of a page.
    /// </summary>
    public sealed class FragmentManager : IFragmentManager
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly FragmentDictionary _dictionary = [];

        /// <summary>
        /// An event that fires when an fragment is added.
        /// </summary>
        public event EventHandler<IFragmentContext> AddFragment;

        /// <summary>
        /// An event that fires when an fragment is removed.
        /// </summary>
        public event EventHandler<IFragmentContext> RemoveFragment;

        /// <summary>
        /// Returns the collection of fragment contexts.
        /// </summary>
        public IEnumerable<IFragmentContext> Fragments => _dictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x.Values)
            .SelectMany(x => x.Values)
            .SelectMany(x => x)
            .Select(x => x.FragmentContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        private FragmentManager(IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            _componentHub = componentHub;
            _httpServerContext = httpServerContext;

            _componentHub.PluginManager.AddPlugin += OnAddPlugin;
            _componentHub.PluginManager.RemovePlugin += OnRemovePlugin;
            _componentHub.ApplicationManager.AddApplication += OnAddApplication;
            _componentHub.ApplicationManager.RemoveApplication += OnRemoveApplication;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:fragmentmanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and binds fragments to an application.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin whose fragments are to be associated.</param>
        private void Register(IPluginContext pluginContext)
        {
            if (_dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            Register(pluginContext, _componentHub.ApplicationManager.GetApplications(pluginContext));
        }

        /// <summary>
        /// Discovers and binds fragments to an application.
        /// </summary>
        /// <param name="applicationContext">The context of the application whose fragments are to be associated.</param>
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

            foreach (var fragmentType in assembly.GetTypes().Where
                (
                    x => x.IsClass &&
                    x.IsSealed &&
                    x.IsPublic &&
                    (
                        x.GetInterfaces().Contains(typeof(IFragment)) ||
                        x.GetInterfaces().Contains(typeof(IFragmentDynamic))
                    )
                ))
            {
                var id = fragmentType.FullName?.ToLower();
                var scopes = new List<Type>();
                var sections = new List<Type>();
                var conditions = new List<ICondition>();
                var cache = false;
                var order = 0;

                // determining attributes
                foreach (var customAttribute in fragmentType.CustomAttributes.Where
                (
                    x => x.AttributeType.GetInterfaces()
                            .Contains(typeof(IEndpointAttribute))
                ))
                {
                    if (customAttribute.AttributeType.Name == typeof(ScopeAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ScopeAttribute<>).Namespace)
                    {
                        scopes.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault());
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

                foreach (var customAttribute in fragmentType.CustomAttributes.Where
                (
                    x => x.AttributeType.GetInterfaces().Contains(typeof(IFragmentAttribute))
                ))
                {
                    if (customAttribute.AttributeType.Name == typeof(SectionAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(SectionAttribute<>).Namespace)
                    {
                        sections.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault());
                    }
                    else if (customAttribute.AttributeType == typeof(OrderAttribute))
                    {
                        try
                        {
                            order = Convert.ToInt32(customAttribute.ConstructorArguments.FirstOrDefault().Value);
                        }
                        catch
                        {
                        }
                    }
                }

                // check section
                if (sections.Count == 0)
                {
                    _httpServerContext.Log.Warning(I18N.Translate
                    (
                        "webexpress.webcore:fragmentmanager.error.section"
                    ));

                    continue;
                }

                // assign the fragment to existing applications
                foreach (var applicationContext in _componentHub.ApplicationManager.GetApplications(pluginContext))
                {
                    // assign section
                    foreach (var section in sections)
                    {
                        // assign scope
                        foreach (var scope in scopes)
                        {
                            var fragmentContext = new FragmentContext()
                            {
                                PluginContext = pluginContext,
                                ApplicationContext = applicationContext,
                                FragmentId = id,
                                Cache = cache,
                                Section = section,
                                Scope = scope,
                                Conditions = conditions
                            };

                            var fragmentItem = new FragmentItem(_componentHub, _httpServerContext)
                            {
                                PluginContext = pluginContext,
                                ApplicationContext = applicationContext,
                                FragmentContext = fragmentContext,
                                FragmentClass = fragmentType,
                                Order = order,
                                Cache = cache,
                                Conditions = conditions,
                                Section = section,
                                Scope = scope
                            };

                            if (_dictionary.AddFragmentItem(pluginContext, applicationContext, fragmentItem))
                            {
                                OnAddFragment(fragmentContext);

                                _httpServerContext?.Log.Debug
                                (
                                    I18N.Translate
                                    (
                                        "webexpress.webcore:fragmentmanager.register",
                                        id,
                                        section,
                                        applicationContext.ApplicationId
                                    )
                                );
                            }
                        }
                    }
                }
            }

            Log();
        }

        /// <summary>
        /// Removes all components associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the components to remove.</param>
        internal void Remove(IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return;
            }

            var fragments = _dictionary.RemoveFragments(pluginContext);

            foreach (var fragment in fragments)
            {
                OnRemoveFragment(fragment);
            }
        }

        /// <summary>
        /// Removes all fragments associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the fragments to remove.</param>
        internal void Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                return;
            }

            var fragments = _dictionary.RemoveFragments(applicationContext);

            foreach (var fragment in fragments)
            {
                OnRemoveFragment(fragment);
            }
        }

        /// <summary>
        /// Raises the AddFragment event.
        /// </summary>
        /// <param name="fragmentContext">The fragment context.</param>
        private void OnAddFragment(IFragmentContext fragmentContext)
        {
            AddFragment?.Invoke(this, fragmentContext);
        }

        /// <summary>
        /// Raises the RemoveFragment event.
        /// </summary>
        /// <param name="fragmentContext">The fragment context.</param>
        private void OnRemoveFragment(IFragmentContext fragmentContext)
        {
            RemoveFragment?.Invoke(this, fragmentContext);
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
        /// Returns all fragment contexts that belong to a given fragment type.
        /// </summary>
        /// <typeparam name="T">The fragment type.</typeparam>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        public IEnumerable<IFragmentContext> GetFragments<T>() where T : IFragment
        {
            return GetFragments(typeof(T));
        }

        /// <summary>
        /// Returns all fragment contexts that belong to a given fragment type.
        /// </summary>
        /// <param name="fragmentType">The fragment type.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        public IEnumerable<IFragmentContext> GetFragments(Type fragmentType)
        {
            return _dictionary.Values
                .SelectMany(x => x)
                .SelectMany(x => x.Value)
                .SelectMany(x => x.Value)
                .SelectMany(x => x.Value)
                .Where(x => x.FragmentClass == fragmentType)
                .OrderBy(x => x.Order)
                .Select(x => x.FragmentContext);
        }

        /// <summary>
        /// Returns all fragment contexts that belong to a given fragment type.
        /// </summary>
        /// <typeparam name="T">The fragment type..</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        public IEnumerable<IFragmentContext> GetFragments<T>(IApplicationContext applicationContext) where T : IFragment
        {
            return GetFragments(applicationContext, typeof(T));
        }

        /// <summary>
        /// Returns all fragment contexts that belong to a given fragment type.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="fragmentType">The fragment type.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        public IEnumerable<IFragmentContext> GetFragments(IApplicationContext applicationContext, Type fragmentType)
        {
            return _dictionary.Values
                .SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(x => x.Value)
                .SelectMany(x => x.Value)
                .SelectMany(x => x.Value)
                .Where(x => x.FragmentClass == fragmentType)
                .OrderBy(x => x.Order)
                .Select(x => x.FragmentContext);
        }

        /// <summary>
        /// Returns all fragment contexts that belong to a given application.
        /// </summary>
        /// <typeparam name="S">The section where the fragment is embedded.</typeparam>
        /// <typeparam name="T">The scope where the fragment is embedded.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        public IEnumerable<IFragmentContext> GetFragments<S, T>(IApplicationContext applicationContext) where S : ISection where T : IScope
        {
            return GetFragments(applicationContext, typeof(S), typeof(T));
        }

        /// <summary>
        /// Returns all fragment contexts that belong to a given application.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="section">The section where the fragment is embedded.</param>
        /// <param name="scope">The scope where the fragment is embedded.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        public IEnumerable<IFragmentContext> GetFragments(IApplicationContext applicationContext, Type section, Type scope)
        {
            return _dictionary.Values
                .SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(x => x.Value)
                .Where(x => x.Key == section)
                .SelectMany(x => x.Value)
                .Where(x => x.Key == scope)
                .SelectMany(x => x.Value)
                .OrderBy(x => x.Order)
                .Select(x => x.FragmentContext);
        }

        /// <summary>
        /// Returns all fragment contexts that belong to a given application.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="section">The section where the fragment is embedded.</param>
        /// <param name="scopes">The scopes where the fragment is embedded.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        public IEnumerable<IFragmentContext> GetFragments(IApplicationContext applicationContext, Type section, IEnumerable<Type> scopes)
        {
            foreach (var scope in scopes)
            {
                foreach (var item in GetFragments(applicationContext, section, scope))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Processes the fragments for a given section within the specified render context.
        /// </summary>
        /// <param name="renderContext">The context in which rendering occurs.</param>
        /// <param name="section">The section where the fragment is embedded.</param>
        public void Process(IRenderContext renderContext, Type section)
        {
            var scopes = renderContext?.PageContext?.Scopes ?? [];

            var items = _dictionary.Values
                .SelectMany(x => x)
                .Where(x => x.Key == renderContext?.PageContext?.ApplicationContext)
                .SelectMany(x => x.Value)
                .Where(x => x.Key == section)
                .SelectMany(x => x.Value)
                .Where(x => scopes.Any(y => x.Key == y))
                .SelectMany(x => x.Value)
                .OrderBy(x => x.Order);

            foreach (var item in items)
            {
                item.Process(renderContext);
            }
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        private void Log()
        {
            //output.Add
            //(
            //    string.Empty.PadRight(deep) +
            //    I18N.Translate("webexpress.webui:fragmentmanager.titel")
            //);

            //foreach (var fragmentItem in GetFragmentItems(pluginContext))
            //{
            //    output.Add
            //    (
            //        string.Empty.PadRight(deep + 2) +
            //        I18N.Translate
            //        (
            //            "webexpress.webui:fragmentmanager.fragment",
            //            fragmentItem.FragmentClass.Name
            //        )
            //    );
            //}
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
