using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebHtml;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebScope;
using WebExpress.WebCore.WebSection;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// Interface for managing web fragments.
    /// </summary>
    public interface IFragmentManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when a fragment is added.
        /// </summary>
        event EventHandler<IFragmentContext> AddFragment;

        /// <summary>
        /// An event that fires when a fragment is removed.
        /// </summary>
        event EventHandler<IFragmentContext> RemoveFragment;

        /// <summary>
        /// Returns the collection of fragment contexts.
        /// </summary>
        IEnumerable<IFragmentContext> Fragments { get; }

        /// <summary>
        /// Returns all fragment contexts that belong to a given fragment type.
        /// </summary>
        /// <typeparam name="T">The fragment type..</typeparam>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        IEnumerable<IFragmentContext> GetFragments<T>() where T : IFragment;

        /// <summary>
        /// Returns all fragment contexts that belong to a given fragment type.
        /// </summary>
        /// <param name="fragmentType">The fragment type.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        IEnumerable<IFragmentContext> GetFragments(Type fragmentType);

        /// <summary>
        /// Returns all fragment contexts that belong to a given fragment type.
        /// </summary>
        /// <typeparam name="T">The fragment type..</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        IEnumerable<IFragmentContext> GetFragments<T>(IApplicationContext applicationContext) where T : IFragment;

        /// <summary>
        /// Returns all fragment contexts that belong to a given fragment type.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="fragmentType">The fragment type.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        IEnumerable<IFragmentContext> GetFragments(IApplicationContext applicationContext, Type fragmentType);

        /// <summary>
        /// Returns all fragment contexts that belong to a given application.
        /// </summary>
        /// <typeparam name="S">The section where the fragment is embedded.</typeparam>
        /// <typeparam name="T">The scope where the fragment is embedded.</typeparam>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        IEnumerable<IFragmentContext> GetFragments<S, T>(IApplicationContext applicationContext) where S : ISection where T : IScope;

        /// <summary>
        /// Returns all fragment contexts that belong to a given application.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="section">The section where the fragment is embedded.</param>
        /// <param name="scope">The scope where the fragment is embedded.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        IEnumerable<IFragmentContext> GetFragments(IApplicationContext applicationContext, Type section, Type scope);

        /// <summary>
        /// Returns all fragment contexts that belong to a given application.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="section">The section where the fragment is embedded.</param>
        /// <param name="scopes">The scopes where the fragment is embedded.</param>
        /// <returns>An enumeration of the filtered fragment contexts.</returns>
        IEnumerable<IFragmentContext> GetFragments(IApplicationContext applicationContext, Type section, IEnumerable<Type> scopes);

        /// <summary>
        /// Converts the fragments to HTML for a given section within the specified render context.
        /// </summary>
        /// <param name="renderContext">The context in which rendering occurs.</param>
        /// <param name="section">The section where the fragment is embedded.</param>
        /// <returns>An HTML node representing the rendered fragments. Can be null if no nodes are present.</returns>
        IHtmlNode Render(IRenderContext renderContext, Type section);
    }
}
