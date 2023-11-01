using System.Collections.Generic;
using WebExpress.WebMessage;

namespace WebExpress.WebSession
{
    public class SessionPropertyParameter : SessionProperty
    {
        /// <summary>
        /// Returns the parameters.
        /// </summary>
        public Dictionary<string, Parameter> Params { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SessionPropertyParameter()
        {
            Params = new Dictionary<string, Parameter>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="param">The parameters</param>
        public SessionPropertyParameter(Dictionary<string, Parameter> param)
        {
            Params = param;
        }
    }
}
