using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Provides the context for a setting group.
    /// </summary>
    public interface ISettingGroupContext : IContext
    {
        /// <summary>
        /// Returns the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Returns the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Returns the setting category context to which the setting group belongs.
        /// </summary>
        ISettingCategoryContext SettingCategory { get; }

        /// <summary>
        /// Returns the group id.
        /// </summary>
        IComponentId GroupId { get; }

        /// <summary>
        /// Returns the icon.
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// Returns the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Returns the section.
        /// </summary>
        SettingSection Section { get; }
    }
}
