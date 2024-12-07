using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the resource manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestResourceManager
    {
        /// <summary>
        /// Test the register function of the resource manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(12, componentHub.ResourceManager.Resources.Count());
        }

        /// <summary>
        /// Test the remove function of the resource manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var resourceManager = componentHub.ResourceManager as ResourceManager;

            // test execution
            resourceManager.Remove(plugin);

            Assert.Empty(componentHub.ResourceManager.Resources);
        }

        /// <summary>
        /// Test the id property of the resource.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceA), "webexpress.webcore.test.testresourcea")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceB), "webexpress.webcore.test.testresourceb")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceC), "webexpress.webcore.test.testresourcec")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceD), "webexpress.webcore.test.testresourced")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceA), "webexpress.webcore.test.testresourcea")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceB), "webexpress.webcore.test.testresourceb")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceC), "webexpress.webcore.test.testresourcec")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceD), "webexpress.webcore.test.testresourced")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceA), "webexpress.webcore.test.testresourcea")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceB), "webexpress.webcore.test.testresourceb")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceC), "webexpress.webcore.test.testresourcec")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceD), "webexpress.webcore.test.testresourced")]
        public void Id(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var resource = componentHub.ResourceManager.GetResorces(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, resource?.EndpointId.ToString());
        }

        /// <summary>
        /// Test the context path property of the resource.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceA), "/server/appa")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceB), "/server/appa/resa")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceC), "/server/appa")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceD), "/server/appa")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceA), "/server/appb")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceB), "/server/appb/resa")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceC), "/server/appb")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceD), "/server/appb")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceA), "/server")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceB), "/server/resa")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceC), "/server")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceD), "/server")]

        public void ContextPath(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var resource = componentHub.ResourceManager.GetResorces(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, resource.ContextPath);
        }

        /// <summary>
        /// Tests whether the resource manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.ResourceManager.GetType()));
        }

        /// <summary>
        /// Tests whether the resource context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var resources in componentHub.ResourceManager.Resources)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(resources.GetType()), $"Resource context {resources.GetType().Name} does not implement IContext.");
            }
        }
    }
}
