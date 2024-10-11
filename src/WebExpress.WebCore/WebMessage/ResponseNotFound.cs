﻿using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response for a resource not found (404) according to RFC 2616 Section 6.
    /// </summary>
    [StatusCode(404)]
    public class ResponseNotFound : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseNotFound()
        {
            var content = "<html><head><title>404</title></head><body>404 - Not Found</body></html>";
            Reason = "Not Found";

            Header.ContentType = "text/html";
            Header.ContentLength = content.Length;
            Content = content;
        }
    }
}
