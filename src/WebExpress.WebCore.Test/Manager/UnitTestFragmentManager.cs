using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebFragment;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebScope;

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
            Assert.Equal(12, componentHub.FragmentManager.Fragments.Count());
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
        /// Test the id property of the fragment.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestFragmentA), "webexpress.webcore.test.testfragmenta")]
        [InlineData(typeof(TestApplicationB), typeof(TestFragmentA), "webexpress.webcore.test.testfragmenta")]
        [InlineData(typeof(TestApplicationC), typeof(TestFragmentA), "webexpress.webcore.test.testfragmenta")]
        [InlineData(typeof(TestApplicationA), typeof(TestFragmentB), "webexpress.webcore.test.testfragmentb")]
        [InlineData(typeof(TestApplicationB), typeof(TestFragmentB), "webexpress.webcore.test.testfragmentb")]
        [InlineData(typeof(TestApplicationC), typeof(TestFragmentB), "webexpress.webcore.test.testfragmentb")]
        [InlineData(typeof(TestApplicationA), typeof(TestFragmentC), "webexpress.webcore.test.testfragmentc")]
        [InlineData(typeof(TestApplicationB), typeof(TestFragmentC), "webexpress.webcore.test.testfragmentc")]
        [InlineData(typeof(TestApplicationC), typeof(TestFragmentC), "webexpress.webcore.test.testfragmentc")]
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

            Assert.Contains(id, fragment.Select(x => x.FragmentId?.ToString()));
        }

        /// <summary>
        /// Test the get fragment function of the fragment.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(IScope), 0)]
        [InlineData(typeof(TestApplicationA), typeof(TestScopeA), 1)]
        [InlineData(typeof(TestApplicationA), typeof(TestPageB), 1)]
        [InlineData(typeof(TestApplicationB), typeof(IScope), 0)]
        [InlineData(typeof(TestApplicationB), typeof(TestPageB), 1)]
        public void GetFragments(Type applicationType, Type scopeType, int count)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType).FirstOrDefault();
            var renderContext = UnitTestFixture.CrerateRenderContextMock(application, [scopeType]);

            // test execution
            var fragments = componentHub.FragmentManager.GetFragments<TestFragmentA, TestSectionA>(application, renderContext?.PageContext?.Scopes).ToList();

            Assert.NotNull(fragments);
            Assert.Equal(count, fragments.Count);
        }

        /// <summary>
        /// Test the process function of the fragment.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSectionA), typeof(TestScopeA))]
        [InlineData(typeof(TestApplicationA), typeof(TestSectionA), typeof(TestScopeB))]
        [InlineData(typeof(TestApplicationA), typeof(TestSectionA), typeof(TestPageB))]
        [InlineData(typeof(TestApplicationA), typeof(TestSectionA), typeof(IScope))]
        public void Process(Type applicationType, Type sectionType, Type scopeType)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType).FirstOrDefault();
            var renderContext = UnitTestFixture.CrerateRenderContextMock(application, [scopeType]);
            var visualTree = new VisualTree();

            // test execution
            var html = componentHub.FragmentManager.Render<IRenderContext, IVisualTree>(renderContext, visualTree, sectionType);

            Assert.NotNull(html);
            Assert.NotEmpty(html.ToString());
        }
    }
}
