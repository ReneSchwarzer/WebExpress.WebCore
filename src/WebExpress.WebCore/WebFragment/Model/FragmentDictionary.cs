using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebFragment.Model
{
    /// <summary>
    /// Represents a dictionary that maps plugin contexts to application contexts,
    /// which in turn maps to a dictionary of section types and inner maps of scope types and lists of FragmentItem objects.
    /// Plugin -> Application -> Section -> Scope -> FragmentItem
    /// </summary>
    internal class FragmentDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, Dictionary<Type, Dictionary<Type, List<FragmentItem>>>>>
    {
        /// <summary>
        /// Adds a fragment item to the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="fragmentItem">The fragment item.</param>
        /// <returns>True if the fragment item was added successfully, false if an element with the same status code already exists.</returns>
        public bool AddFragmentItem(IPluginContext pluginContext, IApplicationContext applicationContext, FragmentItem fragmentItem)
        {
            var type = fragmentItem.FragmentClass;

            if (type.GetInterface(typeof(IFragment<,>).Name) == null)
            {
                return false;
            }

            if (!TryGetValue(pluginContext, out var applicationDict))
            {
                applicationDict = [];
                this[pluginContext] = applicationDict;
            }

            if (!applicationDict.TryGetValue(applicationContext, out var sectionDict))
            {
                sectionDict = [];
                applicationDict[applicationContext] = sectionDict;
            }

            if (!sectionDict.TryGetValue(fragmentItem.Section, out var scopeDict))
            {
                scopeDict = [];
                sectionDict[fragmentItem.Section] = scopeDict;
            }

            if (!scopeDict.TryGetValue(fragmentItem.Scope, out var itemList))
            {
                itemList = [];
                scopeDict[fragmentItem.Scope] = itemList;
            }

            if (!itemList.Any(x => x.FragmentClass == fragmentItem.FragmentClass))
            {
                itemList.Add(fragmentItem);

                return true;
            }

            return false; // item with the same fragment class already exists
        }

        /// <summary>
        /// Removes fragments from the dictionary.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <returns>An IEnumerable of fragment contexts that were removed.</returns>
        public IEnumerable<IFragmentContext> RemoveFragments(IPluginContext pluginContext)
        {
            var fragments = GetFragments(pluginContext);

            Remove(pluginContext);

            return fragments;
        }

        /// <summary>
        /// Removes fragments from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of fragment contexts that were removed.</returns>
        public IEnumerable<IFragmentContext> RemoveFragments(IApplicationContext applicationContext)
        {
            foreach (var pluginKeyValue in this)
            {
                if (pluginKeyValue.Value.TryGetValue(applicationContext, out var sectionDict))
                {
                    pluginKeyValue.Value.Remove(applicationContext);

                    if (pluginKeyValue.Value.Count == 0)
                    {
                        Remove(pluginKeyValue.Key);
                    }

                    foreach (var item in sectionDict.Values
                        .SelectMany(x => x.Values)
                        .SelectMany(x => x)
                        .Select(x => x.FragmentContext))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the fragment items from the dictionary.
        /// </summary>
        /// <typeparam name="TFragment">The type of fragment.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An IEnumerable of fragment items</returns>
        public IEnumerable<FragmentItem> GetFragmentItems<TFragment>(IApplicationContext applicationContext) where TFragment : IFragmentBase
        {
            return GetFragmentItems(applicationContext, typeof(TFragment));
        }

        /// <summary>
        /// Returns the fragment items from the dictionary.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <typeparam name="fragmentType">The type of fragment.</typeparam>
        /// <returns>An IEnumerable of fragment items</returns>
        public IEnumerable<FragmentItem> GetFragmentItems(IApplicationContext applicationContext, Type fragmentType)
        {
            if (!typeof(IFragment<,>).IsAssignableFrom(fragmentType))
            {
                return [];
            }

            return Values
                .Where(x => x.ContainsKey(applicationContext))
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .SelectMany(x => x.Values)
                .SelectMany(x => x)
                .Where(x => x.FragmentClass == fragmentType);
        }

        /// <summary>
        /// Returns all fragment contexts for a given plugin context.
        /// </summary>
        /// <param name="pluginContext">The plugin context.</param>
        /// <returns>An IEnumerable of fragment contexts.</returns>
        public IEnumerable<IFragmentContext> GetFragments(IPluginContext pluginContext)
        {
            return this.Where(x => x.Key == pluginContext)
                       .SelectMany(x => x.Value.Values)
                       .SelectMany(x => x.Values)
                       .SelectMany(x => x.Values)
                       .SelectMany(x => x)
                       .Select(x => x.FragmentContext);
        }
    }
}
