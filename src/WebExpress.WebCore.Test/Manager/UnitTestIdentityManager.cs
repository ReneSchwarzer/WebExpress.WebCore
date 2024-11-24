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
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(9, componentHub.IdentityManager.Permissions.Count());
            Assert.Equal(6, componentHub.IdentityManager.Roles.Count());
        }

        /// <summary>
        /// Test the remove function of the identity manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var identityManager = componentHub.IdentityManager as IdentityManager;

            // test execution
            identityManager.Remove(plugin);

            Assert.Empty(componentHub.IdentityManager.Permissions);
            Assert.Empty(componentHub.IdentityManager.Roles);
        }

        /// <summary>
        /// Tests whether the identity manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();

            // test execution
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
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var applicationContext = componentHub.ApplicationManager.GetApplications(application).FirstOrDefault();
            var identity = MockIdentityFactory.GetIdentity(identityName);

            // test execution
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
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var applicationContext = componentHub.ApplicationManager.GetApplications(application).FirstOrDefault();
            var group = MockIdentityFactory.GetIdentityGroup(groupName);

            // test execution
            var access = identityManager.CheckAccess(applicationContext, group, permission);

            Assert.Equal(expected, access);
        }

        /// <summary>
        /// Test the CheckAccess function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityRoleA), typeof(TestIdentityPermissionA), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityRoleA), typeof(TestIdentityPermissionB), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityRoleA), typeof(TestIdentityPermissionC), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityRoleB), typeof(TestIdentityPermissionA), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityRoleB), typeof(TestIdentityPermissionB), true)]
        [InlineData(typeof(TestApplicationA), typeof(TestIdentityRoleB), typeof(TestIdentityPermissionC), false)]
        public void CheckAccessRole(Type application, Type role, Type permission, bool expected)
        {
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var applicationContext = componentHub.ApplicationManager.GetApplications(application).FirstOrDefault();

            // test execution
            var access = identityManager.CheckAccess(applicationContext, role, permission);

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
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestControlFixture.CrerateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);
            var securePassword = new SecureString();
            password.ToList().ForEach(x => securePassword.AppendChar(x));
            securePassword.MakeReadOnly();

            // test execution
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
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestControlFixture.CrerateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);
            var securePassword = new SecureString();
            password.ToList().ForEach(x => securePassword.AppendChar(x));
            securePassword.MakeReadOnly();
            identityManager.Login(request, identity, securePassword);

            // test execution
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
            // preconditions
            var componentHub = UnitTestControlFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestControlFixture.CrerateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);
            var securePassword = new SecureString();
            password.ToList().ForEach(x => securePassword.AppendChar(x));
            securePassword.MakeReadOnly();
            identityManager.Login(request, identity, securePassword);

            // test execution
            var res = identityManager.GetCurrentIdentity(request);

            Assert.Equal(identity, res);
        }
    }
}
