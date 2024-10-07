using System;
using System.Collections.Generic;
using System.Globalization;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebModule;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebSitemap
{
    /// <summary>
    /// Contains the registration details for an endpoint, including factory and resolver functions.
    /// </summary>
    public class EndpointRegistration
    {
        /// <summary>
        /// Returns or sets the factory function to create a specific endpoint.
        /// </summary>
        public Func<IEndpointContext, UriResource, CultureInfo, IEndpoint> Factory { get; set; }

        /// <summary>
        /// Returns or sets the context resolver function to resolve the corresponding endpoint contexts.
        /// </summary>
        public Func<Type, IModuleContext, IEnumerable<IEndpointContext>> ContextResolver { get; set; }

        /// <summary>
        /// Returns or sets the endpoint resolver function to resolve additional endpoint contexts.
        /// </summary>
        public Func<IEnumerable<IEndpointContext>> EndpointResolver { get; set; }

        /// <summary>
        /// Returns or sets the function to handle requests.
        /// </summary>
        public Func<IEndpoint, IEndpointContext, Request, Response> HandleRequest { get; set; }
    }
}
