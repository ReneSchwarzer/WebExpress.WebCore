using System;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSettingPage.Model
{
    /// <summary>
    /// Represents an item on the setting page.
    /// </summary>
    public class SettingPageItem : IDisposable
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
        /// Returns the setting page context.
        /// </summary>
        public ISettingPageContext SettingPageContext { get; internal set; }

        /// <summary>
        /// Returns or sets the class type of the setting page.
        /// </summary>
        public Type SettingPageClass { get; internal set; }

        /// <summary>
        /// Returns or sets the instance of the setting page, if the page is cached, otherwise null.
        /// </summary>
        public IEndpoint Instance { get; internal set; }

        /// <summary>
        /// Returns the setting context.
        /// </summary>
        public string Context { get; internal set; }

        /// <summary>
        /// Returns the section.
        /// </summary>
        public SettingSection Section { get; internal set; }

        /// <summary>
        /// Returns the group.
        /// </summary>
        public string Group { get; internal set; }

        /// <summary>
        /// Returns a value indicating whether the component is created once and reused on each execution.
        /// </summary>
        public bool Cache { get; internal set; }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
