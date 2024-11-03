using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents a dictionary that maps plugin contexts to application contexts and their corresponding setting page dictionary item contexts.
    /// </summary>
    internal class SettingPageDictionary : Dictionary<IPluginContext, Dictionary<IApplicationContext, SettingPageDictionaryItemContext>>
    {
        /// <summary>
        /// Returns the collection of setting pages.
        /// </summary>
        public IEnumerable<ISettingPageContext> SettingPages => Values
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
            if (!TryGetValue(item.PluginContext, out var appDict))
            {
                appDict = [];
                Add(item.PluginContext, appDict);
            }

            if (!appDict.TryGetValue(item.ApplicationContext, out var contextDict))
            {
                contextDict = [];
                appDict.Add(item.ApplicationContext, contextDict);
            }

            return contextDict.AddSettingPageItem(item.Context, item.Section, item.Group, item);
        }
    }
}
