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

            // act & validation
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
            var plugin = componentHub.PluginManager?.GetPlugin(typeof(TestPlugin));
            var identityManager = componentHub.IdentityManager as IdentityManager;

            // act
            identityManager.Remove(plugin);

            // validation
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

            // act & validation
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

            // validation
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

            // validation
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

            // validation
            Assert.Equal(expected, access);
        }

        /// <summary>
        /// Test the Login function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData(null, false)]
        [InlineData("Alice", true)]
        [InlineData("Bob", true)]
        public void Login(string identityName, bool expected)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestFixture.CreateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);

            // act
            var res = identityManager.Login(identity, request);

            // validation
            Assert.Equal(expected, res is not null);
        }

        /// <summary>
        /// Test the Login function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData("Alice")]
        [InlineData("Bob")]
        [InlineData("Charlie")]
        public void Logout(string identityName)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestFixture.CreateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);
            identityManager.Login(identity, request);

            // act
            identityManager.Logout(request);

            // validation
            var res = identityManager.GetCurrentIdentity(request);
            Assert.Null(res);
        }

        /// <summary>
        /// Test the GetCurrentIdentity function of the identity manager.
        /// </summary>
        [Theory]
        [InlineData("Alice")]
        [InlineData("Bob")]
        [InlineData("Charlie")]
        public void GetCurrentIdentity(string identityName)
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestFixture.CreateRequestMock();
            var identity = MockIdentityFactory.GetIdentity(identityName);
            identityManager.Login(identity, request);

            // act
            var res = identityManager.GetCurrentIdentity(request);

            // validation
            Assert.Equal(identity, res);
        }

        /// <summary>
        /// Signing in must move the session to an id the client did not hold before: the
        /// request keeps its session and identity, the new id resolves to the signed-in
        /// session, and the id in use before the sign-in resolves to nothing.
        /// </summary>
        [Fact]
        public void Login_ReplacesSessionId()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestFixture.CreateRequestMock();
            var identity = MockIdentityFactory.GetIdentity("Alice");
            var idBefore = request.Session.Id;

            // act
            var session = identityManager.Login(identity, request);

            // validation
            Assert.NotEqual(idBefore, session.Id);
            Assert.Same(request.Session, session);
            Assert.Equal(identity, identityManager.GetCurrentIdentity(request));

            var withOldId = UnitTestFixture.CreateRequestMock($"GET / HTTP/1.1\nCookie: session={idBefore}\n\n");
            Assert.Null(identityManager.GetCurrentIdentity(withOldId));

            var withNewId = UnitTestFixture.CreateRequestMock($"GET / HTTP/1.1\nCookie: session={session.Id}\n\n");
            Assert.Equal(identity, identityManager.GetCurrentIdentity(withNewId));
        }

        /// <summary>
        /// The fixation scenario end to end: an attacker plants a session id in the victim's
        /// browser, the victim signs in with it, and the attacker's requests carrying that id
        /// must still see nobody signed in.
        /// </summary>
        [Fact]
        public void Login_PlantedSessionId_DoesNotReachTheAttacker()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var planted = Guid.NewGuid();
            var victim = UnitTestFixture.CreateRequestMock($"GET / HTTP/1.1\nCookie: session={planted}\n\n");
            var identity = MockIdentityFactory.GetIdentity("Alice");

            // act
            var session = identityManager.Login(identity, victim);

            // validation
            Assert.NotNull(session);
            Assert.NotEqual(planted, session.Id);

            var attacker = UnitTestFixture.CreateRequestMock($"GET / HTTP/1.1\nCookie: session={planted}\n\n");
            Assert.Null(identityManager.GetCurrentIdentity(attacker));
        }

        /// <summary>
        /// Signing out retires the id the session was signed in under: the request keeps its
        /// (now anonymous) session, while a copy of the signed-in id resolves to nothing.
        /// </summary>
        [Fact]
        public void Logout_ReplacesSessionId()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var request = UnitTestFixture.CreateRequestMock();
            var identity = MockIdentityFactory.GetIdentity("Alice");
            var session = identityManager.Login(identity, request);
            var signedInId = session.Id;

            // act
            identityManager.Logout(request);

            // validation
            Assert.NotEqual(signedInId, session.Id);
            Assert.Same(request.Session, session);
            Assert.Null(identityManager.GetCurrentIdentity(request));

            var withSignedInId = UnitTestFixture.CreateRequestMock($"GET / HTTP/1.1\nCookie: session={signedInId}\n\n");
            Assert.NotSame(session, componentHub.SessionManager.GetSession(withSignedInId));

            var withNewId = UnitTestFixture.CreateRequestMock($"GET / HTTP/1.1\nCookie: session={session.Id}\n\n");
            Assert.Same(session, componentHub.SessionManager.GetSession(withNewId));
        }

        /// <summary>
        /// Test that the IIdentityGroup interface has the Id and Name properties.
        /// </summary>
        [Fact]
        public void IIdentityGroupHasIdAndName()
        {
            // arrange
            var group = MockIdentityFactory.GetIdentityGroup("Admins");

            // act & validation
            Assert.NotNull(group);
            Assert.IsType<IIdentityGroup>(group, exactMatch: false);
            Assert.NotEqual(Guid.Empty, group.Id);
            Assert.Equal("Admins", group.Name);
        }

        /// <summary>
        /// Tests that an identity provider can be registered and that its identities
        /// are returned by the IdentityManager for the given application context.
        /// </summary>
        [Fact]
        public void RegisterIdentityProvider()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var applicationContext = componentHub.ApplicationManager.GetApplications(typeof(TestApplicationA)).FirstOrDefault();
            var provider = new MockIdentityProvider();
            var identity = MockIdentityFactory.GetIdentity("alice@example.com");

            provider.Identities.Add(identity);

            // act
            identityManager.RegisterIdentityProvider(provider, applicationContext);
            var identities = identityManager.GetIdentities(applicationContext).ToList();

            // validation
            Assert.Contains(identity, identities);
            Assert.Single(identities);
        }

        /// <summary>
        /// Tests that an identity provider can be unregistered and that its identities
        /// are no longer returned by the IdentityManager for the given application context.
        /// </summary>
        [Fact]
        public void UnregisterIdentityProvider()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var identityManager = componentHub.IdentityManager as IdentityManager;
            var applicationContext = componentHub.ApplicationManager
                .GetApplications(typeof(TestApplicationA))
                .FirstOrDefault();

            var provider = new MockIdentityProvider();
            var identity = MockIdentityFactory.GetIdentity("alice@example.com");

            provider.Identities.Add(identity);

            identityManager.RegisterIdentityProvider(provider, applicationContext);

            var identitiesBefore = identityManager.GetIdentities(applicationContext).ToList();
            Assert.Contains(identity, identitiesBefore);
            Assert.Single(identitiesBefore);

            // act
            var removed = identityManager.UnregisterIdentityProvider(provider, applicationContext);
            var identitiesAfter = identityManager.GetIdentities(applicationContext).ToList();

            // validation
            Assert.True(removed);
            Assert.DoesNotContain(identity, identitiesAfter);
            Assert.Empty(identitiesAfter);
        }
    }
}
