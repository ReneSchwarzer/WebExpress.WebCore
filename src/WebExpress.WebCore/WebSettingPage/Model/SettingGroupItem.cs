using System;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents an item on the setting group.
    /// </summary>
    public class SettingGroupItem : IDisposable
    {
        /// <summary>
        /// Returns the context of the associated plugin.
        /// </summary>
        public IPluginContext PluginContext { get; internal set; }

        /// <summary>
        /// Returns the application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; internal set; }

        /// <summary>
        /// Returns the setting group context.
        /// </summary>
        public ISettingGroupContext SettingGroupContext { get; internal set; }

        /// <summary>
        /// Returns the class type of the setting group.
        /// </summary>
        public Type SettingGroupClass { get; internal set; }

        /// <summary>
        /// Returns the human-readable name or a internationalization key of the group.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Returns the human-readable description or a internationalization key of the group. 
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Returns the setting category.
        /// </summary>
        public Type Category { get; internal set; }

        /// <summary>
        /// Returns the section.
        /// </summary>
        public SettingSection Section { get; internal set; }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
