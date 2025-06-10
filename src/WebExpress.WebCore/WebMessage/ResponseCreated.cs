using WebExpress.WebCore.WebAttribute;

namespace WebExpress.WebCore.WebMessage
{
    /// <summary>
    /// Represents a response with a 201 Created status code, indicating that a resource has been successfully created.
    /// </summary>
    [StatusCode(201)]
    public class ResponseCreated : Response
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ResponseCreated()
        {
            Reason = "Created";
        }
    }
}
