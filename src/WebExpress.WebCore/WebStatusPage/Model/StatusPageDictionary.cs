using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebStatusPage.Model
{
    /// <summary>
    /// Represents a dictionary that provides a mapping from plugin contexts to dictionaries of application contexts and status page items.
    /// </summary>
    internal class StatusPageDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, Dictionary<int, StatusPageItem>>>
    {
        /// <summary>
        /// Adds a status page item to the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="statusPageItem">The status page item.</param>
        /// <returns>True if the status page item was added successfully, false if an element with the same status code already exists.</returns>
        public bool AddStatusPageItem(IPluginContext pluginContext, IApplicationContext applicationContext, int statusCode, StatusPageItem statusPageItem)
        {
            if (!ContainsKey(pluginContext))
            {
                this[pluginContext] = [];
            }

            var appContextDict = this[pluginContext];

            if (!appContextDict.ContainsKey(applicationContext))
            {
                appContextDict[applicationContext] = [];
            }

            var statusCodeDict = appContextDict[applicationContext];

            if (statusCodeDict.ContainsKey(statusCode))
            {
                return false; // item with the same status code already exists
            }

            statusCodeDict[statusCode] = statusPageItem;

            return true;
        }

        /// <summary>
        /// Removes a status page item from the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="statusCode">The status code.</param>
        public void RemoveStatusPageItem(IPluginContext pluginContext, IApplicationContext applicationContext, int statusCode)
        {
            if (ContainsKey(pluginContext))
            {
                var appContextDict = this[pluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var statusCodeDict = appContextDict[applicationContext];

                    if (statusCodeDict.ContainsKey(statusCode))
                    {
                        statusCodeDict.Remove(statusCode);

                        if (statusCodeDict.Count == 0)
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
        /// Returns the status page item from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="statusCode">The status code.</param>
        /// <returns>The status page item if found, otherwise null.</returns>
        public StatusPageItem GetStatusPageItem(IApplicationContext applicationContext, int statusCode)
        {
            if (ContainsKey(applicationContext?.PluginContext))
            {
                var appContextDict = this[applicationContext?.PluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var statusCodeDict = appContextDict[applicationContext];

                    if (statusCodeDict.ContainsKey(statusCode))
                    {
                        return statusCodeDict[statusCode];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the status page item based on the application context and status page type.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="statusPageType">The type of the status page.</param>
        /// <returns>The status page item if found, otherwise null.</returns>
        public StatusPageItem GetStatusPageItem(IApplicationContext applicationContext, Type statusPageType)
        {
            return Values
                .SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .Select(x => x.Value)
                .SelectMany(x => x.Values)
                .FirstOrDefault(x => x.StatusPageClass == statusPageType);
        }

        /// <summary>
        /// Returns all StatusPageContexts for a given plugin context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <returns>An IEnumerable of status page contexts.</returns>
        public IEnumerable<StatusPageContext> GetStatusPageContexts(IPluginContext pluginContext)
        {
            return this.Where(entry => entry.Key == pluginContext)
                       .SelectMany(entry => entry.Value.Values)
                       .SelectMany(dict => dict.Values)
                       .Select(item => item.StatusPageContext);
        }
    }
}
