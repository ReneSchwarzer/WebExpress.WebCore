using System.Collections.Generic;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebSession.Model
{
    /// <summary>
    /// Represents a session property with parameters.
    /// </summary>
    public class SessionPropertyParameter : SessionProperty
    {
        /// <summary>
        /// Returns the parameters.
        /// </summary>
        public Dictionary<string, Parameter> Params { get; } = [];

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SessionPropertyParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="params">The parameters</param>
        public SessionPropertyParameter(params Parameter[] parameters)
        {
            foreach (var param in parameters)
            {
                Params.Add(param.Key, param);
            }
        }
    }
}
