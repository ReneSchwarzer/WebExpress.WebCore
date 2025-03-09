using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Provides context for setting categories within the web setting page.
    /// </summary>
    public interface ISettingCategoryContext : IContext
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
        /// Returns the category id.
        /// </summary>
        IComponentId CategoryId { get; }

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
