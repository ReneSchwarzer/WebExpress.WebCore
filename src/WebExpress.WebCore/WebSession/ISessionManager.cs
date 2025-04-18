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
        Session GetSession(Request request);
    }
}
