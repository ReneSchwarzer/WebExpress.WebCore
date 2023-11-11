using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebExpress.Core.Internationalization;
using WebExpress.Core.WebAttribute;
using WebExpress.Core.WebComponent;
using WebExpress.Core.WebPlugin;
using WebExpress.Core.WebUri;

namespace WebExpress.Core.WebApplication
{
    /// <summary>
    /// Management of WebExpress applications.
    /// </summary>
    public sealed class ApplicationManager : IComponentPlugin, IExecutableElements, ISystemComponent
    {
        /// <summary>
        /// An event that fires when an application is added.
        /// </summary>
        public event EventHandler<IApplicationContext> AddApplication;

        /// <summary>
        /// An event that fires when an application is removed.
        /// </summary>
        public event EventHandler<IApplicationContext> RemoveApplication;

        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns or sets the directory where the applications are listed.
        /// </summary>
        private ApplicationDictionary Dictionary { get; } = new ApplicationDictionary();

        /// <summary>
        /// Returns the stored applications.
        /// </summary>
        public IEnumerable<IApplicationContext> Applications => Dictionary.Values.SelectMany(x => x.Values).Select(x => x.ApplicationContext);

        /// <summary>
        /// Constructor
        /// </summary>
        internal ApplicationManager()
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
                InternationalizationManager.I18N("webexpress:applicationmanager.initialization")
            );
        }

        /// <summary>
        /// Discovers and registers applications from the specified plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose applications are to be registered.</param>
        public void Register(IPluginContext pluginContext)
        {
            // the plugin has already been registered
            if (Dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            Dictionary.Add(pluginContext, new Dictionary<string, ApplicationItem>());

            var assembly = pluginContext.Assembly;
            var pluginDict = Dictionary[pluginContext];

            foreach (var type in assembly.GetExportedTypes().Where
                (
                    x => x.IsClass &&
                    x.IsSealed &&
                    x.IsPublic &&
                    x.GetInterface(typeof(IApplication).Name) != null
                ))
            {
                var id = type.FullName?.ToLower();
                var name = type.Name;
                var icon = string.Empty;
                var description = string.Empty;
                var contextPath = string.Empty;
                var assetPath = Path.DirectorySeparatorChar.ToString();
                var dataPath = Path.DirectorySeparatorChar.ToString();
                var options = new List<string>();

                // determining attributes
                foreach (var customAttribute in type.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IApplicationAttribute))))
                {
                    if (customAttribute.AttributeType == typeof(NameAttribute))
                    {
                        name = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(IconAttribute))
                    {
                        icon = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(DescriptionAttribute))
                    {
                        description = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(ContextPathAttribute))
                    {
                        contextPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(AssetPathAttribute))
                    {
                        assetPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(DataPathAttribute))
                    {
                        dataPath = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(OptionAttribute))
                    {
                        var value = customAttribute.ConstructorArguments.FirstOrDefault().Value.ToString().ToLower().Trim();
                        options.Add(value);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(WebExOptionAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(WebExOptionAttribute<>).Namespace)
                    {
                        var value = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower();
                        options.Add(value);
                    }
                    else if (customAttribute.AttributeType.Name == typeof(WebExOptionAttribute<,>).Name && customAttribute.AttributeType.Namespace == typeof(WebExOptionAttribute<,>).Namespace)
                    {
                        var firstValue = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower();
                        var secoundValue = customAttribute.AttributeType.GenericTypeArguments.LastOrDefault()?.FullName?.ToLower();

                        options.Add($"{firstValue}.{secoundValue}");
                    }
                }

                // creating application context
                var applicationContext = new ApplicationContext
                {
                    PluginContext = pluginContext,
                    ApplicationId = id,
                    ApplicationName = name,
                    Description = description,
                    Options = options,
                    AssetPath = Path.Combine(HttpServerContext.AssetPath, assetPath),
                    DataPath = Path.Combine(HttpServerContext.DataPath, dataPath),
                    Icon = UriResource.Combine(HttpServerContext.ContextPath, contextPath, icon),
                    ContextPath = UriResource.Combine(HttpServerContext.ContextPath, contextPath)
                };

                // create application
                var applicationInstance = Activator.CreateInstance(type) as IApplication;

                if (!pluginDict.ContainsKey(id))
                {
                    pluginDict.Add(id, new ApplicationItem()
                    {
                        ApplicationContext = applicationContext,
                        Application = applicationInstance
                    });

                    HttpServerContext.Log.Debug
                    (
                        InternationalizationManager.I18N("webexpress:applicationmanager.register", id)
                    );

                    // raises the AddApplication event
                    OnAddApplication(applicationContext);
                }
                else
                {
                    HttpServerContext.Log.Warning
                    (
                        InternationalizationManager.I18N("webexpress:applicationmanager.duplicate", id)
                    );
                }
            }
        }

        /// <summary>
        /// Discovers and registers applications from the specified plugin.
        /// </summary>
        /// <param name="pluginContexts">A list with plugin contexts that contain the applications.</param>
        public void Register(IEnumerable<IPluginContext> pluginContexts)
        {
            foreach (var pluginContext in pluginContexts)
            {
                Register(pluginContext);
            }
        }

        /// <summary>
        /// Determines the application contexts for a given application id.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <returns>The context of the application or null.</returns>
        public IApplicationContext GetApplcation(string applicationId)
        {
            if (string.IsNullOrWhiteSpace(applicationId)) return null;

            var items = Dictionary.Values
                .Where(x => x.ContainsKey(applicationId.ToLower()))
                .Select(x => x[applicationId.ToLower()])
                .FirstOrDefault();

            if (items != null)
            {
                return items.ApplicationContext;
            }

            return null;
        }

        /// <summary>
        /// Determines the application contexts for the given application ids.
        /// </summary>
        /// <param name="applicationIds">The applications ids. Can contain regular expressions or * for all.</param>
        /// <returns>The contexts of the applications as an enumeration.</returns>
        public IEnumerable<IApplicationContext> GetApplcations(IEnumerable<string> applicationIds)
        {
            var list = new List<IApplicationContext>();

            foreach (var applicationId in applicationIds)
            {
                if (applicationId == "*")
                {
                    list.AddRange(Applications);
                }
                else
                {
                    list.AddRange
                    (
                        Applications.Where
                        (
                            x =>
                            x.ApplicationId.Equals(applicationId, StringComparison.OrdinalIgnoreCase) ||
                            Regex.Match(x.ApplicationId, applicationId).Success
                        )
                    );
                }
            }

            return list.Distinct();
        }

        /// <summary>
        /// Determines the application contexts for the given plugin.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <returns>The contexts of the applications as an enumeration.</returns>
        public IEnumerable<IApplicationContext> GetApplcations(IPluginContext pluginContext)
        {
            if (!Dictionary.ContainsKey(pluginContext))
            {
                return new List<IApplicationContext>();
            }

            return Dictionary[pluginContext].Values.Select(x => x.ApplicationContext);
        }

        /// <summary>
        /// Boots the applications.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the applications.</param>
        public void Boot(IPluginContext pluginContext)
        {
            if (pluginContext == null)
            {
                return;
            }

            if (!Dictionary.ContainsKey(pluginContext))
            {
                HttpServerContext.Log.Warning
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:applicationmanager.application.boot.notfound",
                        pluginContext.PluginId
                    )
                );

                return;
            }

            foreach (var applicationItem in Dictionary[pluginContext]?.Values ?? Enumerable.Empty<ApplicationItem>())
            {
                var token = applicationItem.CancellationTokenSource.Token;

                // Initialize application
                applicationItem.Application.Initialization(applicationItem.ApplicationContext);
                HttpServerContext.Log.Debug
                (
                    InternationalizationManager.I18N
                    (
                        "webexpress:applicationmanager.application.initialization",
                        applicationItem.ApplicationContext.ApplicationId
                    )
                );

                // Run the application concurrently
                Task.Run(() =>
                {
                    HttpServerContext.Log.Debug
                    (
                        InternationalizationManager.I18N
                        (
                            "webexpress:applicationmanager.application.processing.start",
                            applicationItem.ApplicationContext.ApplicationId)
                        );

                    applicationItem.Application.Run();

                    HttpServerContext.Log.Debug
                    (
                        InternationalizationManager.I18N
                        (
                            "webexpress:applicationmanager.application.processing.end",
                            applicationItem.ApplicationContext.ApplicationId
                        )
                    );

                    token.ThrowIfCancellationRequested();
                }, token);
            }
        }

        /// <summary>
        /// Shutting down applications.
        /// </summary>
        ///  <param name="pluginContext">The context of the plugin that contains the applications.</param>
        public void ShutDown(IPluginContext pluginContext)
        {
            foreach (var applicationItem in Dictionary[pluginContext]?.Values ?? Enumerable.Empty<ApplicationItem>())
            {
                applicationItem.CancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// Removes all applications associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the applications to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            if (!Dictionary.ContainsKey(pluginContext))
            {
                return;
            }

            foreach (var applicationContext in Dictionary[pluginContext])
            {
                OnRemoveApplication(applicationContext.Value.ApplicationContext);
            }

            Dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Raises the AddApplication event.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        private void OnAddApplication(IApplicationContext applicationContext)
        {
            AddApplication?.Invoke(this, applicationContext);
        }

        /// <summary>
        /// Raises the RemoveApplication event.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        private void OnRemoveApplication(IApplicationContext applicationContext)
        {
            RemoveApplication?.Invoke(this, applicationContext);
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
            foreach (var applicationContext in GetApplcations(pluginContext))
            {
                output.Add
                (
                    string.Empty.PadRight(deep) +
                    InternationalizationManager.I18N("webexpress:applicationmanager.application", applicationContext.ApplicationId)
                );
            }
        }
    }
}
