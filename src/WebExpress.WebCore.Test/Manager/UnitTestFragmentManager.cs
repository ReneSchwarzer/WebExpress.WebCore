using System.Text;
using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebFragment;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the fragment manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestFragmentManager
    {
        /// <summary>
        /// Test the register function of the fragment manager.
        /// </summary>
        [Fact]
        public void Register()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(9, componentHub.FragmentManager.Fragments.Count());
        }

        /// <summary>
        /// Test the remove function of the fragment manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var fragmentManager = componentHub.FragmentManager as FragmentManager;
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));

            // test execution
            fragmentManager.Remove(plugin);

            Assert.Empty(componentHub.FragmentManager.Fragments);
        }

        /// <summary>
        /// Tests whether the fragment manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.EventManager.GetType()));
        }

        /// <summary>
        /// Test the id property of the fragment handler.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestFragmentA), "webexpress.webcore.test.testfragmenta")]
        [InlineData(typeof(TestApplicationB), typeof(TestFragmentA), "webexpress.webcore.test.testfragmenta")]
        [InlineData(typeof(TestApplicationC), typeof(TestFragmentA), "webexpress.webcore.test.testfragmenta")]
        public void Id(Type applicationType, Type fragmentType, string id)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType).FirstOrDefault();

            // test execution
            var fragment = componentHub.FragmentManager.GetFragments(application, fragmentType);

            if (id == null)
            {
                Assert.Empty(fragment);
                return;
            }

            Assert.Contains(id, fragment.Select(x => x.FragmentId));
        }

        /// <summary>
        /// Test the process function of the fragment handler.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSectionA), typeof(TestScopeA), "TestFragmentA")]
        [InlineData(typeof(TestApplicationA), typeof(TestSectionA), typeof(TestScopeB), "TestFragmentB")]
        [InlineData(typeof(TestApplicationA), typeof(TestSectionA), typeof(TestPageB), "TestFragmentA")]
        public void Process(Type applicationType, Type sectionType, Type scopeType, string expected)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType).FirstOrDefault();
            var renderContext = UnitTestFixture.CrerateRenderContextMock(application, [scopeType]);
            var builder = new StringBuilder();

            // test execution
            componentHub.FragmentManager.Process(renderContext, sectionType);
            renderContext.VisualTree.Content.ToString(builder, 0);

            Assert.Contains(expected, builder.ToString());
        }
    }
}
