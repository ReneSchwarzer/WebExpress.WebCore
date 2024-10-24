using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebResource.Model
{
    /// <summary>
    /// Represents a dictionary that maps plugin contexts to application contexts, resource types, and resource items.
    /// key = plugin context
    /// value = { key = resource type, value = ressource item }
    /// </summary>
    internal class ResourceDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, Dictionary<Type, ResourceItem>>>
    {
        /// <summary>
        /// Adds a resource item to the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="resourceItem">The resource item.</param>
        /// <returns>True if the resource item was added successfully, false if an element with the same status code already exists.</returns>
        public bool AddResourceItem(IPluginContext pluginContext, IApplicationContext applicationContext, ResourceItem resourceItem)
        {
            var type = resourceItem.ResourceClass;

            if (!typeof(IResource).IsAssignableFrom(type))
            {
                return false;
            }

            if (!ContainsKey(pluginContext))
            {
                this[pluginContext] = new Dictionary<IApplicationContext, Dictionary<Type, ResourceItem>>();
            }

            var appContextDict = this[pluginContext];

            if (!appContextDict.ContainsKey(applicationContext))
            {
                appContextDict[applicationContext] = new Dictionary<Type, ResourceItem>();
            }

            var resourceDict = appContextDict[applicationContext];

            if (!resourceDict.ContainsKey(type))
            {
                resourceDict[type] = resourceItem;
                return true;
            }

            return false; // item with the same resource class already exists
        }

        /// <summary>
        /// Removes a resource from the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        public void RemoveResource<T>(IPluginContext pluginContext, IApplicationContext applicationContext) where T : IResource
        {
            var type = typeof(T);

            if (ContainsKey(pluginContext))
            {
                var appContextDict = this[pluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var resourceDict = appContextDict[applicationContext];

                    if (resourceDict.ContainsKey(type))
                    {
                        resourceDict.Remove(type);

                        if (resourceDict.Count == 0)
                        {
                            appContextDict.Remove(applicationContext);

                            if (appContextDict.Count == 0)
                            {
                                Remove(pluginContext);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the resource items from the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of resource.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of resource items</returns>
        public IEnumerable<ResourceItem> GetResourceItems<T>(IApplicationContext applicationContext) where T : IResource
        {
            return GetResourceItems(applicationContext, typeof(T));
        }

        /// <summary>
        /// Returns the resource items from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <typeparam name="resourceType">The type of resource.</typeparam>
        /// <returns>An IEnumerable of resource items</returns>
        public IEnumerable<ResourceItem> GetResourceItems(IApplicationContext applicationContext, Type resourceType)
        {
            if (!typeof(IResource).IsAssignableFrom(resourceType))
            {
                return Enumerable.Empty<ResourceItem>();
            }

            if (ContainsKey(applicationContext?.PluginContext))
            {
                var appContextDict = this[applicationContext?.PluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var resourceDict = appContextDict[applicationContext];

                    if (resourceDict.ContainsKey(resourceType))
                    {
                        return new List<ResourceItem> { resourceDict[resourceType] };
                    }
                }
            }

            return [];
        }

        /// <summary>
        /// Returns all resource contexts for a given plugin context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <returns>An IEnumerable of resource contexts.</returns>
        public IEnumerable<IResourceContext> GetResources(IPluginContext pluginContext)
        {
            return this.Where(entry => entry.Key == pluginContext)
                       .SelectMany(entry => entry.Value.Values)
                       .SelectMany(dict => dict.Values)
                       .Select(x => x.ContextPath as IResourceContext);
        }
    }
}
