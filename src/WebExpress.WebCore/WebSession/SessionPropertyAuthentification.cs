namespace WebExpress.WebCore.WebSession
{
    public class SessionPropertyAuthentification : SessionProperty
    {
        /// <summary>
        /// Returns or sets the login name.
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        /// Provides or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}
