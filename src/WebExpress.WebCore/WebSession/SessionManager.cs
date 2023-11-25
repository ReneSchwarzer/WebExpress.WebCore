using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebSession
{
    public class SessionManager : IComponent, ISystemComponent
    {
        /// <summary>
        /// Returns or sets the reference to the context of the host.
        /// </summary>
        public IHttpServerContext HttpServerContext { get; private set; }

        /// <summary>
        /// Returns the directory in which the sessions are stored on the server side.
        /// </summary>
        private SessionDictionary Dictionary { get; } = new SessionDictionary();

        /// <summary>
        /// Constructor
        /// </summary>
        internal SessionManager()
        {
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="context">The reference to the context of the host.</param>
        public void Initialization(IHttpServerContext context)
        {
            HttpServerContext = context;

            HttpServerContext.Log.Debug
            (
                InternationalizationManager.I18N("webexpress:sessionmanager.initialization")
            );
        }

        /// <summary>
        /// Creates a session or returns an existing session.
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

            if (sessionCookie != null && Dictionary.ContainsKey(Guid))
            {
                session = Dictionary[Guid];
                session.Updated = DateTime.Now;
            }
            else
            {
                // no or invalid session => assign new session
                session = new Session(Guid);

                lock (Dictionary)
                {
                    Dictionary[Guid] = session;
                }
            }

            return session;
        }

        /// <summary>
        /// Information about the component is collected and prepared for output in the log.
        /// </summary>
        /// <param name="pluginContext">The context of the plugin.</param>
        /// <param name="output">A list of log entries.</param>
        /// <param name="deep">The shaft deep.</param>
        public void PrepareForLog(IPluginContext pluginContext, IList<string> output, int deep)
        {
        }
    }
}
