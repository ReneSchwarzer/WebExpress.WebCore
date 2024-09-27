using WebExpress.WebCore.Test.Fixture;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the application manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestApplication
    {
        /// <summary>
        /// Test the register function of the application manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();

            // test execution
            componentManager.PluginManager.Register();

            Assert.Equal(3, componentManager.ApplicationManager.Applications.Count());
            Assert.Equal("webexpress.webcore.test.testapplicationa", componentManager.ApplicationManager.GetApplcation(typeof(TestApplicationA))?.ApplicationId);
            Assert.Equal("webexpress.webcore.test.testapplicationb", componentManager.ApplicationManager.GetApplcation(typeof(TestApplicationB))?.ApplicationId);
            Assert.Equal("testapplicationc", componentManager.ApplicationManager.GetApplcation(typeof(TestApplicationC))?.ApplicationId);
        }

        /// <summary>
        /// Test the remove function of the application manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            componentManager.ApplicationManager.Remove(plugin);

            Assert.Empty(componentManager.ApplicationManager.Applications);
        }

        /// <summary>
        /// Test the name property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "webexpress.webcore.test.testapplicationa")]
        [InlineData(typeof(TestApplicationB), "webexpress.webcore.test.testapplicationb")]
        [InlineData(typeof(TestApplicationC), "testapplicationc")]
        public void GetId(Type applicationType, string id)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(id, applcation.ApplicationId);
        }

        /// <summary>
        /// Test the name property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "TestApplicationA")]
        [InlineData(typeof(TestApplicationB), "TestApplicationB")]
        [InlineData(typeof(TestApplicationC), "TestApplicationC")]
        public void GetName(Type applicationType, string name)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(name, applcation.ApplicationName);
        }

        /// <summary>
        /// Test the description property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "application.description")]
        [InlineData(typeof(TestApplicationB), "application.description")]
        [InlineData(typeof(TestApplicationC), "application.description")]
        public void GetDescription(Type applicationType, string description)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(description, applcation.Description);
        }

        /// <summary>
        /// Test the icon property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationB), "/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationC), "/assets/img/Logo.png")]
        public void GetIcon(Type applicationType, string icon)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateComponentManager();
            componentManager.PluginManager.Register();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(icon, applcation.Icon);
        }
    }
}
