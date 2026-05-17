using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.WebInclude
{
    /// <summary>
    /// Defines the include manager contract. It exposes an inventory of include contexts and raises add/remove events.
    /// </summary>
    public interface IIncludeManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when an include is added.
        /// </summary>
        event EventHandler<IIncludeContext> AddInclude;

        /// <summary>
        /// An event that fires when an include is removed.
        /// </summary>
        event EventHandler<IIncludeContext> RemoveInclude;

        /// <summary>
        /// Gets all include contexts.
        /// </summary>
        IEnumerable<IIncludeContext> Includes { get; }

        /// <summary>
        /// Returns include contexts for a given application.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <returns>Enumerable of include contexts.</returns>
        IEnumerable<IIncludeContext> GetIncludes(IApplicationContext applicationContext);

        /// <summary>
        /// Returns include contexts for a given application and include type.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        /// <param name="includeType">The include class type.</param>
        /// <returns>Enumerable of include contexts.</returns>
        IEnumerable<IIncludeContext> GetIncludes(IApplicationContext applicationContext, Type includeType);
    }
}
