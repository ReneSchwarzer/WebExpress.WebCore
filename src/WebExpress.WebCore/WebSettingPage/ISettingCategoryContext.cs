using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Provides context for setting categories within the web setting page.
    /// </summary>
    public interface ISettingCategoryContext : IContext
    {
        /// <summary>
        /// Gets the associated plugin context.
        /// </summary>
        IPluginContext PluginContext { get; }

        /// <summary>
        /// Gets the corresponding application context.
        /// </summary>
        IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Gets the category id.
        /// </summary>
        IComponentId CategoryId { get; }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        IIcon Icon { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the section.
        /// </summary>
        SettingSection Section { get; }
    }
}
