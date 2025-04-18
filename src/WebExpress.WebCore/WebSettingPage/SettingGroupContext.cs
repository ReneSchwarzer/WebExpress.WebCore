using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Provides context for setting group within the web setting page.
    /// </summary>
    public class SettingGroupContext : ISettingGroupContext
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
        /// Returns the setting category context to which the setting group belongs.
        /// </summary>
        public ISettingCategoryContext SettingCategory { get; internal set; }

        /// <summary>
        /// Returns the group id.
        /// </summary>
        public IComponentId GroupId { get; internal set; }

        /// <summary>
        /// Returns the icon.
        /// </summary>
        public IIcon Icon { get; internal set; }

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
            return $"Setting group: {GroupId}";
        }
    }
}
