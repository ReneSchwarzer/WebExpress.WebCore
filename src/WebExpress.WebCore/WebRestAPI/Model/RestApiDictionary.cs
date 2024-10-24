using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebRestApi.Model
{
    /// <summary>
    /// Represents a dictionary that maps plugin contexts to application contexts, rest api types, and rest api items.
    /// key = plugin context
    /// value = { key = rest api id, value = rest api item }
    /// </summary>
    internal class RestApiDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, Dictionary<Type, RestApiItem>>>
    {
        /// <summary>
        /// Adds a rest api item to the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="restApiItem">The rest api item.</param>
        /// <returns>True if the rest api item was added successfully, false if an element with the same status code already exists.</returns>
        public bool AddRestApiItem(IPluginContext pluginContext, IApplicationContext applicationContext, RestApiItem restApiItem)
        {
            var type = restApiItem.RestApiClass;

            if (!typeof(IRestApi).IsAssignableFrom(type))
            {
                return false;
            }

            if (!ContainsKey(pluginContext))
            {
                this[pluginContext] = new Dictionary<IApplicationContext, Dictionary<Type, RestApiItem>>();
            }

            var appContextDict = this[pluginContext];

            if (!appContextDict.ContainsKey(applicationContext))
            {
                appContextDict[applicationContext] = new Dictionary<Type, RestApiItem>();
            }

            var restApiDict = appContextDict[applicationContext];

            if (!restApiDict.ContainsKey(type))
            {
                restApiDict[type] = restApiItem;
                return true;
            }

            return false; // item with the same rest api class already exists
        }

        /// <summary>
        /// Removes a rest api from the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        public void RemoveRestApi<T>(IPluginContext pluginContext, IApplicationContext applicationContext) where T : IRestApi
        {
            var type = typeof(T);

            if (ContainsKey(pluginContext))
            {
                var appContextDict = this[pluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var restApiDict = appContextDict[applicationContext];

                    if (restApiDict.ContainsKey(type))
                    {
                        restApiDict.Remove(type);

                        if (restApiDict.Count == 0)
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
        /// Returns the rest api items from the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of rest api.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of rest api items</returns>
        public IEnumerable<RestApiItem> GetRestApiItems<T>(IApplicationContext applicationContext) where T : IRestApi
        {
            return GetRestApiItems(applicationContext, typeof(T));
        }

        /// <summary>
        /// Returns the rest api items from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <typeparam name="restApiType">The type of rest api.</typeparam>
        /// <returns>An IEnumerable of rest api items</returns>
        public IEnumerable<RestApiItem> GetRestApiItems(IApplicationContext applicationContext, Type restApiType)
        {
            if (!typeof(IRestApi).IsAssignableFrom(restApiType))
            {
                return Enumerable.Empty<RestApiItem>();
            }

            if (ContainsKey(applicationContext?.PluginContext))
            {
                var appContextDict = this[applicationContext?.PluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var restApiDict = appContextDict[applicationContext];

                    if (restApiDict.ContainsKey(restApiType))
                    {
                        return new List<RestApiItem> { restApiDict[restApiType] };
                    }
                }
            }

            return Enumerable.Empty<RestApiItem>();
        }

        /// <summary>
        /// Returns all rest api contexts for a given plugin context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <returns>An IEnumerable of rest api contexts.</returns>
        public IEnumerable<IRestApiContext> GetRestApis(IPluginContext pluginContext)
        {
            return this.Where(entry => entry.Key == pluginContext)
                       .SelectMany(entry => entry.Value.Values)
                       .SelectMany(dict => dict.Values)
                       .Select(x => x.ContextPath as IRestApiContext);
        }
    }
}
