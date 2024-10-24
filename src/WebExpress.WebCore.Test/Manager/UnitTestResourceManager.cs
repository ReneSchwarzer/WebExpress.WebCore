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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var resource = componentHub.ResourceManager.GetResorces(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, resource?.EndpointId);
        }

        /// <summary>
        /// Test the context path property of the resource.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceA), "/appa")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceB), "/appa/resa")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceC), "/appa")]
        [InlineData(typeof(TestApplicationA), typeof(TestResourceD), "/appa")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceA), "/appb")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceB), "/appb/resa")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceC), "/appb")]
        [InlineData(typeof(TestApplicationB), typeof(TestResourceD), "/appb")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceA), "/")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceB), "/resa")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceC), "/")]
        [InlineData(typeof(TestApplicationC), typeof(TestResourceD), "/")]

        public void ContextPath(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

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
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var resources in componentHub.ResourceManager.Resources)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(resources.GetType()), $"Resource context {resources.GetType().Name} does not implement IContext.");
            }
        }
    }
}
