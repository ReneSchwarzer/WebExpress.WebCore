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
        private readonly Dictionary<IPluginContext, Dictionary<IApplicationContext, List<SettingPageItem>>> _dict = [];

        /// <summary>
        /// Returns the collection of setting pages.
        /// </summary>
        public IEnumerable<ISettingPageContext> All => _dict.Values
            .SelectMany(a => a.Values)
            .SelectMany(x => x)
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

            if (!appDict.TryGetValue(item.ApplicationContext, out var list))
            {
                list = [];
                appDict.Add(item.ApplicationContext, list);
            }

            list.Add(item);

            return true;
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

                foreach (var page in removed)
                {
                    yield return page.SettingPageContext;
                }
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
                .SelectMany(x => x)
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
                .SelectMany(x => x.Value)
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
                .SelectMany(x => x)
                .FirstOrDefault(x => x.SettingPageContext.Equals(settingPageContext));

            if (settingPageItem is not null && settingPageItem.Instance is null)
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
        /// Returns an enumeration of setting page contexts for the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="categoryContext">The category for which to retrieve setting pages.</param>
        /// <returns>An enumeration of setting page contexts.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(IApplicationContext applicationContext, ISettingCategoryContext categoryContext)
        {
            return _dict.Values
                 .SelectMany(a => a)
                 .Where(a => a.Key == applicationContext)
                 .SelectMany(x => x.Value)
                 .Where(x => x.SettingPageContext?.SettingCategory == categoryContext)
                 .Select(x => x.SettingPageContext);
        }

        /// <summary>
        /// Returns an enumeration of setting page contexts for the specified application context, category, and group.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="groupContext">The group for which to retrieve setting pages.</param>
        /// <returns>An enumeration of setting page contexts.</returns>
        public IEnumerable<ISettingPageContext> GetSettingPages(IApplicationContext applicationContext, ISettingGroupContext groupContext)
        {
            return _dict.Values
                 .SelectMany(a => a)
                 .Where(a => a.Key == applicationContext)
                 .SelectMany(x => x.Value)
                 .Where(x => x.SettingPageContext?.SettingGroup == groupContext)
                 .Select(x => x.SettingPageContext);
        }
    }
}
