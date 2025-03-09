using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents a dictionary for managing setting groups, organized by plugin and application contexts.
    /// </summary>
    internal class SettingGroupDictionary
    {
        private readonly Dictionary<IPluginContext, Dictionary<IApplicationContext, List<SettingGroupItem>>> _dict = new();

        /// <summary>
        /// Returns the collection of setting groups.
        /// </summary>
        public IEnumerable<ISettingGroupContext> All => _dict.Values
            .SelectMany(a => a.Values)
            .SelectMany(x => x)
            .Select(x => x.SettingGroupContext);

        /// <summary>
        /// Adds a settings group.
        /// </summary>
        /// <param name="item">The settings group to insert.</param>
        /// <returns>True if the settings group was added successfully; otherwise, false.</returns>
        public bool AddSettingGroupItem(SettingGroupItem item)
        {
            if (!_dict.TryGetValue(item.PluginContext, out var appDict))
            {
                appDict = [];
                _dict.Add(item.PluginContext, appDict);
            }

            if (!appDict.TryGetValue(item.ApplicationContext, out var list))
            {
                list = [];
                appDict.Add(item.ApplicationContext, list);
            }

            list.Add(item);

            return true;
        }

        /// <summary>
        /// Removes all elements associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the elements to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            _dict.Remove(pluginContext);
        }

        /// <summary>
        /// Removes all setting groups associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the fragments to remove.</param>
        /// <returns>An enumerable collection of setting groups contexts that were removed.</returns>
        public IEnumerable<ISettingGroupContext> Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                yield break;
            }

            foreach (var pluginDict in _dict.Values)
            {
                var removed = pluginDict
                    .Where(x => x.Key == applicationContext)
                    .SelectMany(x => x.Value)
                    .ToList();

                pluginDict.Remove(applicationContext);

                foreach (var category in removed)
                {
                    yield return category.SettingGroupContext;
                }
            }
        }

        /// <summary>
        /// Returns the setting group context for the specified application context and group type.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An enumeration of category contexts associated with the specified application context.</returns>
        public IEnumerable<ISettingGroupContext> GetSettingGroups(IApplicationContext applicationContext)
        {
            return _dict.Values
                .SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(x => x.Value)
                .Select(x => x.SettingGroupContext);
        }

        /// <summary>
        /// Returns the setting group context for the specified application context and group type.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="categoryContext">The category context.</param>
        /// <returns>An enumeration of category contexts associated with the specified application context.</returns>
        public IEnumerable<ISettingGroupContext> GetSettingGroups(IApplicationContext applicationContext, ISettingCategoryContext categoryContext)
        {
            return _dict.Values
                .SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(x => x.Value)
                .Where(x => x.SettingGroupContext.SettingCategory == categoryContext)
                .Select(x => x.SettingGroupContext);
        }

        /// <summary>
        /// Returns the setting group context for the specified application context and group type.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="groupType">The type of the group.</param>
        /// <returns>The setting group context if found; otherwise, null.</returns>
        public ISettingGroupContext GetSettingGroup(IApplicationContext applicationContext, Type groupType)
        {
            return _dict.Values
                .SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(x => x.Value)
                .Where(x => x.SettingGroupClass == groupType)
                .Select(x => x.SettingGroupContext)
                .FirstOrDefault();
        }
    }
}
