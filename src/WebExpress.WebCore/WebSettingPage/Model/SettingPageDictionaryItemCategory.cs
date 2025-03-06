using System.Collections.Generic;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents a category for setting page dictionary items, inheriting from dictionary.
    /// </summary>
    public class SettingPageDictionaryItemCategory : Dictionary<string, SettingPageDictionaryItemSection>
    {
        /// <summary>
        /// Adds a setting page item to the dictionary.
        /// </summary>
        /// <param name="context">The setting context.</param>
        /// <param name="section">The section to which the item belongs.</param>
        /// <param name="group">The group to which the item belongs.</param>
        /// <param name="page">The setting page item to add.</param>
        /// <returns>True if the setting page item was added successfully; otherwise, false.</returns>
        public bool AddSettingPageItem(string context, SettingSection section, string group, SettingPageItem page)
        {
            context ??= "*";

            // register context
            if (!ContainsKey(context))
            {
                Add(context, []);
            }

            return this[context].AddSettingPageItem(section, group, page);
        }

        /// <summary>
        /// Searches for a setting page by its ID.
        /// </summary>
        /// <param name="pageId">The ID of the setting page to search for.</param>
        /// <returns>The setting page found, or null if no page was found.</returns>
        public SettingPageSearchResult FindPage(string pageId)
        {
            foreach (var v in this)
            {
                var path = v.Value.FindPage(pageId);
                if (path != null)
                {
                    path.Context = v.Key;
                    return path;
                }
            }

            return null;
        }

        /// <summary>
        /// Provides all sections that have the same setting context.
        /// </summary>
        /// <param name="context">The setting context.</param>
        /// <returns>A listing of all sections of the same context, or null if the context does not exist.</returns>
        public SettingPageDictionaryItemSection GetSections(string context)
        {
            if (ContainsKey(context))
            {
                return this[context];
            }

            return null;
        }
    }
}
