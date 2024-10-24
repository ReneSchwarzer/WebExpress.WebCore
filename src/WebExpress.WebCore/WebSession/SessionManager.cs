using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;
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
        internal SessionManager(IHttpServerContext context)
        {
            _httpServerContext = context;

            _httpServerContext.Log.Debug
            (
                I18N.Translate("webexpress:sessionmanager.initialization")
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

            Guid Guid = Guid.NewGuid();

            try
            {
                Guid = Guid.Parse(sessionCookie?.Value);
            }
            catch
            {

            }

            if (sessionCookie != null && _dictionary.ContainsKey(Guid))
            {
                session = _dictionary[Guid];
                session.Updated = DateTime.Now;
            }
            else
            {
                // no or invalid session => assign new session
                session = new Session(Guid);

                lock (_dictionary)
                {
                    _dictionary[Guid] = session;
                }
            }

            return session;
        }

        /// <summary>
        /// Collects and prepares information about the component for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
