using WebExpress.WebCore.Test.Fixture;
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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
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
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiA), "webexpress.webcore.test.testrestapia")]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiB), "webexpress.webcore.test.testrestapib")]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiC), "webexpress.webcore.test.testrestapic")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiA), "webexpress.webcore.test.testrestapia")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiB), "webexpress.webcore.test.testrestapib")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiC), "webexpress.webcore.test.testrestapic")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiA), "webexpress.webcore.test.testrestapia")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiB), "webexpress.webcore.test.testrestapib")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiC), "webexpress.webcore.test.testrestapic")]
        public void Id(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var api = componentHub.RestApiManager.GetRestApi(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, api?.EndpointId);
        }

        /// <summary>
        /// Test the context path property of the rest api.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiA), "/appa/1")]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiB), "/appa/1/apia/2")]
        [InlineData(typeof(TestApplicationA), typeof(TestRestApiC), "/appa/1/apia/2/apib/3")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiA), "/appb/1")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiB), "/appb/1/apia/2")]
        [InlineData(typeof(TestApplicationB), typeof(TestRestApiC), "/appb/1/apia/2/apib/3")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiA), "/1")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiB), "/1/apia/2")]
        [InlineData(typeof(TestApplicationC), typeof(TestRestApiC), "/1/apia/2/apib/3")]
        public void ContextPath(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var api = componentHub.RestApiManager.GetRestApi(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, api?.ContextPath);
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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var api in componentHub.RestApiManager.RestApis)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(api.GetType()), $"Api context {api.GetType().Name} does not implement IContext.");
            }
        }
    }
}
