using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebSession.Model;

namespace WebExpress.WebCore.WebSession
{
    /// <summary>
    /// Represents a session manager that handles session creation and retrieval.
    /// </summary>
    public class SessionManager : ISessionManager, ISystemComponent
    {
        private readonly IHttpServerContext _httpServerContext;
        private readonly SessionDictionary _dictionary = [];

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used via Reflection.")]
        private SessionManager(IHttpServerContext context)
        {
            _httpServerContext = context;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress.webcore:sessionmanager.initialization")
            );
        }

        /// <summary>
        /// Creates a session or returns an existing session based on the provided request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The session.</returns>
        public Session GetSession(Request request)
        {
            var session = default(Session);

            // Session ermitteln
            var sessionCookie = request?.Header
                .Cookies?.Where(x => x.Name.Equals("session", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            var guid = Guid.NewGuid();

            try
            {
                guid = Guid.Parse(sessionCookie?.Value);
            }
            catch
            {

            }

            if (sessionCookie is not null && _dictionary.TryGetValue(guid, out Session value))
            {
                session = value;
                session.Updated = DateTime.Now;
            }
            else
            {
                // no or invalid session => assign new session
                session = new Session(guid);

                lock (_dictionary)
                {
                    _dictionary[guid] = session;
                }
            }

            return session;
        }

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
        public ISessionManager CleanUp(IApplicationContext applicationContext, int timeoutMinutes = 60 * 24 * 365)
        {
            // validate input
            ArgumentNullException.ThrowIfNull(applicationContext);

            // read timeout; non-positive values disable cleanup
            if (timeoutMinutes <= 0)
            {
                return this;
            }

            var now = DateTime.Now;

            // collect expired ids under lock to avoid concurrent modifications during enumeration
            IEnumerable<Guid> expiredIds;
            lock (_dictionary)
            {
                expiredIds = _dictionary.Values
                    .Where(s => (now - s.Updated).TotalMinutes > timeoutMinutes)
                    .Select(s => s.Id);

                // remove expired sessions under the same lock
                foreach (var id in expiredIds)
                {
                    _dictionary.Remove(id);
                }
            }

            // log removals outside the lock
            foreach (var id in expiredIds)
            {
                _httpServerContext.Log.Info
                (
                    I18N.Translate("webexpress.webcore:sessionmanager.cleanup.removed", id)
                );
            }

            return this;
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
