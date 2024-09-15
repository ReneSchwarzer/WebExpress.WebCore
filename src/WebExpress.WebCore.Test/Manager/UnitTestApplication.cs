using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the application manager.
    /// </summary>
    /// <param name="fixture">The fixture.</param>
    [Collection("NonParallelTests")]
    public class UnitTestApplication(UnitTestControlFixture fixture) : IClassFixture<UnitTestControlFixture>
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Test the register function of the application manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            lock (_lock)
            {
                // preconditions
                var i = 0;
                bool triggered = false;
                fixture.RegisterPlugin(typeof(TestPlugin));
                var plugin = ComponentManager.PluginManager.Plugins.FirstOrDefault();
                ComponentManager.ApplicationManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
                ComponentManager.ApplicationManager.AddApplication += (s, e) => { i++; triggered = true; };

                // test execution
                ComponentManager.ApplicationManager.Register(plugin);

                Assert.Single(ComponentManager.ApplicationManager.Applications);
                Assert.Equal("webexpress.webcore.test.testapplication", ComponentManager.ApplicationManager.Applications.FirstOrDefault().ApplicationId);
                Assert.Equal(1, i);
                Assert.True(triggered);

                // postconditions
                ComponentManager.ApplicationManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
            }
        }

        /// <summary>
        /// Test the remove function of the application manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            lock (_lock)
            {
                // preconditions
                fixture.RegisterPlugin(typeof(TestPlugin));
                var plugin = ComponentManager.PluginManager.Plugins.FirstOrDefault();

                // test execution
                ComponentManager.ApplicationManager.Remove(plugin);

                Assert.Empty(ComponentManager.ApplicationManager.Applications);
            }
        }
    }
}
