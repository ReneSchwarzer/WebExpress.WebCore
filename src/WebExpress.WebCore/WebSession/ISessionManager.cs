using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebSession.Model;

namespace WebExpress.WebCore.WebSession
{
    /// <summary>
    /// Represents a session manager that handles session creation and retrieval.
    /// </summary>
    public interface ISessionManager : IComponentManager
    {
        /// <summary>
        /// Creates a session or returns an existing session based on the provided request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The session.</returns>
        Session GetSession(IRequest request);

        /// <summary>
        /// Cleans up expired sessions from the session manager based on the specified session timeout.
        /// </summary>
        /// <remarks>
        /// This method iterates through the sessions and removes those that have been inactive
        /// for longer than the configured session timeout. It logs the removal of each expired session.
        /// </remarks>
        /// <param name="applicationContext">
        /// The application context containing configuration settings, including the session timeout duration.
        /// </param>
        /// <param name="timeoutMinutes">
        /// The explicit session timeout in minutes; if non-positive, the configured timeout is used. If 
        /// the effective timeout is non-positive, cleanup is skipped.
        /// </param>
        /// <returns>The current instance of the session manager, allowing for method chaining.</returns>
        ISessionManager CleanUp(IApplicationContext applicationContext, int timeoutMinutes = 60 * 24 * 365);
    }
}
