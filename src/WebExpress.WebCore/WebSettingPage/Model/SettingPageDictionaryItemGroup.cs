using System;
using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents a group of setting page dictionary items.
    /// </summary>
    public class SettingPageDictionaryItemGroup : Dictionary<string, List<SettingPageItem>>
    {
        /// <summary>
        /// Adds a setting page item to the specified group.
        /// </summary>
        /// <param name="group">The group to which the item should be added.</param>
        /// <param name="page">The setting page item to add.</param>
        /// <returns>True if the setting page item was added successfully; otherwise, false.</returns>
        public bool AddSettingPageItem(string group, SettingPageItem page)
        {
            group ??= string.Empty;

            // Register group if it does not exist.
            if (!ContainsKey(group))
            {
                Add(group, new List<SettingPageItem>());
            }

            var list = this[group];

            if (!list.Any(x => x.SettingPageContext.EndpointId == page.SettingPageContext.EndpointId))
            {
                list.Add(page);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Searches for a setting page item based on its ID.
        /// </summary>
        /// <param name="pageId">The ID of the setting page item to search for.</param>
        /// <returns>A <see cref="SettingPageSearchResult"/> containing the group and item if found; otherwise, null.</returns>
        public SettingPageSearchResult FindPage(string pageId)
        {
            foreach (var v in this)
            {
                var item = v.Value.Where(x => x.SettingPageContext.EndpointId.ToString().Equals(pageId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (item != null)
                {
                    return new SettingPageSearchResult() { Group = v.Key, Item = item };
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first setting page item in the dictionary.
        /// </summary>
        /// <returns>A <see cref="SettingPageSearchResult"/> containing the group and first item if found; otherwise, null.</returns>
        public SettingPageSearchResult FindFirstPage()
        {
            var firstItem = default(SettingPageItem);

            foreach (var group in this.OrderBy(x => x.Key))
            {
                firstItem = group.Value.FirstOrDefault();

                if (firstItem != null)
                {
                    return new SettingPageSearchResult() { Group = group.Key, Item = firstItem };
                }
            }

            return null;
        }

        /// <summary>
        /// Returns all setting page items in the specified group.
        /// </summary>
        /// <param name="group">The group whose items should be returned.</param>
        /// <returns>A list of <see cref="SettingPageItem"/> in the specified group; otherwise, null if the group does not exist.</returns>
        public List<SettingPageItem> GetPages(string group)
        {
            if (ContainsKey(group))
            {
                return this[group];
            }

            return null;
        }
    }
}
