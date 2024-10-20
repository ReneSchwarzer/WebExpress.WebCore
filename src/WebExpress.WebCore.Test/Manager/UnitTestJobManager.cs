using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebJob;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the job manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestJobManager
    {
        /// <summary>
        /// Test the register function of the job manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(1, componentHub.JobManager.Jobs.Count());
        }

        /// <summary>
        /// Test the remove function of the job manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var jobManager = componentHub.JobManager as JobManager;

            // test execution
            jobManager.Remove(plugin);

            Assert.Empty(componentHub.JobManager.Jobs);
        }

        /// <summary>
        /// Tests whether the job manager implements interface IComponentManager.
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
        /// Tests whether the job context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var application in componentHub.ResourceManager.Resources)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(application.GetType()), $"Resource context {application.GetType().Name} does not implement IContext.");
            }
        }

        /// <summary>
        /// Test the id property of the job.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestResourceA1X), "webexpress.webcore.test.testresourcea1x")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA1), typeof(TestResourceA1Y), "webexpress.webcore.test.testresourcea1y")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleA2), typeof(TestResourceA2X), "webexpress.webcore.test.testresourcea2x")]
        [InlineData(typeof(TestApplicationA), typeof(TestModuleAB1), typeof(TestResourceAB1X), "webexpress.webcore.test.testresourceab1x")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), typeof(TestResourceAB1X), "webexpress.webcore.test.testresourceab1x")]
        [InlineData(typeof(TestApplicationB), typeof(TestModuleAB1), typeof(TestPageA1X), null)]

        public void Id(Type applicationType, Type moduleType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var module = componentHub.ModuleManager.GetModule(applicationType, moduleType);
            var resource = componentHub.ResourceManager.GetResorce(module, resourceType);

            // test execution
            Assert.Equal(id, resource?.EndpointId);
        }




    }
}
