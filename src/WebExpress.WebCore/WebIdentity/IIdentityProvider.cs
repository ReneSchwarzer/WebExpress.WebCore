using System.Collections.Generic;
using WebExpress.WebCore.WebEndpoint;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebIdentity
{
    /// <summary>
    /// Represents an external identity provider that supplies identities and groups
    /// to the WebExpress identity system.
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Returns all identities provided by this source.
        /// </summary>
        IEnumerable<IIdentity> GetIdentities();

        /// <summary>
        /// Returns all groups provided by this source.
        /// </summary>
        IEnumerable<IIdentityGroup> GetGroups();

        /// <summary>
        /// Authenticates the specified request and returns the associated identity.
        /// </summary>
        /// <param name="request">
        /// The request to authenticate. Cannot be null.
        /// </param>
        /// <returns>
        /// An identity representing the authenticated user if authentication is successful; otherwise, null.
        /// </returns>
        IIdentity Authenticate(IRequest request);

        /// <summary>
        /// Displays a login dialog using the specified request and identity information.
        /// </summary>
        /// <param name="request">
        /// The request containing parameters and context for the login operation. Cannot be null.
        /// </param>
        /// <param name="initiator">
        /// The endpoint that triggered the authentication process. Used to determine the origin and
        /// context of the authentication requirement.
        /// </param>
        /// <param name="identity">
        /// The identity information to be used for authentication. Cannot be null.
        /// </param>
        /// <returns>
        /// An object that represents the response to the login dialog, including authentication results and any
        /// relevant status information.
        /// </returns>
        IResponse CreateAuthenticationPrompt(IRequest request, IEndpointContext initiator, IIdentity identity);
    }
}
