using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents a dictionary for managing setting categories, organized by plugin and application contexts.
    /// </summary>
    internal class SettingCategoryDictionary
    {
        private readonly Dictionary<IPluginContext, Dictionary<IApplicationContext, List<SettingCategoryItem>>> _dict = [];

        /// <summary>
        /// Gets the collection of setting categories.
        /// </summary>
        public IEnumerable<ISettingCategoryContext> All => _dict.Values
            .SelectMany(a => a.Values)
            .SelectMany(x => x)
            .Select(x => x.SettingCategoryContext);

        /// <summary>
        /// Adds a settings category.
        /// </summary>
        /// <param name="item">The settings category to insert.</param>
        /// <returns>True if the settings category was added successfully; otherwise, false.</returns>
        public bool AddSettingCategoryItem(SettingCategoryItem item)
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
        /// Removes all setting categories associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the fragments to remove.</param>
        /// <returns>An enumerable collection of setting categories contexts that were removed.</returns>
        public IEnumerable<ISettingCategoryContext> Remove(IApplicationContext applicationContext)
        {
            if (applicationContext is null)
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
                    yield return category.SettingCategoryContext;
                }
            }
        }

        /// <summary>
        /// Returns the setting category context for the specified application context.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An enumeration of category contexts associated with the specified application context.</returns>
        public IEnumerable<ISettingCategoryContext> GetSettingCategories(IApplicationContext applicationContext)
        {
            return _dict.Values
                .SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(x => x.Value)
                .Select(x => x.SettingCategoryContext);
        }

        /// <summary>
        /// Returns the setting category context for the specified application context and category type.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="categoryType">The type of the category.</param>
        /// <returns>>The setting category context if found; otherwise, null.</returns>
        public ISettingCategoryContext GetSettingCategory(IApplicationContext applicationContext, Type categoryType)
        {
            return _dict.Values
                .SelectMany(x => x)
                .Where(x => x.Key == applicationContext)
                .SelectMany(x => x.Value)
                .Where(x => x.SettingCategoryClass == categoryType)
                .Select(x => x.SettingCategoryContext)
                .FirstOrDefault();
        }
    }
}
