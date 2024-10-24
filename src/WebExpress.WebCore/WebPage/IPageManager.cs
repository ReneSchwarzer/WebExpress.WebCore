using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// The page manager manages page elements, which can be called with a URI (Uniform Resource Identifier).
    /// </summary>
    public interface IPageManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when an page is added.
        /// </summary>
        event EventHandler<IPageContext> AddPage;

        /// <summary>
        /// An event that fires when an page is removed.
        /// </summary>
        event EventHandler<IPageContext> RemovePage;

        /// <summary>
        /// Returns all pages contexts.
        /// </summary>
        IEnumerable<IPageContext> Pages { get; }

        /// <summary>
        /// Returns an enumeration of all containing page contexts of a plugin.
        /// </summary>
        /// <param name="pluginContext">A context of a plugin whose pages are to be registered.</param>
        /// <returns>An enumeration of page contexts.</returns>
        IEnumerable<IPageContext> GetPages(IPluginContext pluginContext);

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <typeparam name="T">The page type.</typeparam>
        /// <returns>An enumeration of page contextes.</returns>
        IEnumerable<IPageContext> GetPages<T>() where T : IPage;

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <param name="pageType">The page type.</param>
        /// <returns>An enumeration of page contextes.</returns>
        IEnumerable<IPageContext> GetPages(Type pageType);

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <param name="pageType">The page type.</param>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of page contextes.</returns>
        IEnumerable<IPageContext> GetPages(Type pageType, IApplicationContext applicationContext);

        /// <summary>
        /// Returns an enumeration of page contextes.
        /// </summary>
        /// <typeparam name="T">The page type.</typeparam>
        /// <param name="applicationContext">The context of the application.</param>
        /// <returns>An enumeration of page contextes.</returns>
        IEnumerable<IPageContext> GetPages<T>(IApplicationContext applicationContext) where T : IPage;

        /// <summary>
        /// Returns the page context.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>An page context or null.</returns>
        IPageContext GetPage(IApplicationContext applicationContext, string pageId);

        /// <summary>
        /// Returns the page context.
        /// </summary>
        /// <param name="applicationId">The application id.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>An page context or null.</returns>
        IPageContext GetPage(string applicationId, string pageId);
    }
}
