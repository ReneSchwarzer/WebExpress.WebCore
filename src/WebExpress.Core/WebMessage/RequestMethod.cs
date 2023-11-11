namespace WebExpress.Core.WebMessage
{
    public enum RequestMethod
    {
        NONE,
        GET,
        POST,
        PUT,
        HEAD,
        DELETE,
        PATCH // RFC 5789 
    }

    public static class RequestMethodExtensions
    {
        /// <summary>
        /// Umwandlung in eine CSS-Klasse
        /// </summary>
        /// <param name="layout">Das Layout, welches umgewandelt werden soll</param>
        /// <returns>Die zum Layout gehörende CSS-KLasse</returns>
        public static string ToString(this RequestMethod layout)
        {
            return layout switch
            {
                RequestMethod.GET => "GET",
                RequestMethod.POST => "POST",
                RequestMethod.PUT => "PUT",
                RequestMethod.HEAD => "HEAD",
                RequestMethod.DELETE => "DELETE",
                RequestMethod.PATCH => "PATCH",
                _ => string.Empty,
            };
        }
    }
}
