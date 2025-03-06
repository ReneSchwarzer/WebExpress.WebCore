using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents a dictionary for managing setting pages, organized by plugin and application contexts.
    /// </summary>
    internal class SettingPageDictionary
    {
        private readonly Dictionary<IPluginContext, Dictionary<IApplicationContext, SettingPageDictionaryItemCategory>> _dict = [];

        /// <summary>
        /// Returns the collection of setting pages.
        /// </summary>
        public IEnumerable<ISettingPageContext> All => _dict.Values
            .SelectMany(a => a.Values)
            .SelectMany(c => c.Values)
            .SelectMany(s => s.Values)
            .SelectMany(g => g.Values)
            .SelectMany(i => i)
            .Select(x => x.SettingPageContext);

        /// <summary>
        /// Adds a settings page.
        /// </summary>
        /// <param name="item">The settings page to insert.</param>
        /// <returns>True if the settings page was added successfully; otherwise, false.</returns>
        public bool AddSettingPageItem(SettingPageItem item)
        {
            if (!_dict.TryGetValue(item.PluginContext, out var appDict))
            {
                appDict = [];
                _dict.Add(item.PluginContext, appDict);
            }

            if (!appDict.TryGetValue(item.ApplicationContext, out var contextDict))
            {
                contextDict = [];
                appDict.Add(item.ApplicationContext, contextDict);
            }

            return contextDict.AddSettingPageItem(item.Context, item.Section, item.Group, item);
        }

        /// <summary>
        /// Removes all elemets associated with the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin that contains the elemets to remove.</param>
        public void Remove(IPluginContext pluginContext)
        {
            _dict.Remove(pluginContext);
        }

        /// <summary>
        /// Removes all setting pages associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application that contains the fragments to remove.</param>
        /// <returns>An enumerable collection of setting page contexts that were removed.</returns>
        public IEnumerable<ISettingPageContext> Remove(IApplicationContext applicationContext)
        {
            if (applicationContext == null)
            {
                yield break;
            }

            foreach (var pluginDict in _dict.Values)
            {
                foreach (var appDict in pluginDict.Where(x => x.Key == applicationContext).Select(x => x.Value))
                {
                    foreach (var settingPageItem in appDict.Values
                        .SelectMany(s => s.Values)
                        .SelectMany(g => g.Values)
                        .SelectMany(i => i))
                    {
                        yield return settingPageItem.SettingPageContext;
                    }
                }

                pluginDict.Remove(applicationContext);
            }
        }

        /// <summary>
        /// Returns an enumeration of setting page contextes.
        /// </summary>
        /// <param name="settingPageType">The setting page type.</param>
        /// <returns>An enumeration of setting page contextes.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(Type settingPageType)
        {
            return _dict.Values
                .SelectMany(a => a.Values)
                .SelectMany(c => c.Values)
                .SelectMany(s => s.Values)
                .SelectMany(g => g.Values)
                .SelectMany(i => i)
                .Where(x => x.SettingPageClass.Equals(settingPageType))
                .Select(x => x.SettingPageContext);
        }

        /// <summary>
        /// Returns an enumeration of setting page contextes.
        /// </summary>
        /// <param name="settingPageType">The setting page type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of setting page contextes.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(Type settingPageType, IApplicationContext applicationContext)
        {
            return _dict.Values
                .SelectMany(a => a)
                .Where(a => a.Key.Equals(applicationContext))
                .Select(a => a.Value)
                .SelectMany(c => c.Values)
                .SelectMany(s => s.Values)
                .SelectMany(g => g.Values)
                .SelectMany(i => i)
                .Where(x => x.SettingPageClass.Equals(settingPageType))
                .Select(x => x.SettingPageContext);
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified plugin context.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin to locate in the dictionary.</param>
        /// <returns>True if the dictionary contains an element with the specified plugin context; otherwise, false.</returns>
        public bool Contains(IPluginContext pluginContext)
        {
            return _dict.ContainsKey(pluginContext);
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified apllication context.
        /// </summary>
        /// <param name="applicationContext">The context of the application to locate in the dictionary.</param>
        /// <returns>True if the dictionary contains an element with the specified application context; otherwise, false.</returns>
        public bool Contains(IApplicationContext applicationContext)
        {
            return _dict.Values.Any(x => x.ContainsKey(applicationContext));
        }

        /// <summary>
        /// Creates a new setting page instance or returns an existing cached instance.
        /// </summary>
        /// <param name="settingPageContext">The context used for setting page creation.</param>
        /// <param name="componentHub">The central management hub for components.</param>
        /// <param name="httpServerContext">The context of the HTTP server.</param>
        /// <returns>The created or cached setting page instance.</returns>
        public IEndpoint CreateSettingPageInstance(ISettingPageContext settingPageContext, IComponentHub componentHub, IHttpServerContext httpServerContext)
        {
            var settingPageItem = _dict.Values
                .SelectMany(a => a.Values)
                .SelectMany(c => c.Values)
                .SelectMany(s => s.Values)
                .SelectMany(g => g.Values)
                .SelectMany(i => i)
                .FirstOrDefault(x => x.SettingPageContext.Equals(settingPageContext));

            if (settingPageItem != null && settingPageItem.Instance == null)
            {
                var instance = ComponentActivator.CreateInstance<IEndpoint, ISettingPageContext>
                (
                    settingPageItem.SettingPageClass,
                    settingPageContext,
                    httpServerContext,
                    componentHub,
                    settingPageContext.ApplicationContext
                );

                if (settingPageItem.Cache)
                {
                    settingPageItem.Instance = instance;
                }

                return instance;
            }

            return settingPageItem?.Instance;
        }

        /// <summary>
        /// Returns the categories associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of category names.</returns>
        public IEnumerable<string> GetCategories(IApplicationContext applicationContext)
        {
            return _dict.Values
                 .SelectMany(a => a)
                 .Where(a => a.Key == applicationContext)
                 .SelectMany(c => c.Value)
                 .Select(c => c.Key);
        }

        /// <summary>
        /// Returns the groups associated with the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve groups.</param>
        /// <returns>An enumeration of group names.</returns>
        public IEnumerable<string> GetGroups(IApplicationContext applicationContext, string category)
        {
            return _dict.Values
                 .SelectMany(a => a)
                 .Where(a => a.Key == applicationContext)
                 .SelectMany(c => c.Value)
                 .Where(c => c.Key == category)
                 .SelectMany(s => s.Value)
                 .SelectMany(g => g.Value)
                 .Select(g => g.Key);
        }

        /// <summary>
        /// Returns an enumeration of setting page contexts for the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve setting pages.</param>
        /// <returns>An enumeration of setting page contexts.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(IApplicationContext applicationContext, string category)
        {
            return _dict.Values
                 .SelectMany(a => a)
                 .Where(a => a.Key == applicationContext)
                 .SelectMany(c => c.Value)
                 .Where(c => c.Key == category)
                 .SelectMany(s => s.Value)
                 .SelectMany(g => g.Value)
                 .SelectMany(g => g.Value)
                 .Select(x => x.SettingPageContext);
        }

        /// <summary>
        /// Returns an enumeration of setting page contexts for the specified application context, category, and group.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve setting pages.</param>
        /// <param name="group">The group for which to retrieve setting pages.</param>
        /// <returns>An enumeration of setting page contexts.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(IApplicationContext applicationContext, string category, string group)
        {
            return _dict.Values
                 .SelectMany(a => a)
                 .Where(a => a.Key == applicationContext)
                 .SelectMany(c => c.Value)
                 .Where(c => c.Key == category)
                 .SelectMany(s => s.Value)
                 .SelectMany(g => g.Value)
                 .Where(g => g.Key == group)
                 .SelectMany(g => g.Value)
                 .Select(x => x.SettingPageContext);
        }
    }
}
