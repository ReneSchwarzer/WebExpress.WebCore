using System.Collections.Generic;
using WebExpress.WebCore.SettingPage;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    public class SettingPageDictionaryItemContext : Dictionary<string, SettingPageDictionaryItemSection>
    {
        /// <summary>
        /// Adds a page.
        /// </summary>
        /// <param name="context">The settig context.</param>
        /// <param name="section">The section.</param>
        /// <param name="group">The group.</param>
        /// <param name="page">The item to insert.</param>
        public void AddPage(string context, SettingSection section, string group, SettingPageItem page)
        {
            context ??= "*";

            // register context
            if (!ContainsKey(context))
            {
                Add(context, new SettingPageDictionaryItemSection());
            }

            this[context].AddPage(section, group, page);
        }

        /// <summary>
        /// Searches for a setting page by its id.
        /// </summary>
        /// <param name="pageId">The setting site.</param>
        /// <returns>The setting page found or null.</returns>
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
        /// Provide all sections that have the same setting context.
        /// </summary>
        /// <param name="context">The setting context.</param>
        /// <returns>A listing of all sections of the same context.</returns>
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
