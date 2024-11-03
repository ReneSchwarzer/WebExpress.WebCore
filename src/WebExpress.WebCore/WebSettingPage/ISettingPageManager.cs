using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebSettingPage
{
    /// <summary>
    /// Interface for managing setting pages.
    /// </summary>
    public interface ISettingPageManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when an setting page is added.
        /// </summary>
        event EventHandler<ISettingPageContext> AddSettingPage;

        /// <summary>
        /// An event that fires when an setting page is removed.
        /// </summary>
        event EventHandler<ISettingPageContext> RemoveSettingPage;

        /// <summary>
        /// Returns the collection of setting pages.
        /// </summary>
        IEnumerable<ISettingPageContext> SettingPages { get; }

        /// <summary>
        /// Returns an enumeration of setting page contextes.
        /// </summary>
        /// <param name="settingPageType">The setting page type.</param>
        /// <returns>An enumeration of setting page contextes.</returns>
        IEnumerable<ISettingPageContext> GetSettingPages(Type settingPageType);

        /// <summary>
        /// Returns an enumeration of setting page contextes.
        /// </summary>
        /// <param name="settingPageType">The setting page type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of setting page contextes.</returns>
        IEnumerable<ISettingPageContext> GetSettingPages(Type settingPageType, IApplicationContext applicationContext);
    }
}