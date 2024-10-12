namespace WebExpress.WebCore.WebSession.Model
{
    /// <summary>
    /// Represents the authentication session property.
    /// </summary>
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
