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
        /// Stores the event handler for adding an endpoint.
        /// </summary>
        public EventHandler<IEndpointContext> AddEndpoint { get; set; }

        /// <summary>
        /// Stores the event handler for removing an endpoint.
        /// </summary>
        public EventHandler<IEndpointContext> RemoveEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the context resolver function to resolve the corresponding endpoint contexts.
        /// </summary>
        public Func<Type, IApplicationContext, IEnumerable<IEndpointContext>> EndpointResolver { get; set; }

        /// <summary>
        /// Gets or sets the endpoint resolver function to resolve additional endpoint contexts.
        /// </summary>
        public Func<IEnumerable<IEndpointContext>> EndpointsResolver { get; set; }

        /// <summary>
        /// Gets or sets the function to handle requests.
        /// </summary>
        public Func<IRequest, IEndpointContext, IResponse> HandleRequest { get; set; }
    }
}
