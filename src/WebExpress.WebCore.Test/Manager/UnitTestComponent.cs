using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the component manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestComponent
    {
        /// <summary>
        /// Test the plugin manager property of the component manager.
        /// </summary>
        [Fact]
        public void PluginManager()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();

            // test execution
            Assert.NotNull(componentManager.PluginManager);
        }

        /// <summary>
        /// Test the application manager property of the component manager.
        /// </summary>
        [Fact]
        public void ApplicationManager()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();

            // test execution
            Assert.NotNull(componentManager.ApplicationManager);
        }

        /// <summary>
        /// Test the module manager property of the component manager.
        /// </summary>
        [Fact]
        public void ModuleManager()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();

            // test execution
            Assert.NotNull(componentManager.ModuleManager);
        }
    }
}
