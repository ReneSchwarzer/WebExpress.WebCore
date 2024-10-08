using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebRestApi;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy class for testing purposes.
    /// </summary>
    [Title("webindex:resourcea1x.label")]
    [Segment("ra1z", "webindex:homepage.label")]
    [Parent<TestRestApiA1Y>]
    [Method(CrudMethod.GET)]
    [Version(3)]
    [Module<TestModuleA1>]
    public sealed class TestRestApiA1Z : RestApi
    {
        /// <summary>
        /// Instillation of the rest api resource. Here, for example, managed resources can be loaded. 
        /// </summary>
        /// <param name="componentHub">The component hub.</param>
        /// <param name="restApiContext">The context of the restapi resource.</param>
        public TestRestApiA1Z(IComponentHub componentHub, IRestApiContext restApiContext)
            : base(restApiContext)
        {
            // test the injection
            if (componentHub == null)
            {
                throw new ArgumentNullException(nameof(componentHub), "Parameter cannot be null or empty.");
            }

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
        public override void CreateData(WebMessage.Request request)
        {

        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The data.</returns>
        public override object GetData(WebMessage.Request request)
        {
            return null;
        }

        /// <summary>
        /// Updates data.
        /// </summary>
        /// <param name="request">The request.</param>
        public override void UpdateData(WebMessage.Request request)
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
        public override void DeleteData(WebMessage.Request request)
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
