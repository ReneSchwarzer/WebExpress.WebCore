using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebLog;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the log manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestLogManager
    {
        /// <summary>
        /// Test the register function of the log manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateComponentHubMock();
            var logManager = componentHub.LogManager as LogManager;

            // test execution
            Assert.NotNull(logManager);
        }

        /// <summary>
        /// Test the remove function of the log manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateComponentHubMock();
            var logManager = componentHub.LogManager as LogManager;

            // test execution
            Assert.NotNull(logManager);
        }

        /// <summary>
        /// Tests whether the log manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var logManager = componentHub.LogManager as LogManager;

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(logManager.GetType()));
        }
    }
}
