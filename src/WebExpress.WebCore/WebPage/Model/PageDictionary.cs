using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebPage.Model
{
    /// <summary>
    /// Represents a dictionary that maps plugin contexts to application contexts, page types, and page items.
    /// key = plugin context
    /// value = application context { key = page type, value = page item }
    /// </summary>
    internal class PageDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, Dictionary<Type, PageItem>>>
    {
        /// <summary>
        /// Adds a page item to the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="pageItem">The page item.</param>
        /// <returns>True if the page item was added successfully, false if an element with the same status code already exists.</returns>
        public bool AddPageItem(IPluginContext pluginContext, IApplicationContext applicationContext, PageItem pageItem)
        {
            var type = pageItem.PageClass;

            if (type.GetInterface(typeof(IPage<>).Name) == null)
            {
                return false;
            }

            if (!ContainsKey(pluginContext))
            {
                this[pluginContext] = [];
            }

            var appContextDict = this[pluginContext];

            if (!appContextDict.ContainsKey(applicationContext))
            {
                appContextDict[applicationContext] = [];
            }

            var pageDict = appContextDict[applicationContext];

            if (!pageDict.ContainsKey(type))
            {
                pageDict[type] = pageItem;
                return true;
            }

            return false; // item with the same page class already exists
        }

        /// <summary>
        /// Removes a page from the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        public void RemovePage<T>(IPluginContext pluginContext, IApplicationContext applicationContext) where T : IPage
        {
            var type = typeof(T);

            if (ContainsKey(pluginContext))
            {
                var appContextDict = this[pluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var pageDict = appContextDict[applicationContext];

                    if (pageDict.ContainsKey(type))
                    {
                        pageDict.Remove(type);

                        if (pageDict.Count == 0)
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
        /// Returns the page items from the dictionary.
        /// </summary>
        /// <typeparam name="T">The type of page.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of page items</returns>
        public IEnumerable<PageItem> GetPageItems<T>(IApplicationContext applicationContext) where T : IPage
        {
            return GetPageItems(applicationContext, typeof(T));
        }

        /// <summary>
        /// Returns the page items from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <typeparam name="pageType">The type of page.</typeparam>
        /// <returns>An IEnumerable of page items</returns>
        public IEnumerable<PageItem> GetPageItems(IApplicationContext applicationContext, Type pageType)
        {
            if (!typeof(IPage).IsAssignableFrom(pageType))
            {
                return [];
            }

            if (ContainsKey(applicationContext?.PluginContext))
            {
                var appContextDict = this[applicationContext?.PluginContext];

                if (appContextDict.ContainsKey(applicationContext))
                {
                    var pageDict = appContextDict[applicationContext];

                    if (pageDict.ContainsKey(pageType))
                    {
                        return [pageDict[pageType]];
                    }
                }
            }

            return [];
        }
    }
}
