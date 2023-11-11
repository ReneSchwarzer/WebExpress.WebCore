using System;

namespace WebExpress.Core.WebResource
{
    public class RedirectException : Exception
    {
        /// <summary>
        /// Liefert oder setzt das Weiterleitungsziel
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Bestimmt, ob ein permanete Weiterleitung erfolgen soll
        /// </summary>
        public bool Permanet { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="url">Das Weiterleitungsziel</param> 
        /// <param name="permanent">true wenn 301 gesendet werden soll, flase für 302</param>
        public RedirectException(string url, bool permanent = false)
            : base("Redirecting to " + url)
        {
            Url = url;
            Permanet = permanent;
        }
    }
}
