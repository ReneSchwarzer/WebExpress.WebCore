using System;

namespace WebExpress.WebCore.WebPage
{
    /// <summary>
    /// Represents an exception that is thrown to redirect a web page.
    /// </summary>
    public class RedirectException : Exception
    {
        /// <summary>
        /// Returns or sets the redirection target.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Determines whether a permanent redirection should occur.
        /// </summary>
        public bool Permanet { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="url">The redirection target.</param> 
        /// <param name="permanent">true if 301 should be sent, false for 302.</param>
        public RedirectException(string url, bool permanent = false)
            : base("Redirecting to " + url)
        {
            Url = url;
            Permanet = permanent;
        }
    }
}
