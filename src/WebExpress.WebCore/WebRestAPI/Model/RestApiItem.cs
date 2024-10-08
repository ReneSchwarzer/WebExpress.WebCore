using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebLog;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebRestApi.Model
{
    /// <summary>
    /// A rest api resource that contains meta information about a rest api resource.
    /// </summary>
    internal class RestApiItem : IDisposable
    {
        private readonly IRestApiManager _restApiManager;

        /// <summary>
        /// An event that fires when an rest api resource is added.
        /// </summary>
        public event EventHandler<IRestApiContext> AddRestApi;

        /// <summary>
        /// An event that fires when an rest api resource is removed.
        /// </summary>
        public event EventHandler<IRestApiContext> RemoveRestApi;

        /// <summary>
        /// Returns or sets the rest api resource id.
        /// </summary>
        public string RestApiId { get; set; }

        /// <summary>
        /// Returns or sets the parent id.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Returns or sets the type of rest api resource.
        /// </summary>
        public Type RestApiClass { get; set; }

        /// <summary>
        /// Returns or sets the instance of the rest api resource, if the rest api resource is cached, otherwise null.
        /// </summary>
        public IRestApi Instance { get; set; }

        /// <summary>
        /// Returns or sets the module id.
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// Returns or sets the paths of the resource.
        /// </summary>
        public UriResource ContextPath { get; set; }

        /// <summary>
        /// Returns or sets the path segment.
        /// </summary>
        public IUriPathSegment PathSegment { get; internal set; }

        /// <summary>
        /// Returns or sets whether all subpaths should be taken into sitemap.
        /// </summary>
        public bool IncludeSubPaths { get; set; }

        /// <summary>
        /// Returns the conditions that must be met for the rest api resource to be active.
        /// </summary>
        public IEnumerable<ICondition> Conditions { get; set; }

        /// <summary>
        /// Returns the crud methods.
        /// </summary>
        public IEnumerable<CrudMethod> Methods { get; set; }

        /// <summary>
        /// Returns the version number of the rest api.
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Returns whether it is a optional rest api resource.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Returns the log to write status messages to the console and to a log file.
        /// </summary>
        public ILog Log { get; internal set; }

        /// <summary>
        /// Returns the directory where the module instances are listed.
        /// </summary>
        private IDictionary<IModuleContext, IRestApiContext> Dictionary { get; }
            = new Dictionary<IModuleContext, IRestApiContext>();

        /// <summary>
        /// Returns the associated module contexts.
        /// </summary>
        public IEnumerable<IModuleContext> ModuleContexts => Dictionary.Keys;

        /// <summary>
        /// Returns the rest api contexts.
        /// </summary>
        public IEnumerable<IRestApiContext> RestApiContexts => Dictionary.Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestApiItem"/> class.
        /// </summary>
        /// <param name="restApiManager">The rest api manager.</param>
        internal RestApiItem(IRestApiManager restApiManager)
        {
            _restApiManager = restApiManager;
        }

        /// <summary>
        /// Adds an module assignment
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        public void AddModule(IModuleContext moduleContext)
        {
            // only if no instance has been created yet
            if (Dictionary.ContainsKey(moduleContext))
            {
                Log.Warning(message: I18N.Translate("webexpress:restapimanager.addresource.duplicate", RestApiId, moduleContext.ModuleId));

                return;
            }

            // create context
            var restApiContext = new RestApiContext(moduleContext, _restApiManager, this)
            {
                Conditions = Conditions,
                EndpointId = RestApiId,
                Cache = Cache,
                IncludeSubPaths = IncludeSubPaths,
                Methods = Methods,
                Version = Version
            };

            if
            (
                !Optional ||
                moduleContext.ApplicationContext.Options.Contains($"{ModuleId.ToLower()}.{RestApiId.ToLower()}") ||
                moduleContext.ApplicationContext.Options.Contains($"{ModuleId.ToLower()}.*") ||
                moduleContext.ApplicationContext.Options.Where(x => Regex.Match($"{ModuleId.ToLower()}.{RestApiId.ToLower()}", x).Success).Any()
            )
            {
                Dictionary.Add(moduleContext, restApiContext);
                OnAddRestApi(restApiContext);
            }
        }

        /// <summary>
        /// Remove an module assignment.
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        public void DetachModule(IModuleContext moduleContext)
        {
            // not an assignment has been created yet
            if (!Dictionary.ContainsKey(moduleContext))
            {
                return;
            }

            foreach (var restApiContext in Dictionary.Values)
            {
                OnRemovePage(restApiContext);
            }

            Dictionary.Remove(moduleContext);
        }

        /// <summary>
        /// Checks whether a module context is already assigned to the item.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        /// <returns>True a mapping exists, false otherwise.</returns>
        public bool IsAssociatedWithModule(IModuleContext moduleContext)
        {
            return Dictionary.ContainsKey(moduleContext);
        }

        /// <summary>
        /// Raises the AddRestApi event.
        /// </summary>
        /// <param name="restApiContext">The rest api context.</param>
        private void OnAddRestApi(IRestApiContext restApiContext)
        {
            AddRestApi?.Invoke(this, restApiContext);
        }

        /// <summary>
        /// Raises the RemoveRestApi event.
        /// </summary>
        /// <param name="restApiContext">The rest api context.</param>
        private void OnRemovePage(IRestApiContext restApiContext)
        {
            RemoveRestApi?.Invoke(this, restApiContext);
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged rest api resources.
        /// </summary>
        public void Dispose()
        {
            foreach (Delegate d in AddRestApi.GetInvocationList())
            {
                AddRestApi -= (EventHandler<IRestApiContext>)d;
            }

            foreach (Delegate d in RemoveRestApi.GetInvocationList())
            {
                RemoveRestApi -= (EventHandler<IRestApiContext>)d;
            }
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The rest api resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"RestApi '{RestApiId}'";
        }
    }
}
