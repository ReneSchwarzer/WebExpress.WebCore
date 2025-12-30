using System.Collections.Generic;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Defines the context for a rest api resource, providing access to various related contexts and properties.
    /// </summary>
    public interface IRestApiContext : IEndpointContext
    {
        /// <summary>
        /// Returns the crud methods.
        /// </summary>
        IEnumerable<RequestMethod> Methods { get; }

        /// <summary>
        /// Returns the version number of the rest api.
        /// </summary>
        uint Version { get; }
    }
}
