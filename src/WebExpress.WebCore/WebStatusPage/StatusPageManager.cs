using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebStatusPage.Model;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebStatusPage
{
    /// <summary>
    /// Management of status pages.
    /// </summary>
    public class StatusPageManager : IStatusPageManager, IComponentManagerPlugin, ISystemComponent
    {
        private readonly IComponentHub _componentHub;
        private readonly IHttpServerContext _httpServerContext;
        private readonly StatusPageDictionary _dictionary = [];
        private readonly Dictionary<int, StatusPageItem> _defaults = [];

        /// <summary>
        /// An event that fires when an status page is added.
        /// </summary>
        public event EventHandler<IStatusPageContext> AddStatusPage;

        /// <summary>
        /// An event that fires when an status page is removed.
        /// </summary>
        public event EventHandler<IStatusPageContext> RemoveStatusPage;

        /// <summary>
        /// Returns all status pages.
        /// </summary>
        public IEnumerable<IStatusPageContext> StatusPages => _dictionary.Values
            .SelectMany(x => x.Values)
            .SelectMany(x => x.Values)
            .Select(x => x.StatusPageContext);

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="componentManager">The component manager.</param>
        /// <param name="httpServerContext">The reference to the context of the host.</param>
        internal StatusPageManager(IComponentHub componentManager, IHttpServerContext httpServerContext)
        {
            _componentHub = componentManager;

            _componentHub.PluginManager.AddPlugin += (sender, pluginContext) =>
            {
                Register(pluginContext);
            };

            _componentHub.PluginManager.RemovePlugin += (sender, pluginContext) =>
            {
                Remove(pluginContext);
            };

            _httpServerContext = httpServerContext;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:statuspagemanager.initialization")
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
                var id = resource.FullName?.ToLower();
                var applicationIds = new List<string>();
                var statusResponse = typeof(ResponseInternalServerError);
                var icon = string.Empty;
                var title = resource.Name;
                var defaultItem = false;

                foreach (var customAttribute in resource.CustomAttributes
                    .Where(x => x.AttributeType.GetInterfaces().Contains(typeof(IStatusPageAttribute))))
                {
                    if (customAttribute.AttributeType.Name == typeof(StatusResponseAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(StatusResponseAttribute<>).Namespace)
                    {
                        statusResponse = customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault();
                    }
                    else if (customAttribute.AttributeType.Name == typeof(ApplicationAttribute<>).Name && customAttribute.AttributeType.Namespace == typeof(ApplicationAttribute<>).Namespace)
                    {
                        applicationIds.Add(customAttribute.AttributeType.GenericTypeArguments.FirstOrDefault()?.FullName?.ToLower());
                    }
                    else if (customAttribute.AttributeType == typeof(TitleAttribute))
                    {
                        title = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(IconAttribute))
                    {
                        icon = customAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    }
                    else if (customAttribute.AttributeType == typeof(DefaultAttribute))
                    {
                        defaultItem = true;
                    }
                }

                if (!applicationIds.Any())
                {
                    // no application specified
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate("webexpress:statuspagemanager.applicationless", id)
                    );

                    break;
                }

                if (applicationIds.Count() > 1)
                {
                    // too many specified applications
                    _httpServerContext.Log.Warning
                    (
                        I18N.Translate("webexpress:statuspagemanager.applicationrich", id)
                    );
                }

                // assign the module to existing applications.
                var applicationContext = _componentHub.ApplicationManager.GetApplication(applicationIds.FirstOrDefault());

                if (statusResponse != default)
                {
                    var stausIcon = !string.IsNullOrEmpty(icon) ? UriResource.Combine(applicationContext.ContextPath, icon) : null;
                    var statusCode = statusResponse.GetCustomAttribute<StatusCodeAttribute>().StatusCode;
                    var statusPageContext = new StatusPageContext()
                    {
                        StatusId = id,
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        StatusCode = statusCode,
                        StatusTitle = title,
                        StatusIcon = stausIcon
                    };

                    if (_dictionary.AddStatusPageItem(pluginContext, applicationContext, statusCode, new StatusPageItem()
                    {
                        StatusPageId = id,
                        StatusPageContext = statusPageContext,
                        PluginContext = pluginContext,
                        ApplicationContext = applicationContext,
                        StatusResponse = statusResponse,
                        StatusPageClass = resource
                    }))
                    {
                        OnAddStatusPage(statusPageContext);

                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:statuspagemanager.register",
                                statusResponse,
                                resource.Name
                            )
                        );
                    }
                    else
                    {
                        _httpServerContext.Log.Debug
                        (
                            I18N.Translate
                            (
                                "webexpress:statuspagemanager.duplicat",
                                statusResponse,
                                resource.Name
                            )
                        );
                    }

                    // default
                    if (!_defaults.ContainsKey(statusCode))
                    {
                        _defaults.Add(statusCode, new StatusPageItem()
                        {
                            StatusPageId = id,
                            StatusPageContext = new StatusPageContext()
                            {
                                StatusId = id,
                                PluginContext = pluginContext,
                                ApplicationContext = applicationContext,
                                StatusCode = statusCode,
                                StatusTitle = title,
                                StatusIcon = stausIcon
                            },
                            StatusPageClass = resource,
                            StatusResponse = statusResponse,
                            PluginContext = pluginContext
                        });
                    }
                    else if (defaultItem)
                    {
                        _defaults[statusCode] = new StatusPageItem()
                        {
                            StatusPageId = id,
                            StatusPageContext = new StatusPageContext()
                            {
                                StatusId = id,
                                PluginContext = pluginContext,
                                ApplicationContext = applicationContext,
                                StatusCode = statusCode,
                                StatusTitle = title,
                                StatusIcon = stausIcon
                            },
                            StatusPageClass = resource,
                            StatusResponse = statusResponse,
                            PluginContext = pluginContext,
                        };
                    }

                }
                else
                {
                    _httpServerContext.Log.Debug
                    (
                        I18N.Translate
                        (
                            "webexpress:statuspagemanager.statuscode",
                            resource.Name,
                            applicationIds.FirstOrDefault()
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
        /// Determines the status page for a given application context and status type.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="statusPageClass">The status page class.</param>
        /// <returns>The context of the status page or null.</returns>
        public IStatusPageContext GetStatusPage(IApplicationContext applicationContext, Type statusPageClass)
        {
            var item = _dictionary.GetStatusPageItem(applicationContext, statusPageClass);

            return item?.StatusPageContext;
        }

        /// <summary>
        /// Creates a status response.
        /// </summary>
        /// <param name="message">The status message.</param>
        /// <param name="status">The status code.</param>
        /// <param name="applicationContext">The application context where the status pages are located or null for an undefined page (may be from another application) that matches the status code.</param>
        /// <param name="request">The request.</param>
        /// <returns>The response or null.</returns>
        public Response CreateStatusResponse(string message, int status, IApplicationContext applicationContext, Request request)
        {
            var statusPageItem = _dictionary.GetStatusPageItem(applicationContext, status);

            if (statusPageItem == null && _defaults.TryGetValue(status, out StatusPageItem value))
            {
                statusPageItem = value;
            }

            if (statusPageItem == null)
            {
                switch (status)
                {
                    case 400:
                        return new ResponseBadRequest(!string.IsNullOrWhiteSpace(message) ? new StatusMessage(message) : null);
                    case 401:
                        return new ResponseUnauthorized(!string.IsNullOrWhiteSpace(message) ? new StatusMessage(message) : null);
                    case 404:
                        return new ResponseNotFound(!string.IsNullOrWhiteSpace(message) ? new StatusMessage(message) : null);
                    case 500:
                        return new ResponseInternalServerError(!string.IsNullOrWhiteSpace(message) ? new StatusMessage(message) : null);
                    default:
                        return new ResponseInternalServerError(!string.IsNullOrWhiteSpace(message) ? new StatusMessage(message) : null);
                }
            }

            var instance = ComponentActivator.CreateInstance<IStatusPage, IStatusPageContext>
            (
                statusPageItem.StatusPageClass,
                statusPageItem.StatusPageContext,
                _componentHub,
                new StatusMessage(message)
            );
            var type = instance.GetType();
            var renderContext = default(IRenderContext);

            if (type.IsGenericType)
            {
                var typeOfT = type.GetGenericArguments()[0];
                var parameters = new object[] { statusPageItem.PluginContext, request, new List<string>() };

                renderContext = Activator.CreateInstance(typeOfT, parameters) as IRenderContext;
            }
            else
            {
                renderContext = new RenderContext();
            }

            instance.Process(renderContext);


            var response = ComponentActivator.CreateInstance<Response>(statusPageItem.StatusResponse, _componentHub, new StatusMessage(message));
            var content = renderContext.VisualTree.Render(new VisualTreeContext(request))?.ToString();

            response.Content = content;
            response.Header.ContentLength = content?.Length ?? 0;

            return response;
        }

        /// <summary>
        /// Removes all status pages associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the status pages to remove.</param>
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

            _dictionary.Remove(pluginContext);
        }

        /// <summary>
        /// Raises the AddStatusPage event.
        /// </summary>
        /// <param name="statusPage">The status page.</param>
        private void OnAddStatusPage(IStatusPageContext statusPage)
        {
            AddStatusPage?.Invoke(this, statusPage);
        }

        /// <summary>
        /// Raises the RemoveComponent event.
        /// </summary>
        /// <param name="statusPage">The status page.</param>
        private void OnRemoveStatusPage(IStatusPageContext statusPage)
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
            foreach (var statusCode in _dictionary.GetStatusPageContexts(pluginContext).Select(x => x.StatusCode))
            {
                output.Add
                (
                    string.Empty.PadRight(4) +
                    I18N.Translate
                    (
                        "webexpress:statuspagemanager.statuspage",
                        statusCode
                    )
                );
            }
        }
    }
}
