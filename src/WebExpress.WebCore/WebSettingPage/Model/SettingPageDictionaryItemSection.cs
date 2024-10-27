using System.Collections.Generic;
using WebExpress.WebCore.SettingPage;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    public class SettingPageDictionaryItemSection : Dictionary<SettingSection, SettingPageDictionaryItemGroup>
    {
        /// <summary>
        /// Adds an item.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <param name="group">The group.</param>
        /// <param name="item">The item to insert.</param>
        public void AddPage(SettingSection section, string group, SettingPageItem item)
        {
            // register Section
            if (!ContainsKey(section))
            {
                Add(section, new SettingPageDictionaryItemGroup());
            }

            this[section].AddPage(group, item);
        }

        /// <summary>
        /// Searches for an item based on its id.
        /// </summary>
        /// <param name="pageId">The setting site id.</param>
        /// <returns>The setting page found or null.</returns>
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
        /// Returns the first setting page.
        /// </summary>
        /// <returns> The first setting page.</returns>
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
        /// Returns a groups that are in the given section.
        /// </summary>
        /// <param name="section">The section.</param>
        /// <returns>A group in the same section.</returns>
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
