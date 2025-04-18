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
        /// Returns the collection of setting categories.
        /// </summary>
        IEnumerable<ISettingCategoryContext> SettingCategories { get; }

        /// <summary>
        /// Returns the collection of setting groups.
        /// </summary>
        IEnumerable<ISettingGroupContext> SettingGroups { get; }

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

        /// <summary>
        /// Returns an enumeration of setting page contexts for the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve setting pages.</param>
        /// <returns>An enumeration of setting page contexts.</returns>
        IEnumerable<ISettingPageContext> GetSettingPages(IApplicationContext applicationContext, ISettingCategoryContext category);

        /// <summary>
        /// Returns an enumeration of setting page contexts for the specified application context, category, and group.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="group">The group for which to retrieve setting pages.</param>
        /// <returns>An enumeration of setting page contexts.</returns>
        IEnumerable<ISettingPageContext> GetSettingPages(IApplicationContext applicationContext, ISettingGroupContext group);

        /// <summary>
        /// Returns the first setting page context for the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve setting pages.</param>
        /// <returns>The first setting page context or null.</returns>
        ISettingPageContext GetFirstSettingPage(IApplicationContext applicationContext, ISettingCategoryContext category);

        /// <summary>
        /// Returns the categories associated with the specified application context.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of category contexts associated with the specified application context.</returns>
        IEnumerable<ISettingCategoryContext> GetSettingCategories(IApplicationContext applicationContext);

        /// <summary>
        /// Returns the groups associated with the specified application context and category.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="category">The category for which to retrieve groups.</param>
        /// <returns>An enumeration of setting group contexts associated with the provided application context and category.</returns>
        IEnumerable<ISettingGroupContext> GetSettingGroups(IApplicationContext applicationContext, ISettingCategoryContext category);
    }
}