﻿using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the plugin manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestPlugin
    {
        /// <summary>
        /// Test the register function of the plugin manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();

            // test execution
            componentManager.PluginManager.Register();

            Assert.Single(componentManager.PluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", componentManager.PluginManager.GetPlugin(typeof(TestPlugin))?.PluginId);
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void RegisterEvent()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            var i = 0;
            var triggered = false;

            componentManager.PluginManager.AddPlugin += (s, e) => { i++; triggered = true; };

            // test execution
            componentManager.PluginManager.Register();

            Assert.Single(componentManager.PluginManager.Plugins);
            Assert.Contains("webexpress.webcore.test", componentManager.PluginManager.GetPlugin(typeof(TestPlugin))?.PluginId);
            Assert.Equal(1, i);
            Assert.True(triggered);
        }

        /// <summary>
        /// Test the remove function of the plugin manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            componentManager.PluginManager.Remove(plugin);

            Assert.Empty(componentManager.PluginManager.Plugins);
        }

        /// <summary>
        /// Test the event of the plugin manager.
        /// </summary>
        [Fact]
        public void RemoveEvent()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            var i = 1;
            var triggered = false;

            componentManager.PluginManager.RemovePlugin += (s, e) => { i--; triggered = true; };
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            componentManager.PluginManager.Remove(plugin);

            Assert.Empty(componentManager.PluginManager.Plugins);
            Assert.Equal(0, i);
            Assert.True(triggered);
        }

        /// <summary>
        /// Test the get plugin function of the plugin manager.
        /// </summary>
        [Fact]
        public void GetPluginById()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();

            // test execution
            var plugin = componentManager.PluginManager.GetPlugin("webexpress.webcore.test");

            Assert.Equal("webexpress.webcore.test", plugin?.PluginId);
        }

        /// <summary>
        /// Test the name property of the plugin.
        /// </summary>
        [Fact]
        public void GetId()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            Assert.Equal(typeof(TestPlugin).Namespace.ToLower(), plugin.PluginId);
        }

        /// <summary>
        /// Test the name property of the plugin.
        /// </summary>
        [Fact]
        public void GetName()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            Assert.Equal("TestPlugin", plugin.PluginName);
        }

        /// <summary>
        /// Test the description property of the plugin.
        /// </summary>
        [Fact]
        public void GetDescription()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            Assert.Equal("plugin.description", plugin.Description);
        }

        /// <summary>
        /// Test the icon property of the plugin.
        /// </summary>
        [Fact]
        public void GetIcon()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            Assert.Equal("/assets/img/Logo.png", plugin.Icon);
        }

        /// <summary>
        /// Test the get plugin function of the plugin manager.
        /// </summary>
        [Fact]
        public void GetPluginByType()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();

            // test execution
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            Assert.Equal("webexpress.webcore.test", plugin?.PluginId);
        }

        /// <summary>
        /// Test the boot function of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test", "webexpress.webcore.test")]
        [InlineData("non.existent.plugin", null)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void Boot(string pluginId, string expected)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(pluginId);

            // test execution
            componentManager.PluginManager.Boot(plugin);

            Assert.Single(componentManager.PluginManager.Plugins);
            Assert.Equal(expected, plugin?.PluginId);
        }

        /// <summary>
        /// Test the shut down of the plugin manager.
        /// </summary>
        [Theory]
        [InlineData("webexpress.webcore.test", "webexpress.webcore.test")]
        [InlineData("non.existent.plugin", null)]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void ShutDown(string pluginId, string expected)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(pluginId);

            // test execution
            componentManager.PluginManager.ShutDown(plugin);

            Assert.Single(componentManager.PluginManager.Plugins);
            Assert.Equal(expected, plugin?.PluginId);
        }
    }
}
