using System.Security;
using WebExpress.WebCore.Test.Data;
using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the identity manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestIdentityManager
    {
        /// <summary>
        /// Test the register function of the identity manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // act
            Assert.Equal(9, componentHub.IdentityManager.Permissions.Count());
            Assert.Equal(6, componentHub.IdentityManager.Policies.Count());
        }

        /// <summary>
        /// Test the remove function of the identity manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var identityManager = componentHub.IdentityManager as IdentityManager;

            // act
            identityManager.Remove(plugin);

            Assert.Empty(componentHub.IdentityManager.Permissions);
            Assert.Empty(componentHub.IdentityManager.Policies);
        }

        /// <summary>
        /// Tests whether the identity manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // act
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.IdentityManager.GetType()));
        }

        /// <summary>
        /// Test the CheckAccess function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "Alice", typeof(TestIdentityPermissionA), true)]
        [InlineData(typeof(TestApplicationA), "Alice", typeof(TestIdentityPermissionB), true)]
        [InlineData(typeof(TestApplicationA), "Alice", typeof(TestIdentityPermissionC), true)]
        [InlineData(typeof(TestApplicationA), "Bob", typeof(TestIdentityPermissionA), true)]
        [InlineData(typeof(TestApplicationA), "Bob", typeof(TestIdentityPermissionB), true)]
        [InlineData(typeof(TestApplicationA), "Bob", typeof(TestIdentityPermissionC), false)]
        [InlineData(typeof(TestApplicationA), "Charlie", typeof(TestIdentityPermissionA), false)]
        [InlineData(typeof(TestApplicationA), "Charlie", typeof(TestIdentityPermissionB), false)]
        [InlineData(typeof(TestApplicationA), "Charlie", typeof(TestIdentityPermissionC), false)]
        public void CheckAccessIdentity(Type application, string identityName, Type permission, bool expected)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var applicationContext = componentHub.ApplicationManager.GetApplications(application).FirstOrDefault();
            var identity = MockIdentityFactory.GetIdentity(identityName);

            // act
            var access = identityManager.CheckAccess(applicationContext, identity, permission);

            Assert.Equal(expected, access);
        }

        /// <summary>
        /// Test the CheckAccess function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), "Admins", typeof(TestIdentityPermissionA), true)]
        [InlineData(typeof(TestApplicationA), "Admins", typeof(TestIdentityPermissionB), true)]
        [InlineData(typeof(TestApplicationA), "Admins", typeof(TestIdentityPermissionC), true)]
        [InlineData(typeof(TestApplicationA), "Users", typeof(TestIdentityPermissionA), true)]
        [InlineData(typeof(TestApplicationA), "Users", typeof(TestIdentityPermissionB), true)]
        [InlineData(typeof(TestApplicationA), "Users", typeof(TestIdentityPermissionC), false)]
        [InlineData(typeof(TestApplicationA), "Guests", typeof(TestIdentityPermissionA), false)]
        [InlineData(typeof(TestApplicationA), "Guests", typeof(TestIdentityPermissionB), false)]
        [InlineData(typeof(TestApplicationA), "Guests", typeof(TestIdentityPermissionC), false)]
        public void CheckAccessGroup(Type application, string groupName, Type permission, bool expected)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var applicationContext = componentHub.ApplicationManager.GetApplications(application).FirstOrDefault();
            var group = MockIdentityFactory.GetIdentityGroup(groupName);

            // act
            var access = identityManager.CheckAccess(applicationContext, group, permission);

            Assert.Equal(expected, access);
        }

        /// <summary>
        /// Test the CheckAccess function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityPolicyA), typeof(TestIdentityPermissionA), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityPolicyA), typeof(TestIdentityPermissionB), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityPolicyA), typeof(TestIdentityPermissionC), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityPolicyB), typeof(TestIdentityPermissionA), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityPolicyB), typeof(TestIdentityPermissionB), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityPolicyB), typeof(TestIdentityPermissionC), false)]
        public void CheckAccess(Type application, Type policy, Type permission, bool expected)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var applicationContext = componentHub.ApplicationManager.GetApplications(application).FirstOrDefault();

            // act
            var access = identityManager.CheckAccess(applicationContext, policy, permission);

            Assert.Equal(expected, access);
        }

        /// <summary>
        /// Test the Login function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData("Alice", "abc", true)]
        [InlineData("Alice", "123", false)]
        public void Login(string identityName, string password, bool expected)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestFixture.CreateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);
            var securePassword = new SecureString();
            password.ToList().ForEach(x => securePassword.AppendChar(x));
            securePassword.MakeReadOnly();

            // act
            var res = identityManager.Login(request, identity, securePassword);

            Assert.Equal(expected, res);
        }

        /// <summary>
        /// Test the Login function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData("Alice", "abc")]
        [InlineData("Bob", "abc")]
        [InlineData("Charlie", "abc")]
        public void Logout(string identityName, string password)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestFixture.CreateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);
            var securePassword = new SecureString();
            password.ToList().ForEach(x => securePassword.AppendChar(x));
            securePassword.MakeReadOnly();
            identityManager.Login(request, identity, securePassword);

            // act
            identityManager.Logout(request);

            var res = identityManager.GetCurrentIdentity(request);
            Assert.Null(res);
        }

        /// <summary>
        /// Test the GetCurrentIdentity function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData("Alice", "abc")]
        [InlineData("Bob", "abc")]
        [InlineData("Charlie", "abc")]
        public void GetCurrentIdentity(string identityName, string password)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestFixture.CreateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);
            var securePassword = new SecureString();
            password.ToList().ForEach(x => securePassword.AppendChar(x));
            securePassword.MakeReadOnly();
            identityManager.Login(request, identity, securePassword);

            // act
            var res = identityManager.GetCurrentIdentity(request);

            Assert.Equal(identity, res);
        }
    }
}
