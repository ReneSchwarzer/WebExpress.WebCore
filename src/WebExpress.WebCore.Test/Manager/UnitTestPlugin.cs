using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the plugin manager.
    /// </summary>
    /// <param name="fixture">The fixture.</param>
    [Collection("NonParallelTests")]
    public class UnitTestPlugin(UnitTestControlFixture fixture) : IClassFixture<UnitTestControlFixture>
    {
        private static readonly object _lock = new object();

        /// <summary>
        /// Test the register function of the plugin manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            lock (_lock)
            {
                // test execution
                ComponentManager.PluginManager.Register();

                Assert.Single(ComponentManager.PluginManager.Plugins);
                Assert.Equal("webexpress.webcore.test.testplugin", ComponentManager.PluginManager.Plugins.FirstOrDefault().PluginId);

                // postconditions
                ComponentManager.PluginManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
            }
        }

        /// <summary>
        /// Test the register function of the plugin manager.
        /// </summary>
        [Fact]
        public void RegisterAssembly()
        {
            lock (_lock)
            {
                // test execution
                fixture.RegisterPlugin(typeof(TestPlugin));

                Assert.Single(ComponentManager.PluginManager.Plugins);
                Assert.Equal("webexpress.webcore.test.testplugin", ComponentManager.PluginManager.Plugins.FirstOrDefault().PluginId);

                // postconditions
                ComponentManager.PluginManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
            }
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void RegisterEvent()
        {
            lock (_lock)
            {
                // preconditions
                var i = 0;
                bool triggered = false;
                ComponentManager.PluginManager.AddPlugin += (s, e) => { i++; triggered = true; };

                // test execution
                fixture.RegisterPlugin(typeof(TestPlugin));

                Assert.Single(ComponentManager.PluginManager.Plugins);
                Assert.Equal("webexpress.webcore.test.testplugin", ComponentManager.PluginManager.Plugins.FirstOrDefault().PluginId);
                Assert.Equal(1, i);
                Assert.True(triggered);

                // postconditions
                ComponentManager.PluginManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
            }
        }

        /// <summary>
        /// Test the remove function of the plugin manager.
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
                ComponentManager.PluginManager.Remove(plugin);

                Assert.Empty(ComponentManager.PluginManager.Plugins);
            }
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void RemoveEvent()
        {
            lock (_lock)
            {
                // preconditions
                var i = 1;
                ComponentManager.PluginManager.RemovePlugin += (s, e) => i--;
                fixture.RegisterPlugin(typeof(TestPlugin));
                var plugin = ComponentManager.PluginManager.Plugins.FirstOrDefault();

                // test execution
                ComponentManager.PluginManager.Remove(plugin);

                Assert.Empty(ComponentManager.PluginManager.Plugins);
                Assert.Equal(0, i);
            }
        }

        /// <summary>
        /// Test the get plugin function of the plugin manager.
        /// </summary>
        [Fact]
        public void GetPluginById()
        {
            lock (_lock)
            {
                // preconditions
                fixture.RegisterPlugin(typeof(TestPlugin));
                var plugin1 = ComponentManager.PluginManager.Plugins.FirstOrDefault();

                // test execution
                var plugin2 = ComponentManager.PluginManager.GetPlugin(plugin1.PluginId);

                Assert.Equal(plugin1, plugin2);

                // postconditions
                ComponentManager.PluginManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
            }
        }

        /// <summary>
        /// Test the get plugin function of the plugin manager.
        /// </summary>
        [Fact]
        public void GetPluginByType()
        {
            lock (_lock)
            {
                // preconditions
                fixture.RegisterPlugin(typeof(TestPlugin));
                var plugin1 = ComponentManager.PluginManager.Plugins.FirstOrDefault();

                // test execution
                var plugin2 = ComponentManager.PluginManager.GetPlugin(typeof(TestPlugin));

                Assert.Equal(plugin1, plugin2);

                // postconditions
                ComponentManager.PluginManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
            }
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void Boot()
        {
            lock (_lock)
            {
                // preconditions
                fixture.RegisterPlugin(typeof(TestPlugin));
                var plugin = ComponentManager.PluginManager.Plugins.FirstOrDefault();

                // test execution
                ComponentManager.PluginManager.Boot(ComponentManager.PluginManager.Plugins.FirstOrDefault());

                Assert.Single(ComponentManager.PluginManager.Plugins);

                // postconditions
                ComponentManager.PluginManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
            }
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void ShutDown()
        {
            lock (_lock)
            {
                // preconditions
                fixture.RegisterPlugin(typeof(TestPlugin));
                var plugin = ComponentManager.PluginManager.Plugins.FirstOrDefault();

                // test execution
                ComponentManager.PluginManager.ShutDown(ComponentManager.PluginManager.Plugins.FirstOrDefault());

                Assert.Single(ComponentManager.PluginManager.Plugins);

                // postconditions
                ComponentManager.PluginManager.Remove(ComponentManager.PluginManager.Plugins.FirstOrDefault());
            }
        }
    }
}
