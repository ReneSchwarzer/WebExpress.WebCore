namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// The type of the URI.
    /// </summary>
    public enum UriScheme
    {
        /// <summary>
        /// The File URI scheme.
        /// </summary>
        File,

        /// <summary>
        /// The FTP URI scheme.
        /// </summary>
        FTP,

        /// <summary>
        /// The HTTP URI scheme.
        /// </summary>
        Http,

        /// <summary>
        /// The HTTPS URI scheme.
        /// </summary>
        Https,

        /// <summary>
        /// The LDAP URI scheme.
        /// </summary>
        Ldap,

        /// <summary>
        /// The LDAPS URI scheme.
        /// </summary>
        Ldaps,

        /// <summary>
        /// The Mailto URI scheme.
        /// </summary>
        Mailto
    }

    /// <summary>
    /// Extension methods for the <see cref="UriScheme"/> enum.
    /// </summary>
    public static class UriSchemeExtension
    {
        /// <summary>
        /// Converts the <see cref="UriScheme"/> to its string representation.
        /// </summary>
        /// <param name="scheme">The URI scheme to convert.</param>
        /// <returns>The string representation of the URI scheme.</returns>
        public static string ToString(this UriScheme scheme)
        {
            return scheme switch
            {
                UriScheme.File => "file",
                UriScheme.FTP => "ftp",
                UriScheme.Http => "http",
                UriScheme.Https => "https",
                UriScheme.Ldap => "ldap",
                UriScheme.Ldaps => "ldaps",
                UriScheme.Mailto => "mailto",
                _ => "http"
            };
        }
    }
}