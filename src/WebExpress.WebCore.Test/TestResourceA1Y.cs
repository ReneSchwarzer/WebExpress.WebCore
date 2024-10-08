﻿using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Segment("a1y", "webindex:homepage.label")]
    [Parent<TestResourceA1X>]
    [Module<TestModuleA1>]
    public sealed class TestResourceA1Y : IResource
    {
        /// <summary>
        /// Instillation of the resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        public TestResourceA1Y()
        {
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
