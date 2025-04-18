using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPackage;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the package manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestPackageManager
    {
        /// <summary>
        /// Test the register function of the package manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateComponentHubMock();
            var packageManager = componentHub.PackageManager as PackageManager;

            // test execution
            Assert.NotNull(packageManager);
        }

        /// <summary>
        /// Test the remove function of the package manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateComponentHubMock();
            var packageManager = componentHub.PackageManager as PackageManager;

            // test execution
            Assert.NotNull(packageManager);
        }

        /// <summary>
        /// Tests whether the package manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var packageManager = componentHub.PackageManager as PackageManager;

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(packageManager.GetType()));
        }
    }
}
