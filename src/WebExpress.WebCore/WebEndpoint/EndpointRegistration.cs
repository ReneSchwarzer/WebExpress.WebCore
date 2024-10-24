using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebEndpoint
{
    /// <summary>
    /// Contains the registration details for an endpoint, including factory and resolver functions.
    /// </summary>
    public class EndpointRegistration
    {
        /// <summary>
        /// Returns or sets the context resolver function to resolve the corresponding endpoint contexts.
        /// </summary>
        public Func<Type, IApplicationContext, IEnumerable<IEndpointContext>> EndpointResolver { get; set; }

        /// <summary>
        /// Returns or sets the endpoint resolver function to resolve additional endpoint contexts.
        /// </summary>
        public Func<IEnumerable<IEndpointContext>> EndpointsResolver { get; set; }

        /// <summary>
        /// Returns or sets the function to handle requests.
        /// </summary>
        public Func<Request, IEndpointContext, Response> HandleRequest { get; set; }


    }
}
