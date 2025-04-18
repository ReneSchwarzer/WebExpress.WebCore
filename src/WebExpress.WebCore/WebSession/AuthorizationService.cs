using WebExpress.WebCore.WebSession.Model;

namespace WebExpress.WebCore.WebSession
{

    /// <summary>
    /// Represents an abstract authorization service.
    /// </summary>
    public abstract class AuthorizationService
    {
        /// <summary>
        /// Checks if the authenticated user is authorized.
        /// </summary>
        /// <param name="session">The current session.</param>
        /// <returns>true if authorized, false otherwise.</returns>
        public abstract bool Authorization(Session session);
    }
}
