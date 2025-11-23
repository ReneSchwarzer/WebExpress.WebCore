using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebInclude.Model
{
    /// <summary>
    /// Nested dictionary for includes: Plugin -> Application -> Id -> IncludeItem
    /// </summary>
    internal sealed class IncludeDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, Dictionary<string, IncludeItem>>>
    {
        /// <summary>
        /// Adds an item to the internal collection for the specified plugin and application contexts.
        /// </summary>
        /// <param name="pluginContext">The plugin context associated with the item. Cannot be null.</param>
        /// <param name="applicationContext">The application context associated with the item. Cannot be null.</param>
        /// <param name="includeItem">The item to add. Cannot be null.</param>
        /// <returns>True if the item was successfully added; otherwise, false if an item with the same key already exists in the collection.</returns>
        public bool AddIncludeItem(IPluginContext pluginContext, IApplicationContext applicationContext, IncludeItem includeItem)
        {
            if (pluginContext is null || applicationContext is null || includeItem is null)
            {
                return false;
            }

            if (!TryGetValue(pluginContext, out var appDict))
            {
                appDict = [];
                Add(pluginContext, appDict);
            }

            if (!appDict.TryGetValue(applicationContext, out var includeMap))
            {
                includeMap = new Dictionary<string, IncludeItem>(StringComparer.OrdinalIgnoreCase);
                appDict.Add(applicationContext, includeMap);
            }

            var key = includeItem.IncludeId?.ToString() ?? includeItem.IncludeClass?.FullName?.ToLower() ?? Guid.NewGuid().ToString("N");

            if (!includeMap.ContainsKey(key))
            {
                includeMap[key] = includeItem;
                return true;
            }

            return false;
        }
    }
}
