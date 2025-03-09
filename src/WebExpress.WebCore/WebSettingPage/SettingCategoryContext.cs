using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Provides context for setting categories within the web setting page.
    /// </summary>
    public class SettingCategoryContext : ISettingCategoryContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the category id.
        /// </summary>
        public IComponentId CategoryId { get; internal set; }

        /// <summary>
        /// Returns the icon.
        /// </summary>
        public string Icon { get; internal set; }

        /// <summary>
        /// Returns the name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Returns the description.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Returns the section.
        /// </summary>
        public SettingSection Section { get; internal set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Setting category: {CategoryId}";
        }
    }
}
