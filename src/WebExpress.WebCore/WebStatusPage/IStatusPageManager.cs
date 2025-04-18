using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebStatusPage
{
    /// <summary>
    /// Management of status pages.
    /// </summary>
    public interface IStatusPageManager : IComponentManager
    {
        /// <summary>
        /// An event that fires when an status page is added.
        /// </summary>
        event EventHandler<IStatusPageContext> AddStatusPage;

        /// <summary>
        /// An event that fires when an status page is removed.
        /// </summary>
        event EventHandler<IStatusPageContext> RemoveStatusPage;

        /// <summary>
        /// Returns all status pages.
        /// </summary>
        IEnumerable<IStatusPageContext> StatusPages { get; }

        /// <summary>
        /// Determines the status page for a given application context and status type.
        /// </summary>
        /// <param name="applicationContext">The context of the application.</param>
        /// <param name="statusPageClass">The status page class.</param>
        /// <returns>The context of the status page or null.</returns>
        IStatusPageContext GetStatusPage(IApplicationContext applicationContext, Type statusPageClass);

        /// <summary>
        /// Creates a status response.
        /// </summary>
        /// <param name="message">The status message.</param>
        /// <param name="status">The status code.</param>
        /// <param name="applicationContext">The application context where the status pages are located or null for an undefined page (may be from another application) that matches the status code.</param>
        /// <param name="request">The request.</param>
        /// <returns>The response or null.</returns>
        Response CreateStatusResponse(string message, int status, IApplicationContext applicationContext, Request request);
    }
}
