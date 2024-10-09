using System.Collections.Generic;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebSession.Model
{
    public class SessionPropertyParameter : SessionProperty
    {
        /// <summary>
        /// Returns the parameters.
        /// </summary>
        public Dictionary<string, Parameter> Params { get; private set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SessionPropertyParameter()
        {
            Params = new Dictionary<string, Parameter>();
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="param">The parameters</param>
        public SessionPropertyParameter(Dictionary<string, Parameter> param)
        {
            Params = param;
        }
    }
}
