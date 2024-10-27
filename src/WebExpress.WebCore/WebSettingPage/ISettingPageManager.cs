using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.SettingPage
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
    }
}