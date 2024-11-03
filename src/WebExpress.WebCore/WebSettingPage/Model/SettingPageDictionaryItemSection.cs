using System.Collections.Generic;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    public class SettingPageDictionaryItemSection : Dictionary<SettingSection, SettingPageDictionaryItemGroup>
    {
        /// <summary>
        /// Adds an item to the specified section and group.
        /// </summary>
        /// <param name="section">The section to add the item to.</param>
        /// <param name="group">The group within the section to add the item to.</param>
        /// <param name="item">The item to insert.</param>
        /// <returns>True if the settings page was added successfully; otherwise, false.</returns>
        public bool AddSettingPageItem(SettingSection section, string group, SettingPageItem item)
        {
            // register Section
            if (!ContainsKey(section))
            {
                Add(section, new SettingPageDictionaryItemGroup());
            }

            return this[section].AddSettingPageItem(group, item);
        }

        /// <summary>
        /// Searches for a setting page item based on its ID.
        /// </summary>
        /// <param name="pageId">The ID of the setting page item to search for.</param>
        /// <returns>The setting page item found, or null if no item with the specified ID exists.</returns>
        public SettingPageSearchResult FindPage(string pageId)
        {
            foreach (var v in this)
            {
                var path = v.Value.FindPage(pageId);
                if (path != null)
                {
                    path.Section = v.Key;
                    return path;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first setting page item found.
        /// </summary>
        /// <returns>The first setting page item found, or null if no items exist.</returns>
        public SettingPageSearchResult FindFirstPage()
        {
            var firstPage = default(SettingPageSearchResult);

            if (ContainsKey(SettingSection.Preferences))
            {
                firstPage = this[SettingSection.Preferences].FindFirstPage();
                if (firstPage != null)
                {
                    firstPage.Section = SettingSection.Preferences;

                    return firstPage;
                }
            }
            else if (ContainsKey(SettingSection.Primary))
            {
                firstPage = this[SettingSection.Primary].FindFirstPage();
                if (firstPage != null)
                {
                    firstPage.Section = SettingSection.Primary;

                    return firstPage;
                }
            }
            else if (ContainsKey(SettingSection.Secondary))
            {
                firstPage = this[SettingSection.Secondary].FindFirstPage();
                if (firstPage != null)
                {
                    firstPage.Section = SettingSection.Secondary;

                    return firstPage;
                }
            }

            return firstPage;
        }

        /// <summary>
        /// Returns the group of items that are in the specified section.
        /// </summary>
        /// <param name="section">The section to retrieve the group from.</param>
        /// <returns>The group of items in the specified section, or null if the section does not exist.</returns>
        public SettingPageDictionaryItemGroup GetGroup(SettingSection section)
        {
            if (ContainsKey(section))
            {
                return this[section];
            }

            return null;
        }
    }
}
