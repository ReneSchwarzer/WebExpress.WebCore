namespace WebExpress.WebCore.WebSession
{
    public abstract class AuthorizationService
    {
        /// <summary>
        /// Prüft ob der authentifizierte Nutzer autorisiert ist
        /// </summary>
        /// <param name="session">Die aktuelle Session</param>
        /// <returns>true, wenn autorisiert false sonst</returns>
        public abstract bool Authorization(Session session);
    }
}
