using WebExpress.WebCore.Test.Fixture;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebSettingPage;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.Test.Manager
{
    /// <summary>
    /// Test the setting page manager.
    /// </summary>
    [Collection("NonParallelTests")]
    public class UnitTestSettingPageManager
    {
        /// <summary>
        /// Test the register function of the setting page manager.
        /// </summary>
        [Fact]
        public void RegisterSettingPages()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(9, componentHub.SettingPageManager.SettingPages.Count());
        }

        /// <summary>
        /// Test the register function of the setting page manager.
        /// </summary>
        [Fact]
        public void RegisterSettingCategories()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(9, componentHub.SettingPageManager.SettingCategories.Count());
        }

        /// <summary>
        /// Test the register function of the setting page manager.
        /// </summary>
        [Fact]
        public void RegisterSettingGroups()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.Equal(9, componentHub.SettingPageManager.SettingGroups.Count());
        }

        /// <summary>
        /// Test the remove function of the setting page manager.
        /// </summary>
        [Fact]
        public void Remove()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var plugin = componentHub.PluginManager.GetPlugin(typeof(TestPlugin));
            var settingPageManager = componentHub.SettingPageManager as SettingPageManager;

            // test execution
            settingPageManager.Remove(plugin);

            Assert.Empty(componentHub.SettingPageManager.SettingPages);
        }

        /// <summary>
        /// Test the id property of the setting page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageA), "webexpress.webcore.test.testsettingpagea")]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageB), "webexpress.webcore.test.testsettingpageb")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageA), "webexpress.webcore.test.testsettingpagea")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageB), "webexpress.webcore.test.testsettingpageb")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageA), "webexpress.webcore.test.testsettingpagea")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageB), "webexpress.webcore.test.testsettingpageb")]
        public void Id(Type applicationType, Type resourceType, string id)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingPage = componentHub.SettingPageManager.GetSettingPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(id, settingPage.EndpointId.ToString());
        }

        /// <summary>
        /// Test the title property of the setting page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageA), "webindex:settingpagea.label")]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageB), "webindex:settingpageb.label")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageA), "webindex:settingpagea.label")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageB), "webindex:settingpageb.label")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageA), "webindex:settingpagea.label")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageB), "webindex:settingpageb.label")]
        public void Title(Type applicationType, Type resourceType, string title)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingPage = componentHub.SettingPageManager.GetSettingPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(title, settingPage.PageTitle);
        }

        /// <summary>
        /// Test the context path property of the setting page.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageA), "/server/appa")]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingPageB), "/server/appa")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageA), "/server/appb")]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingPageB), "/server/appb")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageA), "/server")]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingPageB), "/server")]
        public void ContextPath(Type applicationType, Type resourceType, string path)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingPage = componentHub.SettingPageManager.GetSettingPages(resourceType, application)?.FirstOrDefault();

            // test execution
            Assert.Equal(path, settingPage.ContextPath);
        }

        /// <summary>
        /// Tests whether the setting page manager implements interface IComponentManager.
        /// </summary>
        [Fact]
        public void IsIComponentManager()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            Assert.True(typeof(IComponentManager).IsAssignableFrom(componentHub.SettingPageManager.GetType()));
        }

        /// <summary>
        /// Tests whether the setting page context implements interface IContext.
        /// </summary>
        [Fact]
        public void IsIContext()
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();

            // test execution
            foreach (var settingPages in componentHub.SettingPageManager.SettingPages)
            {
                Assert.True(typeof(IContext).IsAssignableFrom(settingPages.GetType()), $"Page context {settingPages.GetType().Name} does not implement IContext.");
            }
        }

        /// <summary>
        /// Test the name property of the setting categories.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), new[] { "SettingCategory A", "SettingCategory B", "SettingCategory C" })]
        [InlineData(typeof(TestApplicationB), new[] { "SettingCategory A", "SettingCategory B", "SettingCategory C" })]
        [InlineData(typeof(TestApplicationC), new[] { "SettingCategory A", "SettingCategory B", "SettingCategory C" })]
        public void CategoryName(Type applicationType, params string[] names)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategories = componentHub.SettingPageManager.GetSettingCategories(application);

            // test execution
            Assert.Equal([.. names], [.. settingCategories.Select(x => x.Name)]);
        }

        /// <summary>
        /// Test the icon property of the setting categories.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), new[] { "WebExpress.WebCore.Test.TestIconBell", "WebExpress.WebCore.Test.TestIconProfile", null })]
        [InlineData(typeof(TestApplicationB), new[] { "WebExpress.WebCore.Test.TestIconBell", "WebExpress.WebCore.Test.TestIconProfile", null })]
        [InlineData(typeof(TestApplicationC), new[] { "WebExpress.WebCore.Test.TestIconBell", "WebExpress.WebCore.Test.TestIconProfile", null })]
        public void CategoryIcon(Type applicationType, params string[] icons)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategories = componentHub.SettingPageManager.GetSettingCategories(application);

            // test execution
            Assert.Equal([.. icons], [.. settingCategories.Select(x => x.Icon?.ToString())]);
        }

        /// <summary>
        /// Test the description property of the setting categories.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), new[] { "Description of category a.", "Description of category b.", "Description of category c." })]
        [InlineData(typeof(TestApplicationB), new[] { "Description of category a.", "Description of category b.", "Description of category c." })]
        [InlineData(typeof(TestApplicationC), new[] { "Description of category a.", "Description of category b.", "Description of category c." })]
        public void CategoryDescription(Type applicationType, params string[] descriptions)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategories = componentHub.SettingPageManager.GetSettingCategories(application);

            // test execution
            Assert.Equal([.. descriptions], [.. settingCategories.Select(x => x.Description)]);
        }

        /// <summary>
        /// Test the section property of the setting categories.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), new[] { SettingSection.Preferences, SettingSection.Primary, SettingSection.Secondary })]
        [InlineData(typeof(TestApplicationB), new[] { SettingSection.Preferences, SettingSection.Primary, SettingSection.Secondary })]
        [InlineData(typeof(TestApplicationC), new[] { SettingSection.Preferences, SettingSection.Primary, SettingSection.Secondary })]
        public void CategorySection(Type applicationType, params SettingSection[] sections)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategories = componentHub.SettingPageManager.GetSettingCategories(application);

            // test execution
            Assert.Equal([.. sections], [.. settingCategories.Select(x => x.Section)]);
        }

        /// <summary>
        /// Test the name property of the setting groups.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryA), new[] { "SettingGroup A", "SettingGroup B" })]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingCategoryA), new[] { "SettingGroup A", "SettingGroup B" })]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingCategoryA), new[] { "SettingGroup A", "SettingGroup B" })]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryB), new string[0])]
        [InlineData(typeof(TestApplicationA), null, new[] { "SettingGroup C" })]
        public void GroupName(Type applicationType, Type settingCategoryType, params string[] names)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategory = componentHub.SettingPageManager.GetSettingCategories(application).FirstOrDefault(x => x.CategoryId.ToString() == settingCategoryType?.FullName.ToLower());
            var settinGroups = componentHub.SettingPageManager.GetSettingGroups(application, settingCategory);

            // test execution
            Assert.Equal([.. names], [.. settinGroups.Select(x => x.Name)]);
        }

        /// <summary>
        /// Test the description property of the setting groups.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryA), new[] { "Description of group a.", "Description of group b." })]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingCategoryA), new[] { "Description of group a.", "Description of group b." })]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingCategoryA), new[] { "Description of group a.", "Description of group b." })]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryB), new string[0])]
        [InlineData(typeof(TestApplicationA), null, new[] { "Description of group c." })]
        public void GroupDescription(Type applicationType, Type settingCategoryType, params string[] descriptions)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategory = componentHub.SettingPageManager.GetSettingCategories(application).FirstOrDefault(x => x.CategoryId.ToString() == settingCategoryType?.FullName.ToLower());
            var settinGroups = componentHub.SettingPageManager.GetSettingGroups(application, settingCategory);

            // test execution
            Assert.Equal([.. descriptions], [.. settinGroups.Select(x => x.Description)]);
        }

        /// <summary>
        /// Test the section property of the setting groups.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryA), new[] { SettingSection.Preferences, SettingSection.Primary })]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingCategoryA), new[] { SettingSection.Preferences, SettingSection.Primary })]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingCategoryA), new[] { SettingSection.Preferences, SettingSection.Primary })]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryB), new SettingSection[0])]
        [InlineData(typeof(TestApplicationA), null, new[] { SettingSection.Secondary })]
        public void GroupSection(Type applicationType, Type settingCategoryType, params SettingSection[] sections)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategory = componentHub.SettingPageManager.GetSettingCategories(application).FirstOrDefault(x => x.CategoryId.ToString() == settingCategoryType?.FullName.ToLower());
            var settinGroups = componentHub.SettingPageManager.GetSettingGroups(application, settingCategory);

            // test execution
            Assert.Equal([.. sections], [.. settinGroups.Select(x => x.Section)]);
        }

        /// <summary>
        /// Test the category property of the setting groups.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryA))]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingCategoryA))]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingCategoryA))]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryB))]
        [InlineData(typeof(TestApplicationA), null)]
        public void GroupCategory(Type applicationType, Type settingCategoryType)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategory = componentHub.SettingPageManager.GetSettingCategories(application).FirstOrDefault(x => x.CategoryId.ToString() == settingCategoryType?.FullName.ToLower());
            var settinGroups = componentHub.SettingPageManager.GetSettingGroups(application, settingCategory);

            // test execution
            Assert.Equal(settinGroups.Count(), settinGroups.Where(x => x.SettingCategory == settingCategory).Count());
        }

        /// <summary>
        /// Test the GetFirstSettingPage function of the setting manager.
        /// </summary>
        [Theory]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryA), typeof(TestSettingPageA))]
        [InlineData(typeof(TestApplicationB), typeof(TestSettingCategoryA), typeof(TestSettingPageA))]
        [InlineData(typeof(TestApplicationC), typeof(TestSettingCategoryA), typeof(TestSettingPageA))]
        [InlineData(typeof(TestApplicationA), typeof(TestSettingCategoryB), null)]
        [InlineData(typeof(TestApplicationA), null, typeof(TestSettingPageC))]
        public void GetFirstSettingPage(Type applicationType, Type settingCategoryType, Type firstPageType)
        {
            // preconditions
            var componentHub = UnitTestFixture.CreateAndRegisterComponentHubMock();
            var application = componentHub.ApplicationManager.GetApplications(applicationType)?.FirstOrDefault();
            var settingCategory = componentHub.SettingPageManager.GetSettingCategories(application).FirstOrDefault(x => x.CategoryId.ToString() == settingCategoryType?.FullName.ToLower());
            var firstPage = firstPageType != null ? componentHub.SettingPageManager.GetSettingPages(firstPageType, application).FirstOrDefault() : null;
            var settingPage = componentHub.SettingPageManager.GetFirstSettingPage(application, settingCategory);

            // test execution
            Assert.Equal(firstPage, settingPage);
        }
    }
}
