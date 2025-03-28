using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.Test.WWW.Api;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebRestApi;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the rest api manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestRestApiManager
    {
        /// <summary>
        /// Test the register function of the rest api manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(9, componentHub.RestApiManager.RestApis.Count());
        }

        /// <summary>
        /// Test the remove function of the rest api manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var apiManager = componentHub.RestApiManager as RestApiManager;

            // test execution
            apiManager.Remove(plugin);

            Assert.Empty(componentHub.RestApiManager.RestApis);
        }

        /// <summary>
        /// Test the id property of the rest api.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiA), "webexpress.webcore.test.www.api.testrestapia")]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiB), "webexpress.webcore.test.www.api.testrestapib")]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiC), "webexpress.webcore.test.www.api.testrestapic")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiA), "webexpress.webcore.test.www.api.testrestapia")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiB), "webexpress.webcore.test.www.api.testrestapib")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiC), "webexpress.webcore.test.www.api.testrestapic")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiA), "webexpress.webcore.test.www.api.testrestapia")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiB), "webexpress.webcore.test.www.api.testrestapib")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiC), "webexpress.webcore.test.www.api.testrestapic")]
        public void Id(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var api = componentHub.RestApiManager.GetRestApi(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, api?.EndpointId.ToString());
        }

        /// <summary>
        /// Test the context path property of the rest api.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiA), "/server/appa/api/1/testrestapia")]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiB), "/server/appa/api/2/testrestapib")]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiC), "/server/appa/api/3/testrestapic")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiA), "/server/appb/api/1/testrestapia")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiB), "/server/appb/api/2/testrestapib")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiC), "/server/appb/api/3/testrestapic")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiA), "/server/api/1/testrestapia")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiB), "/server/api/2/testrestapib")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiC), "/server/api/3/testrestapic")]
        public void RoutePath(Type applicationType, Type resourceType, string path)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var api = componentHub.RestApiManager.GetRestApi(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(path, api?.Route.ToString());
        }

        /// <summary>
        /// Test the context path property of the rest api.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiA), CrudMethod.POST)]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiA), CrudMethod.GET)]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiB), CrudMethod.GET)]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiC), CrudMethod.GET)]
        public void Method(Type applicationType, Type resourceType, CrudMethod method)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var api = componentHub.RestApiManager.GetRestApi(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Contains(method, api?.Methods);
        }

        /// <summary>
        /// Tests whether the rest api manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.RestApiManager.GetType()));
        }

        /// <summary>
        /// Tests whether the rest api context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var api in componentHub.RestApiManager.RestApis)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(api.GetType()), $"Api context {api.GetType().Name} does not implement IContext.");
            }
        }
    }
}
