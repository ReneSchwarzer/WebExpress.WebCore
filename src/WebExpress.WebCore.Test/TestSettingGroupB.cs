using WebExpress.WebCore.WebAttribute;
using WebExpress.WebCore.WebSettingPage;
using WebExpress.WebCore.WebSettingPage.Model;

namespace WebExpress.WebCore.Test
{
    /// <summary>
    /// A dummy setting group for testing purposes.
    /// </summary>
    [Icon("InfoCircle")]
    [Name("SettingGroup B")]
    [Description("Description of group b.")]
    [SettingCategory<TestSettingCategoryA>()]
    [SettingSection(SettingSection.Primary)]
    public sealed class TestSettingGroupB : ISettingGroup
    {
    }
}
