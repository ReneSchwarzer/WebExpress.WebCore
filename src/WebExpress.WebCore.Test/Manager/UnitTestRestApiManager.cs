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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            Assert.Equal(3, componentHub.RestApiManager.RestApis.Count());
        }

        /// <summary>
        /// Test the remove function of the rest api manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
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
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1X), "webexpress.webcore.test.testrestapia1x")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1Y), "webexpress.webcore.test.testrestapia1y")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1Z), "webexpress.webcore.test.testrestapia1z")]
        public void Id(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var api = componentHub.RestApiManager.GetRestApi(module, resourceType);

            // test execution
            Assert.Equal(id, api.EndpointId);
        }

        /// <summary>
        /// Test the context path property of the rest api.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1X), "/aca/mca/1")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1Y), "/aca/mca/1/ra1x/2")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1Z), "/aca/mca/1/ra1x/2/ra1y/3")]
        public void ContextPath(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var api = componentHub.RestApiManager.GetRestApi(module, resourceType);

            // test execution
            Assert.Equal(id, api.ContextPath);
        }

        /// <summary>
        /// Test the context path property of the rest api.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1X), CrudMethod.POST)]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1X), CrudMethod.GET)]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1Y), CrudMethod.GET)]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestRestApiA1Z), CrudMethod.GET)]
        public void Method(Type applicationType, Type moduleType, Type resourceType, CrudMethod method)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var api = componentHub.RestApiManager.GetRestApi(module, resourceType);

            // test execution
            Assert.Contains(method, api.Methods);
        }

        /// <summary>
        /// Tests whether the rest api manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHub();

            // test execution
            foreach (var api in componentHub.RestApiManager.RestApis)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(api.GetType()), $"Api context {api.GetType().Name} does not implement IContext.");
            }
        }
    }
}
