using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebPlugin;

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
            var pluginManager = componentManager.PluginManager as PluginManager;

            // test execution
            pluginManager.Register();

            Assert.Equal(3, componentManager.ApplicationManager.Applications.Count());
            Assert.Equal("webexpress.webcore.test.testapplicationa", componentManager.ApplicationManager.GetApplcation(typeof(TestApplicationA))?.ApplicationId);
            Assert.Equal("webexpress.webcore.test.testapplicationb", componentManager.ApplicationManager.GetApplcation(typeof(TestApplicationB))?.ApplicationId);
            Assert.Equal("webexpress.webcore.test.testapplicationc", componentManager.ApplicationManager.GetApplcation(typeof(TestApplicationC))?.ApplicationId);
        }

        /// <summary>
        /// Test the remove function of the application manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var applicationManager = componentManager.ApplicationManager as ApplicationManager;
            var plugin = componentManager.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            applicationManager.Remove(plugin);

            Assert.Empty(componentManager.ApplicationManager.Applications);
        }

        /// <summary>
        /// Test the name property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "webexpress.webcore.test.testapplicationa")]
        [InlineData(typeof(TestApplicationB), "webexpress.webcore.test.testapplicationb")]
        [InlineData(typeof(TestApplicationC), "webexpress.webcore.test.testapplicationc")]
        public void Id(Type applicationType, string id)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
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
        public void Name(Type applicationType, string name)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
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
        public void Description(Type applicationType, string description)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(description, applcation.Description);
        }

        /// <summary>
        /// Test the icon property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/aca/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationB), "/acb/assets/img/Logo.png")]
        [InlineData(typeof(TestApplicationC), "/assets/img/Logo.png")]
        public void Icon(Type applicationType, string icon)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(icon, applcation.Icon);
        }

        /// <summary>
        /// Test the context path property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/aca")]
        [InlineData(typeof(TestApplicationB), "/acb")]
        [InlineData(typeof(TestApplicationC), "/")]
        public void ContextPath(Type applicationType, string contextPath)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(contextPath, applcation.ContextPath);
        }

        /// <summary>
        /// Test the asset path property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/aaa")]
        [InlineData(typeof(TestApplicationB), "/aab")]
        [InlineData(typeof(TestApplicationC), "/")]
        public void AssetPath(Type applicationType, string assetPath)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(assetPath, applcation.AssetPath);
        }

        /// <summary>
        /// Test the data path property of the application.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "/ada")]
        [InlineData(typeof(TestApplicationB), "/adb")]
        [InlineData(typeof(TestApplicationC), "/")]
        public void DataPath(Type applicationType, string dataPath)
        {
            // preconditions
            var componentManager = UnitTestControlFixture.CreateAndRegisterComponentManager();
            var applcation = componentManager.ApplicationManager.GetApplcation(applicationType);

            // test execution
            Assert.Equal(dataPath, applcation.DataPath);
        }
    }
}
