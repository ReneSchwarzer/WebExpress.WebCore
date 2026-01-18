using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebParameter;
using WebExpress.WebCore.WebSession.Model;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the session manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestSessionManager
    {
        /// <summary>
        /// Test the register function of the session manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // act
            Assert.NotNull(componentHub.SessionManager);
        }

        /// <summary>
        /// Tests whether the session manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // act
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.SessionManager.GetType()));
        }

        /// <summary>
        /// Test the GetSession function of the session manager.
        /// </summary>
        [Fact]
        public void GetSession()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var request = UnitTestFixture.CrerateRequestMock();

            // act
            var session = componentHub.SessionManager.GetSession(request);

            Assert.NotNull(session);
        }

        /// <summary>
        /// Test the SetProperty function of the session manager.
        /// </summary>
        [Fact]
        public void AddPropertyToSession()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var request = UnitTestFixture.CrerateRequestMock();
            var session = componentHub.SessionManager.GetSession(request);

            // act
            session.SetProperty(new SessionPropertyParameter(new Parameter("test", "test param", ParameterScope.Session)));

            var testProperty = session.GetProperty<SessionPropertyParameter>();

            Assert.NotNull(testProperty);
            Assert.Single(testProperty.Params);
            Assert.Equal("test param", testProperty.Params["test"].Value);
            Assert.Equal(ParameterScope.Session, testProperty.Params["test"].Scope);
        }

        /// <summary>
        /// Test the RemoveProperty function of the session manager.
        /// </summary>
        [Fact]
        public void RemovePropertyFromSession()
        {
            // arrange
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var request = UnitTestFixture.CrerateRequestMock();
            var session = componentHub.SessionManager.GetSession(request);

            // act
            session.SetProperty(new SessionPropertyParameter(new Parameter("test", "test param", ParameterScope.Session)));
            session.RemoveProperty<SessionPropertyParameter>();

            var testProperty = session.GetProperty<SessionPropertyParameter>();

            Assert.Null(testProperty);
        }
    }
}
