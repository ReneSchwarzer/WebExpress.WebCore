﻿using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:resourcea1x.label")]
    [Segment("a1x", "webindex:homepage.label")]
    [ContextPath(null)]
    [Module<TestModuleA1>]
    public sealed class TestResourceA1X : IResource
    {
        /// <summary>
        /// Instillation of the resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="resourceContext">The context of the resource.</param>
        public TestResourceA1X(IResourceContext resourceContext)
        {
            // test the injection
            if (resourceContext == null)
            {
                throw new ArgumentNullException(nameof(resourceContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The processed response.</returns>
        public Response Process(WebMessage.Request request)
        {
            // test the request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Parameter cannot be null or empty.");
            }

            return null;
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
