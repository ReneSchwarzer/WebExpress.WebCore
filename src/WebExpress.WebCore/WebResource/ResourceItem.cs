using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebCondition;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebResource
{
    /// <summary>
    /// A resource element that contains meta information about a resource.
    /// </summary>
    public class ResourceItem : IDisposable
    {
        /// <summary>
        /// An event that fires when an ressource is added.
        /// </summary>
        public event EventHandler<IResourceContext> AddResource;

        /// <summary>
        /// An event that fires when an resource is removed.
        /// </summary>
        public event EventHandler<IResourceContext> RemoveResource;

        /// <summary>
        /// Returns or sets the resource id.
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// Returns or sets the resource title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Returns or sets the parent id.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// Returns or sets the type of resource.
        /// </summary>
        public Type ResourceClass { get; set; }

        /// <summary>
        /// Returns or sets the instance of the resource, if the resource is cached, otherwise null.
        /// </summary>
        public IResource Instance { get; set; }

        /// <summary>
        /// Returns or sets the module id.
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        /// Returns the scope names that provides the resource. The scope name
        /// is a string with a name (e.g. global, admin), which can be used by elements to 
        /// determine whether content and how content should be displayed.
        /// </summary>
        public IReadOnlyList<string> Scopes { get; set; }

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
        /// Returns the conditions that must be met for the resource to be active.
        /// </summary>
        public ICollection<ICondition> Conditions { get; set; }

        /// <summary>
        /// Returns whether the resource is created once and reused each time it is called.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Returns whether it is a optional resource.
        /// </summary>
        public bool Optional { get; set; }

        /// <summary>
        /// Returns the log to write status messages to the console and to a log file.
        /// </summary>
        public Log Log { get; internal set; }

        /// <summary>
        /// Returns the directory where the module instances are listed.
        /// </summary>
        private IDictionary<IModuleContext, IResourceContext> Dictionary { get; }
            = new Dictionary<IModuleContext, IResourceContext>();

        /// <summary>
        /// Returns the associated module contexts.
        /// </summary>
        public IEnumerable<IModuleContext> ModuleContexts => Dictionary.Keys;

        /// <summary>
        /// Returns the resource contexts.
        /// </summary>
        public IEnumerable<IResourceContext> ResourceContexts => Dictionary.Values;

        /// <summary>
        /// Adds an module assignment
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        public void AddModule(IModuleContext moduleContext)
        {
            // only if no instance has been created yet
            if (Dictionary.ContainsKey(moduleContext))
            {
                Log.Warning(message: InternationalizationManager.I18N("webexpress:resourcemanager.addresource.duplicate", ResourceId, moduleContext.ModuleId));

                return;
            }

            // create context
            var resourceContext = new ResourceContext(moduleContext)
            {
                Scopes = Scopes,
                Conditions = Conditions,
                ResourceId = ResourceId,
                ResourceTitle = Title,
                Cache = Cache,
                ResourceItem = this
            };

            if
            (
                !Optional ||
                moduleContext.ApplicationContext.Options.Contains($"{ModuleId.ToLower()}.{ResourceId.ToLower()}") ||
                moduleContext.ApplicationContext.Options.Contains($"{ModuleId.ToLower()}.*") ||
                moduleContext.ApplicationContext.Options.Where(x => Regex.Match($"{ModuleId.ToLower()}.{ResourceId.ToLower()}", x).Success).Any()
            )
            {
                Dictionary.Add(moduleContext, resourceContext);
                OnAddResource(resourceContext);
            }
        }

        /// <summary>
        /// Remove an module assignment
        /// </summary>
        /// <param name="moduleContext">The context of the module.</param>
        public void DetachModule(IModuleContext moduleContext)
        {
            // not an assignment has been created yet
            if (!Dictionary.ContainsKey(moduleContext))
            {
                return;
            }

            foreach (var resourceContext in Dictionary.Values)
            {
                OnRemoveResource(resourceContext);
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
        /// Raises the AddResource event.
        /// </summary>
        /// <param name="resourceContext">The resource context.</param>
        private void OnAddResource(IResourceContext resourceContext)
        {
            AddResource?.Invoke(this, resourceContext);
        }

        /// <summary>
        /// Raises the RemoveResource event.
        /// </summary>
        /// <param name="resourceContext">The resource context.</param>
        private void OnRemoveResource(IResourceContext resourceContext)
        {
            RemoveResource?.Invoke(this, resourceContext);
        }

        /// <summary>
        /// Performs application-specific tasks related to sharing, returning, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (Delegate d in AddResource.GetInvocationList())
            {
                AddResource -= (EventHandler<IResourceContext>)d;
            }

            foreach (Delegate d in RemoveResource.GetInvocationList())
            {
                RemoveResource -= (EventHandler<IResourceContext>)d;
            }
        }

        /// <summary>
        /// Convert the resource element to a string.
        /// </summary>
        /// <returns>The resource element in its string representation.</returns>
        public override string ToString()
        {
            return $"Resource '{ResourceId}'";
        }
    }
}
