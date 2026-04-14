using System.Collections.Generic;
using WebExpress.WebCore.WebParameter;

namespace WebExpress.WebCore.WebSession.Model
{
    /// <summary>
    /// Represents a session property with parameters.
    /// </summary>
    public class SessionPropertyParameter : SessionProperty
    {
        /// <summary>
        /// Gets the parameters.
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
        /// <param name="parameters">The parameters.</param>
        public SessionPropertyParameter(params Parameter[] parameters)
        {
            foreach (var param in parameters)
            {
                Params.Add(param.Key, param);
            }
        }
    }
}
