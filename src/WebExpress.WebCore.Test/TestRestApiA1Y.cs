﻿using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebRestApi;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:resourcea1x.label")]
    [Segment("ra1y", "webindex:homepage.label")]
    [Parent<TestRestApiA1X>]
    [Method(CrudMethod.GET)]
    [Version(2)]
    [Module<TestModuleA1>]
    public sealed class TestRestApiA1Y : IRestApi
    {
        /// <summary>
        /// Initialization of the rest api resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="restApiContext">The context of the restapi resource.</param>
        public TestRestApiA1Y(IRestApiContext restApiContext)
        {
            // test the injection
            if (restApiContext == null)
            {
                throw new ArgumentNullException(nameof(restApiContext), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Creates data.
        /// </summary>
        /// <param name="request">The request.</param>
        public void CreateData(WebMessage.Request request)
        {

        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The data.</returns>
        public object GetData(WebMessage.Request request)
        {
            return null;
        }

        /// <summary>
        /// Updates data.
        /// </summary>
        /// <param name="request">The request.</param>
        public void UpdateData(WebMessage.Request request)
        {
            // test the request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Deletes data.
        /// </summary>
        /// <param name="request">The request.</param>
        public void DeleteData(WebMessage.Request request)
        {
            // test the request
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Parameter cannot be null or empty.");
            }
        }

        /// <summary>
        /// Release of unmanaged resources reserved during use.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
